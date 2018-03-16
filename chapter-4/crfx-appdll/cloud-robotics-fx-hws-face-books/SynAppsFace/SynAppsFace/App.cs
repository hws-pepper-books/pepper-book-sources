using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using CloudRoboticsUtil;
using HwsMicrosoftFaceApi.Face;

namespace SynAppsFace
{
    public class App : MarshalByRefObject, IAppRouterDll
    {
        private readonly static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly HttpClient FaceApiHttpClient = new HttpClient();
        private static int ERROR_CODE = 404;

        public JArrayString ProcessMessage(RbAppMasterCache rbappmc, RbAppRouterCache rbapprc, RbHeader rbh, string rbBodyString)
        {
            var results = new JArray();

            var appInfo = JsonConvert.DeserializeObject<JObject>(rbappmc.AppInfo);
            var connectionString = (appInfo["SqlConnectionString"] ?? "").ToString();
            PersonInfoModel.Connection(connectionString);
            PersonFaceInfoModel.Connection(connectionString);

            var rbBody = JsonConvert.DeserializeObject<JObject>(rbBodyString);
            RbMessage rbMessage = null;
            if (rbh.MessageId == "GetFaceInfo")
            {
                rbMessage = GetFaceInfo(rbh, appInfo, rbBody);
            }
            else if (rbh.MessageId == "RegisterFace")
            {
                rbMessage = ResisterPersonFace(rbh, appInfo, rbBody);
            }
            else if (rbh.MessageId == "DeleteFace")
            {
                rbMessage = DeleteFace(rbh, appInfo, rbBody);
            }
            results.Add(JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(rbMessage)));

            return new JArrayString(results);
        }

        private RbMessage GetFaceInfo(RbHeader _rbh, JObject _appInfo, JObject _rbBody)
        {
            var storageAccount   = (_appInfo["StorageAccount"] ?? "").ToString();
            var storageKey       = (_appInfo["StorageKey"] ?? "").ToString();
            var storageContainer = (_appInfo["StorageContainerTemporaryThumbnail"] ?? "").ToString();
            var faceApiKey       = (_appInfo["FaceApiKey"] ?? "").ToString();

            var appbody = new AppBody();
            appbody.PersonGroupId = (_rbBody["PersonGroupId"] ?? "").ToString();

           var buffer = GetFaceImageData(_appInfo, _rbBody, appbody.PersonGroupId, _rbh);
            if (buffer == null) { return new RbMessage(); }

            var facialHairConfidence = (double)_appInfo["FacialHairConfidence"];
            var fd = new Detection(FaceApiHttpClient, faceApiKey, facialHairConfidence);
            var fdResult = fd.DetectFace(buffer);
            appbody.PersonInfos = fdResult.PersonInfos;
            if (fdResult.apiResult.IsSuccessStatusCode)
            {
                var faceDetectJson = JsonConvert.DeserializeObject<JArray>(fdResult.apiResult.Result);
                var faceRectangle = faceDetectJson[0]["faceRectangle"];
                appbody.EmotionBodies = new List<HwsFaceCores.Emotion>();
                foreach (var p in appbody.PersonInfos)
                {
                    appbody.EmotionBodies.Add(p.FaceAttributes.Emotion);
                }

                var fiResult = new HwsFaceCores.AppResult();
                var fi = new Identification(FaceApiHttpClient, faceApiKey, facialHairConfidence);
                fiResult = fi.IdentifyFace(buffer, appbody.PersonInfos, appbody.PersonGroupId);
                appbody.PersonInfos = fiResult.PersonInfos;
                if (fiResult.apiResult.IsSuccessStatusCode)
                {
                    var personInfoQueryResult = CreateOrUpdatePersonInfo(appbody);
                    if (personInfoQueryResult.apiResult.IsSuccessStatusCode)
                    {
                        appbody = personInfoQueryResult.appBody;
                        foreach (var info in appbody.PersonInfos)
                        {
                            CreatePersonFaceInfo(appbody, info);

                            var faceFileName = info.PersonId;
                        }
                    }
                    else
                    {
                        _rbh.ProcessingStack += " " + personInfoQueryResult.apiResult.Message;
                    }
                }
                else
                {
                    _rbh.ProcessingStack += " " + fiResult.apiResult.Message;
                }
            }
            else
            {
                RbMessage noFaceDetectError = new RbMessage();
                noFaceDetectError.RbBody = JsonConvert.DeserializeObject<JObject>("{\"Code\": 404, \"Message\": \"画像に人の顔が含まれていません.\", \"PersonInfos\": [], \"EmotionBodies\": []}");
                _rbh.MessageId = "Res" + _rbh.MessageId;
                noFaceDetectError.RbHeader = _rbh;
                return noFaceDetectError;
            }

            return GetRbMessage(_rbh, appbody);
        }

        private RbMessage ResisterPersonFace(RbHeader _rbh, JObject _appInfo, JObject _rbBody)
        {
            var faceApiKey = (_appInfo["FaceApiKey"] ?? "").ToString();
            var facialHairConfidence = (double)_appInfo["FacialHairConfidence"];

            var appbody = new AppBody();
            appbody.PersonName = (_rbBody["PersonName"] ?? "").ToString();
            appbody.PersonNameYomi = (_rbBody["PersonNameYomi"] ?? "").ToString();
            appbody.PersonGroupId = (_rbBody["PersonGroupId"] ?? "").ToString();
            appbody.PersonId = (_rbBody["PersonId"] ?? "").ToString();

            var buffer = GetFaceImageData(_appInfo, _rbBody, appbody.PersonGroupId, _rbh);

            var faceDetection = new Detection(FaceApiHttpClient, faceApiKey, 0.1);
            var faceDetectResult = faceDetection.DetectFace(buffer);
            if (faceDetectResult.apiResult.IsSuccessStatusCode)
            {
                appbody.PersonInfos = faceDetectResult.PersonInfos;
                var faceDetectJson = JsonConvert.DeserializeObject<JArray>(faceDetectResult.apiResult.Result);
                var faceRectangle = faceDetectJson[0]["faceRectangle"];
                if (appbody.PersonId == "")
                {
                    var fiResult = new HwsFaceCores.AppResult();
                    var fi = new Identification(FaceApiHttpClient, faceApiKey, facialHairConfidence);
                    fiResult = fi.IdentifyFace(buffer, appbody.PersonInfos, appbody.PersonGroupId);
                    appbody.PersonInfos = fiResult.PersonInfos;
                    if (fiResult.apiResult.IsSuccessStatusCode)
                    {
                        if (appbody.PersonInfos.Count > 0)
                        {
                            RbMessage faceDuplicatedError = new RbMessage();
                            faceDuplicatedError.RbBody = JsonConvert.DeserializeObject<JObject>("{\"Code\": 406, \"Message\": \"顔情報は登録済みです.\", \"PersonInfos\": [], \"EmotionBodies\": []}");
                            _rbh.MessageId = "Res" + _rbh.MessageId;
                            faceDuplicatedError.RbHeader = _rbh;
                            return faceDuplicatedError;
                        }
                    }

                    var facePerson = new HwsMicrosoftFaceApi.Person.Client(faceApiKey);
                    var personCreateResult = facePerson.Create(appbody.PersonGroupId, appbody.PersonName);
                    if (personCreateResult.apiResult.IsSuccessStatusCode)
                    {
                        var result = (JObject)JsonConvert.DeserializeObject(personCreateResult.apiResult.Result);
                        appbody.PersonId = (result["personId"] ?? "").ToString();
                        appbody = ResisterFace(_appInfo, appbody, buffer, faceRectangle);
                    }
                }
                else
                {
                    appbody = ResisterFace(_appInfo, appbody, buffer, faceRectangle);
                }
            }
            else
            {
                RbMessage noFaceDetectError = new RbMessage();
                noFaceDetectError.RbBody = JsonConvert.DeserializeObject<JObject>("{\"Code\": 404, \"Message\": \"画像に人の顔が含まれていません.\", \"PersonInfos\": [], \"EmotionBodies\": []}");
                _rbh.MessageId = "Res" + _rbh.MessageId;
                noFaceDetectError.RbHeader = _rbh;
                return noFaceDetectError;
            }

            return GetRbMessage(_rbh, appbody);
        }
        private AppBody ResisterFace(JObject _appInfo, AppBody _appbody, byte[] _faceImageBuffer, JToken _faceRectangle)
        {
            var storageAccount      = (_appInfo["StorageAccount"] ?? "").ToString();
            var storageKey          = (_appInfo["StorageKey"] ?? "").ToString();
            var storageContainer    = (_appInfo["StorageContainer"] ?? "").ToString();
            var faceApiKey          = (_appInfo["FaceApiKey"] ?? "").ToString();

            var facePerson = new HwsMicrosoftFaceApi.Person.Client(faceApiKey);
            var personAddResult = facePerson.Add(_faceImageBuffer, _appbody.PersonGroupId, _appbody.PersonId);
            if (personAddResult.apiResult.IsSuccessStatusCode)
            {
                _appbody.PersonInfos[0].PersonId = _appbody.PersonId;
                _appbody.PersonInfos[0].PersonName = _appbody.PersonName;
                _appbody.PersonInfos[0].PersonNameYomi = _appbody.PersonNameYomi;
                CreateOrUpdatePersonInfo(_appbody);

                var addFaceResult = JsonConvert.DeserializeObject<JObject>(personAddResult.apiResult.Result);
                var persistedFaceId = (addFaceResult["persistedFaceId"] ?? "").ToString();
                var faceFileName = _appbody.PersonId + "_" + persistedFaceId;

                var personGroup = new HwsMicrosoftFaceApi.PersonGroup.Client(faceApiKey);
                personGroup.Train(_appbody.PersonGroupId);
            }

            return _appbody;
        }

        private RbMessage DeleteFace(RbHeader rbh, JObject appInfo, JObject jo_input)
        {
            var storageAccount      = (appInfo["StorageAccount"] ?? "").ToString();
            var storageKey          = (appInfo["StorageKey"] ?? "").ToString();
            var storageContainer    = (appInfo["StorageContainer"] ?? "").ToString();
            var faceApiKey          = (appInfo["FaceApiKey"] ?? "").ToString();

            AppBody appbody = new AppBody();
            appbody.PersonId = (jo_input["PersonId"] ?? "").ToString();
            appbody.PersonGroupId = (jo_input["PersonGroupId"] ?? "").ToString();

            var facePerson = new HwsMicrosoftFaceApi.Person.Client(faceApiKey);
            var personDeleteResult = facePerson.Delete(appbody.PersonGroupId, appbody.PersonId);
            if (personDeleteResult.apiResult.IsSuccessStatusCode)
            {
                var personGroup = new HwsMicrosoftFaceApi.PersonGroup.Client(faceApiKey);
                personGroup.Train(appbody.PersonGroupId);

                PersonInfoModel.Find(appbody.PersonId).Delete();
            } else
            {
                appbody.Code = ERROR_CODE;
            }

            return GetRbMessage(rbh, appbody);
        }

        private RbMessage GetRbMessage(RbHeader _rbh, AppBody _appbody)
        {
            RbMessage message = new RbMessage();
            _rbh.MessageId = "Res" + _rbh.MessageId;
            message.RbHeader = _rbh;
            message.RbBody = _appbody;

            return message;
        }

        private byte[] GetFaceImageData(JObject _appInfo, JObject _rbBody, string _personGroupId, RbHeader _rbh)
        {
            string base64Image = (_rbBody["Face"] ?? "").ToString();
            if (base64Image == "" && _personGroupId == "") return null;

            return System.Convert.FromBase64String(base64Image);
        }

        private AppResult CreateOrUpdatePersonInfo(AppBody _appbody)
        {
            AppResult appResult = new AppResult();
            ApiResult apiResult = new ApiResult();
            apiResult.IsSuccessStatusCode = true;

            var now = DateTime.UtcNow;
            foreach (var personInfo in _appbody.PersonInfos)
            {
                try
                {
                    var model            = PersonInfoModel.Find(personInfo.PersonId);
                    var lastDetectedAt = model.LastDetectedAt.HasValue ? model.LastDetectedAt.Value : now;
                    var registeredAt   = model.CreatedAt.HasValue ? model.CreatedAt.Value : now;

                    model.PersonGroupId  = _appbody.PersonGroupId;
                    model.PersonId       = personInfo.PersonId;
                    model.PersonName     = personInfo.PersonName ?? model.PersonName;
                    model.PersonNameYomi = personInfo.PersonNameYomi ?? model.PersonNameYomi;
                    model.LastDetectedAt = now;
                    model.Save();

                    personInfo.PersonName     = model.PersonName;
                    personInfo.PersonNameYomi = model.PersonNameYomi;
                    personInfo.LastDetectedAt = (long)lastDetectedAt.Subtract(UnixEpoch).TotalSeconds;
                    personInfo.RegisteredAt   = (long)registeredAt.Subtract(UnixEpoch).TotalSeconds;
                }
                catch (Exception ex)
                {
                    apiResult.IsSuccessStatusCode = false;
                    apiResult.Message = ex.Message;
                }
            }

            appResult.apiResult = apiResult;
            appResult.appBody   = _appbody;

            return appResult;
        }

        private AppResult CreatePersonFaceInfo(AppBody _appbody, HwsFaceCores.PersonInfo _faceDetectBody)
        {
            AppResult appResult = new AppResult();
            ApiResult apiResult = new ApiResult();

            try
            {
                apiResult.IsSuccessStatusCode = true;
                var model = PersonFaceInfoModel.New();
                model.PersonId = _faceDetectBody.PersonId;
                model.FaceConfidence = _faceDetectBody.FaceConfidence;
                model.FaceRectangle_Width = _faceDetectBody.FaceRectangle.Width;
                model.FaceRectangle_Height = _faceDetectBody.FaceRectangle.Height;
                model.FaceRectangle_Left = _faceDetectBody.FaceRectangle.Left;
                model.FaceRectangle_Top = _faceDetectBody.FaceRectangle.Top;
                model.FaceLandmarks_PupilLeft_X = _faceDetectBody.FaceLandmarks.PupilLeftX;
                model.FaceLandmarks_PupilLeft_Y = _faceDetectBody.FaceLandmarks.PupilLeftY;
                model.FaceLandmarks_PupilRight_X = _faceDetectBody.FaceLandmarks.PupilRightX;
                model.FaceLandmarks_PupilRight_Y = _faceDetectBody.FaceLandmarks.PupilRightY;
                model.FaceLandmarks_NoseTip_X = _faceDetectBody.FaceLandmarks.NoseTipX;
                model.FaceLandmarks_NoseTip_Y = _faceDetectBody.FaceLandmarks.NoseTipY;
                model.FaceLandmarks_MouseLeft_X = _faceDetectBody.FaceLandmarks.MouthLeftX;
                model.FaceLandmarks_MouseLeft_Y = _faceDetectBody.FaceLandmarks.MouthLeftY;
                model.FaceLandmarks_MouseRight_X = _faceDetectBody.FaceLandmarks.MouthRightX;
                model.FaceLandmarks_MouseRight_Y = _faceDetectBody.FaceLandmarks.MouthRightY;
                model.FaceLandmarks_EyebrowLeftOuter_X = _faceDetectBody.FaceLandmarks.EyebrowLeftOuterX;
                model.FaceLandmarks_EyebrowLeftOuter_Y = _faceDetectBody.FaceLandmarks.EyebrowLeftOuterY;
                model.FaceLandmarks_EyebrowLeftInner_X = _faceDetectBody.FaceLandmarks.EyebrowLeftInnerX;
                model.FaceLandmarks_EyebrowLeftInner_Y = _faceDetectBody.FaceLandmarks.EyebrowLeftInnerY;
                model.FaceLandmarks_EyeLeftOuter_X = _faceDetectBody.FaceLandmarks.EyeLeftOuterX;
                model.FaceLandmarks_EyeLeftOuter_Y = _faceDetectBody.FaceLandmarks.EyeLeftOuterY;
                model.FaceLandmarks_EyeLeftTop_X = _faceDetectBody.FaceLandmarks.EyeLeftTopX;
                model.FaceLandmarks_EyeLeftTop_Y = _faceDetectBody.FaceLandmarks.EyeLeftTopY;
                model.FaceLandmarks_EyeLeftBottom_X = _faceDetectBody.FaceLandmarks.EyeLeftBottomX;
                model.FaceLandmarks_EyeLeftBottom_Y = _faceDetectBody.FaceLandmarks.EyeLeftBottomY;
                model.FaceLandmarks_EyeLeftInner_X = _faceDetectBody.FaceLandmarks.EyeLeftInnerX;
                model.FaceLandmarks_EyeLeftInner_Y = _faceDetectBody.FaceLandmarks.EyeLeftInnerY;
                model.FaceLandmarks_EyebrowRightInner_X = _faceDetectBody.FaceLandmarks.EyebrowRightInnerX;
                model.FaceLandmarks_EyebrowRightInner_Y = _faceDetectBody.FaceLandmarks.EyebrowRightInnerY;
                model.FaceLandmarks_EyebrowRightOuter_X = _faceDetectBody.FaceLandmarks.EyebrowRightOuterX;
                model.FaceLandmarks_EyebrowRightOuter_Y = _faceDetectBody.FaceLandmarks.EyebrowRightOuterY;
                model.FaceLandmarks_EyeRightInner_X = _faceDetectBody.FaceLandmarks.EyeRightInnerX;
                model.FaceLandmarks_EyeRightInner_Y = _faceDetectBody.FaceLandmarks.EyeRightInnerY;
                model.FaceLandmarks_EyeRightTop_X = _faceDetectBody.FaceLandmarks.EyeRightTopX;
                model.FaceLandmarks_EyeRightTop_Y = _faceDetectBody.FaceLandmarks.EyeRightTopY;
                model.FaceLandmarks_EyeRightBottom_X = _faceDetectBody.FaceLandmarks.EyeRightBottomX;
                model.FaceLandmarks_EyeRightBottom_Y = _faceDetectBody.FaceLandmarks.EyeRightBottomY;
                model.FaceLandmarks_EyeRightOuter_X = _faceDetectBody.FaceLandmarks.EyeRightOuterX;
                model.FaceLandmarks_EyeRightOuter_Y = _faceDetectBody.FaceLandmarks.EyeRightOuterY;
                model.FaceLandmarks_NoseRootLeft_X = _faceDetectBody.FaceLandmarks.NoseRootLeftX;
                model.FaceLandmarks_NoseRootLeft_Y = _faceDetectBody.FaceLandmarks.NoseRootLeftY;
                model.FaceLandmarks_NoseRootRight_X = _faceDetectBody.FaceLandmarks.NoseRootRightX;
                model.FaceLandmarks_NoseRootRight_Y = _faceDetectBody.FaceLandmarks.NoseRootRightY;
                model.FaceLandmarks_NoseLeftAlarTop_X = _faceDetectBody.FaceLandmarks.NoseLeftAlarTopX;
                model.FaceLandmarks_NoseLeftAlarTop_Y = _faceDetectBody.FaceLandmarks.NoseLeftAlarTopY;
                model.FaceLandmarks_NoseRightAlarTop_X = _faceDetectBody.FaceLandmarks.NoseRightAlarTopX;
                model.FaceLandmarks_NoseRightAlarTop_Y = _faceDetectBody.FaceLandmarks.NoseRightAlarTopY;
                model.FaceLandmarks_NoseLeftAlarOutTip_X = _faceDetectBody.FaceLandmarks.NoseLeftAlarOutTipX;
                model.FaceLandmarks_NoseLeftAlarOutTip_Y = _faceDetectBody.FaceLandmarks.NoseLeftAlarOutTipY;
                model.FaceLandmarks_NoseRightAlarOutTip_X = _faceDetectBody.FaceLandmarks.NoseRightAlarOutTipX;
                model.FaceLandmarks_NoseRightAlarOutTip_Y = _faceDetectBody.FaceLandmarks.NoseRightAlarOutTipY;
                model.FaceLandmarks_UpperLipTop_X = _faceDetectBody.FaceLandmarks.UpperLipTopX;
                model.FaceLandmarks_UpperLipTop_Y = _faceDetectBody.FaceLandmarks.UpperLipTopY;
                model.FaceLandmarks_UpperLipBottom_X = _faceDetectBody.FaceLandmarks.UpperLipBottomX;
                model.FaceLandmarks_UpperLipBottom_Y = _faceDetectBody.FaceLandmarks.UpperLipBottomY;
                model.FaceLandmarks_UnderLipTop_X = _faceDetectBody.FaceLandmarks.UnderLipTopX;
                model.FaceLandmarks_UnderLipTop_Y = _faceDetectBody.FaceLandmarks.UnderLipTopY;
                model.FaceLandmarks_UnderLipBottom_X = _faceDetectBody.FaceLandmarks.UnderLipBottomX;
                model.FaceLandmarks_UnderLipBottom_Y = _faceDetectBody.FaceLandmarks.UnderLipBottomY;
                model.FaceAttributes_Age = _faceDetectBody.FaceAttributes.Age;
                model.FaceAttributes_Gender = _faceDetectBody.FaceAttributes.Gender;
                model.FaceAttributes_Smile = _faceDetectBody.FaceAttributes.Smile;
                model.FaceAttributes_FacialHair_Moustache = _faceDetectBody.FaceAttributes.FacialHair.Moustache;
                model.FaceAttributes_FacialHair_Beard = _faceDetectBody.FaceAttributes.FacialHair.Beard;
                model.FaceAttributes_FacialHair_Sideburns = _faceDetectBody.FaceAttributes.FacialHair.Sideburns;
                model.FaceAttributes_Glasses = _faceDetectBody.FaceAttributes.Glasses;
                model.FaceAttributes_HeadPose_Roll = _faceDetectBody.FaceAttributes.HeadPose.Roll;
                model.FaceAttributes_HeadPose_Yaw = _faceDetectBody.FaceAttributes.HeadPose.Yaw;
                model.FaceAttributes_HeadPose_Pitch = _faceDetectBody.FaceAttributes.HeadPose.Pitch;

                model.Save();
            }
            catch (Exception ex)
            {
                apiResult.IsSuccessStatusCode = false;
                apiResult.Message = ex.Message;
            }
            appResult.apiResult = apiResult;
            appResult.appBody   = _appbody;

            return appResult;
        }
    }

}

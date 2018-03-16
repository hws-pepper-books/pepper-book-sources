using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using HwsFaceCores;

namespace HwsMicrosoftFaceApi.Face
{
    public class Identification
    {
        private HttpClient httpClient;
        private double facialHairConfidence;

        public Identification(HttpClient _httpClient, string faceApiKey, double facialHairConfidence)
        {
            this.httpClient = _httpClient;
            if (!this.httpClient.DefaultRequestHeaders.Contains("Ocp-Apim-Subscription-Key"))
            {
                this.httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", faceApiKey);
            }
            this.facialHairConfidence = facialHairConfidence;
        }

        public AppResult IdentifyFace(byte[] buffer, List<PersonInfo> personInfos, string personGroupId)
        {
            var appResult = new AppResult();
            var apiResult = new ApiResult();

            var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/identify";

            // Request body
            var reqestBody = new JObject();
            reqestBody["personGroupId"] = personGroupId;
            reqestBody["confidenceThreshold"] = this.facialHairConfidence;
            var faceIds = new JArray();
            foreach(var personInfo in personInfos)
            {
                faceIds.Add(personInfo.FaceId.ToString());
            }
            reqestBody["faceIds"] = faceIds;
            reqestBody["maxNumOfCandidatesReturned"] = 1;

            byte[] byteData = Encoding.UTF8.GetBytes((string)JsonConvert.SerializeObject(reqestBody));

            // Call REST API
            var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = this.httpClient.PostAsync(uri, content);
            response.Wait();

            apiResult.IsSuccessStatusCode = response.Result.IsSuccessStatusCode;
            if (apiResult.IsSuccessStatusCode)
            {
                var resdata = response.Result.Content.ReadAsStringAsync();
                resdata.Wait();
                apiResult.Result = resdata.Result.ToString();
            }
            else
            {
                apiResult.Message = response.Result.ToString();
                appResult.apiResult = apiResult;
                appResult.PersonInfos = personInfos;
                return appResult;
            }

            var result = apiResult.Result;
            var faceIdentities = (JArray)JsonConvert.DeserializeObject(result);

            foreach (var identity in faceIdentities)
            {
                apiResult.IsSuccessStatusCode = false;
                if (! identity["candidates"].HasValues)
                {
                    continue;
                }

                var personId = (identity["candidates"][0]["personId"] ?? "").ToString();
                var confidence = (double)identity["candidates"][0]["confidence"];
                confidence = Math.Floor(confidence * 100) / 100;
                var _condidence = confidence.ToString();

                foreach (var personInfo in personInfos)
                {
                    if (personInfo.FaceId == identity["faceId"].ToString())
                    {
                        personInfo.PersonId = personId;
                        personInfo.FaceConfidence = _condidence;
                    }
                }
                apiResult.IsSuccessStatusCode = true;
            }

            appResult.apiResult = apiResult;
            appResult.PersonInfos = personInfos;

            return appResult;
        }
    }
}

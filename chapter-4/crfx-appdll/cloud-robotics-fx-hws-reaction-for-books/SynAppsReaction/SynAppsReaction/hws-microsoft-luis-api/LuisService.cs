using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SynAppsLuis
{
    public class LuisService
    {
        private const string EndPoint = "https://westus.api.cognitive.microsoft.com/luis/api/v2.0/apps/";
        private const string TrainingStatusInProgress = "3";
        private HttpClient httpClient = new HttpClient();
        private string DeviceId;
        private string luisAppId;
        private string luisSubscriptionKey;
        private string luisVersionId;

        public LuisService(string _deviceId, string _luisAppId, string _luisSubscriptionKey, string _luisProgrammaticAPIKey, string _luisVersionId, string _connectionString)
        {
            this.DeviceId            = _deviceId;
            this.luisAppId           = _luisAppId;
            this.luisSubscriptionKey = _luisSubscriptionKey;
            this.luisVersionId       = _luisVersionId;

            this.httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _luisProgrammaticAPIKey);

            SynAppsIntentModel.Connection(_connectionString);
            ConversationHistoryModel.Connection(_connectionString);
            PredictionIntentModel.Connection(_connectionString);
            PredictionEntityModel.Connection(_connectionString);
            SynAppsSyncStatusModel.Connection(_connectionString);
        }
        public LuisService(string deviceId, JObject appInfo) : this(deviceId, appInfo["LuisAppId"].ToString(), appInfo["LuisSubscriptionKey"].ToString(), appInfo["LuisProgrammaticAPIKey"].ToString(), appInfo["LuisVersionId"].ToString(), appInfo["SqlConnectionString"].ToString())
        {
        }

        public List<string> GetIntents()
        {
            var intents = new List<string>();

            var response = this.httpClient.GetAsync($"{EndPoint}{this.luisAppId}/versions/{this.luisVersionId}/intents").Result;

            if (response.IsSuccessStatusCode)
            {
                var resdata = response.Content.ReadAsStringAsync().Result;
                var jsonResults = JsonConvert.DeserializeObject<JArray>(resdata);
                foreach (var intent in jsonResults)
                {
                    intents.Add(intent["name"].ToString());
                }
            }

            return intents;
        }

        public List<string> GetEntities()
        {
            var entities = new List<string>();

            var response = this.httpClient.GetAsync($"{EndPoint}{this.luisAppId}/versions/{this.luisVersionId}/entities").Result;
            if (response.IsSuccessStatusCode)
            {
                var resdata = response.Content.ReadAsStringAsync().Result;
                var jsonResults = JsonConvert.DeserializeObject<JArray>(resdata);
                foreach (var entity in jsonResults)
                {
                    entities.Add(entity["name"].ToString());
                }
            }

            entities.AddRange(GetHierarchicalEntities());

            return entities;
        }

        public List<string> GetHierarchicalEntities()
        {
            var entities = new List<string>();

            var response = this.httpClient.GetAsync($"{EndPoint}{this.luisAppId}/versions/{this.luisVersionId}/hierarchicalentities").Result;
            if (response.IsSuccessStatusCode)
            {
                var resdata = response.Content.ReadAsStringAsync().Result;
                var jsonResults = JsonConvert.DeserializeObject<JArray>(resdata);
                foreach (var entity in jsonResults)
                {
                    foreach (var child in entity["children"])
                    {
                        entities.Add(entity["name"].ToString() + "::" + child["name"].ToString());
                    }
                }
            }

            return entities;
        }

        public List<PreviewLabeledExampleDto> GetPreviewLabeledExamples(int _skip = 0, int _take = 500)
        {
            var results = new List<PreviewLabeledExampleDto>();

            var response = this.httpClient.GetAsync($"{EndPoint}{this.luisAppId}/versions/{this.luisVersionId}/examples?skip={_skip}&take={_take}").Result;
            if (response.IsSuccessStatusCode)
            {
                var resdata = response.Content.ReadAsStringAsync().Result;
                var jsonResults = JsonConvert.DeserializeObject<JArray>(resdata);
                foreach (var json in jsonResults)
                {
                    var dto = new PreviewLabeledExampleDto(json);
                    results.Add(dto);
                }
            }

            return results;
        }

        public bool IsRefreshPredictions()
        {
            var result = false;

            var m = SynAppsSyncStatusModel.FindBySynAppsDeviceId(this.DeviceId);
            var statuses = TrainingSatus();
            foreach (var r in statuses)
            {
                if (r["details"] != null && r["details"]["trainingDateTime"] != null)
                {
                    var trainingDateTime = DateTimeOffset.Parse(r["details"]["trainingDateTime"].ToString());
                    if (trainingDateTime.CompareTo(m.LastTrainingDateTime) > 0)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public ApiResult RefreshPredictions()
        {
            var apiResult = new ApiResult { StatusCode = StatusCode.Success };

            var intentList = new List<PredictionIntentModel>();
            var entityList = new List<PredictionEntityModel>();
            var list = new List<PreviewLabeledExampleDto>();

            var r = GetPreviewLabeledExamples();
            list.AddRange(r);
            while (r.Count >= 500)
            {
                r = GetPreviewLabeledExamples(list.Count);
                list.AddRange(r);
            }

            foreach (var l in list)
            {
                var intentModel = PredictionIntentModel.New();
                intentModel.SynAppsDeviceId = this.DeviceId;
                intentModel.LuisExampleId = l.Id;
                intentModel.Intent = l.IntentLabel;
                intentList.Add(intentModel);

                foreach (var e in l.EntityLabels)
                {
                    var entityModel = PredictionEntityModel.New();
                    entityModel.SynAppsDeviceId = this.DeviceId;
                    entityModel.LuisExampleId   = l.Id;
                    entityModel.EntityName      = e.EntityName;
                    entityModel.EntityValue     = e.EntityValue;
                    entityList.Add(entityModel);
                }
            }

            var intentResult = PredictionIntentModel.Refresh(this.DeviceId, intentList);
            if (intentResult.StatusCode == StatusCode.Success)
            {
                var entityResult = PredictionEntityModel.Refresh(this.DeviceId, entityList);
                if (entityResult.StatusCode == StatusCode.Success)
                {
                    var status = SynAppsSyncStatusModel.FindBySynAppsDeviceId(this.DeviceId);
                    status.LastTrainingDateTime = DateTime.UtcNow;
                    status.Save();
                }
                else
                {
                    apiResult.StatusCode = StatusCode.Error;
                    apiResult.Message = entityResult.Message;
                }
            }
            else
            {
                apiResult.StatusCode = StatusCode.Error;
                apiResult.Message = intentResult.Message;
            }

            return apiResult;
        }

        public ConversationHistoryModel GetUnKnownMessage()
        {
            return ConversationHistoryModel.FindOneOfUnknownMessageByDeviceId(this.DeviceId);
        }

        public List<ConversationHistoryModel> GetLearningMessages()
        {
            return ConversationHistoryModel.FindAllByDeviceId(this.DeviceId, ConversationStatus.Learning);
        }

        public ConversationHistoryModel GetLearningMessage()
        {
            return ConversationHistoryModel.FindByDeviceId(this.DeviceId, ConversationStatus.Learning);
        }

        public void StartLearning(long exampleTextId)
        {
            StartLearning(ConversationHistoryModel.FindById(exampleTextId));
        }

        public void StartLearning(ConversationHistoryModel model)
        {
            model.Status = ConversationStatus.Learning;
            model.Save();
        }

        public List<ConversationHistoryModel> GetLearnedMessage()
        {
            return ConversationHistoryModel.FindAllByDeviceId(this.DeviceId, ConversationStatus.Learned);
        }

        public List<ConversationHistoryModel> GetDenyTeachMessage()
        {
            return ConversationHistoryModel.FindAllByDeviceId(this.DeviceId, ConversationStatus.DenyTeach);
        }

        public List<ConversationHistoryModel> GetSynAppsUnlinks()
        {
            return ConversationHistoryModel.FindAllByDeviceIdAndSynAppsUnlink(this.DeviceId);
        }

        public void NotificationFinished(long exampleTextId)
        {
            NotificationFinished(ConversationHistoryModel.FindById(exampleTextId));
        }
        public void NotificationFinished(ConversationHistoryModel model)
        {
            model.Status = ConversationStatus.Notified;
            model.Save();
        }

        public void ResetLearning(long _conversationHistoryId)
        {
            var model = ConversationHistoryModel.FindById(_conversationHistoryId);
            if (model != null && model.Status != ConversationStatus.None)
            {
                model.Status = ConversationStatus.None;
                model.Save();
            }
        }

        public string GetUnKnownMessageText()
        {
            return "申し訳ありません。ご要望にお答えできません。";
        }

        public ApiResult AddExample(LuisExample example)
        {
            var result = new ApiResult { StatusCode = StatusCode.Success, Message = "" };
            byte[] payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(example, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            var content = new ByteArrayContent(payload);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = this.httpClient.PostAsync($"{EndPoint}{this.luisAppId}/versions/{this.luisVersionId}/example", content).Result;
            if (!response.IsSuccessStatusCode)
            {
                result.StatusCode = (int)response.StatusCode;
                result.Message = response.ReasonPhrase;
            }

            return result;
        }

        public void TrainAndPublish(long exampleTextId)
        {
            this.Train(exampleTextId);
            this.Publish();
        }

        public ApiResult Train()
        {
            var result = new ApiResult { StatusCode = StatusCode.Success, Message = "" };

            var content = new StringContent("{body}");
            var response = this.httpClient.PostAsync($"{EndPoint}{this.luisAppId}/versions/{this.luisVersionId}/train", content).Result;
            if (!response.IsSuccessStatusCode)
            {
                result.StatusCode = StatusCode.Error;
                result.Message = response.Content.ToString();
            }

            return result;
        }

        public void Train(long exampleTextId)
        {
            this.Train();

            var model = ConversationHistoryModel.FindById(exampleTextId);
            model.Learned();
        }

        public void DenyTeach(long exampleTextId)
        {
            var model = ConversationHistoryModel.FindById(exampleTextId);
            model.DenyTeach();
        }

        public JArray TrainingSatus()
        {
            var response = this.httpClient.GetAsync($"{EndPoint}{this.luisAppId}/versions/{this.luisVersionId}/train").Result;
            if (response.IsSuccessStatusCode)
            {
                var resdata = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<JArray>(resdata.ToString());
            }

            return JsonConvert.DeserializeObject<JArray>("[{\"details: {\"statusId: 4\"}\"}]");
        }

        public ApiResult Publish(int recursiveCount = 0)
        {
            var result = new ApiResult { StatusCode = StatusCode.Success, Message = "" };

            var status = this.TrainingSatus();
            if (status[0]["details"]["statusId"].ToString() == TrainingStatusInProgress && recursiveCount < 10)
            {
                System.Threading.Thread.Sleep(1000);
                recursiveCount += 1;
                Publish(recursiveCount);
            }

            var content = new StringContent("{\"versionId\": \"" + this.luisVersionId + "\", \"isStaging\": \"false\"}");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = this.httpClient.PostAsync($"{EndPoint}{this.luisAppId}/publish", content).Result;
            if (!response.IsSuccessStatusCode)
            {
                result.StatusCode = StatusCode.Error;
                result.Message = response.Content.ToString();
            }

            return result;
        }

        public ApiResult AddMessageText(MessageTextDto dto)
        {
            return ConversationHistoryModel.AddMessageText(
                    this.DeviceId, 
                    dto.FromUserId, 
                    dto.FromMessage, 
                    dto.ToMessage, 
                    dto.Intent, 
                    dto.Score, 
                    dto.SynAppsAccountId, 
                    dto.SynAppsAccountName, 
                    dto.SynAppsAssetId, 
                    dto.Status
                );
        }

        public void SaveReaction(LuisExample example, string action, long _exampleTextId)
        {
            string entity_values = "";
            foreach (var label in example.EntityLabels)
            {
                var value = example.Text.Substring(label.StartCharIndex, (label.EndCharIndex - label.StartCharIndex + 1)) + ",";
                entity_values += value;
            }
            entity_values = entity_values.Trim(',');

            var model = SynAppsIntentModel.New();
            model.SynAppsDeviceId = this.DeviceId;
            model.Name = example.IntentName;
            model.Entity = entity_values;
            model.ReactionBody = "{\"Talk\": \"" + action + "\"}";
            model.IsSynAppsLinked = false;
            model.Save();

            if (_exampleTextId > 0)
            {
                var m = ConversationHistoryModel.FindById(_exampleTextId);
                m.ToMessage = action;
                m.IsSynAppsLinked = false;
                m.Save();
            }
        }

        public ApiResult AdvancedLearning(string learningJson, string exampleText, long _exampleTextId)
        {
            var result = new ApiResult { StatusCode = StatusCode.Success, Message = "" };
            try
            {
                var learningObject = JsonConvert.DeserializeObject<JObject>(learningJson);
                var luisExample = new LuisExample();
                luisExample.Text = exampleText;
                luisExample.IntentName = (learningObject["intents"] ?? "").ToString();
                if (luisExample.IntentName == "") { throw new Exception("intent must have some value."); }
                var action = (learningObject["action"] ?? "").ToString();
                if (action == "") { throw new Exception("action must have some value."); }
                var entityLabels = new List<LuisEntityLabel>();
                foreach (var entity in learningObject["entities"])
                {
                    var luisEntityLabel = new LuisEntityLabel();
                    luisEntityLabel.EntityName = (entity["name"] ?? "").ToString();
                    var entityValue = (entity["value"] ?? "").ToString();
                    if (luisEntityLabel.EntityName == "" || entityValue == "") { throw new Exception("entity name and value must have some value."); }
                    if (! exampleText.Contains(entityValue)) { throw new Exception("json text must contain entity value in example text."); }

                    luisEntityLabel.StartCharIndex = exampleText.IndexOf(entityValue);
                    luisEntityLabel.EndCharIndex = luisEntityLabel.StartCharIndex + entityValue.Length - 1;

                    entityLabels.Add(luisEntityLabel);
                }
                luisExample.EntityLabels = entityLabels;

                result = AddExample(luisExample);
                if (result.StatusCode == StatusCode.Success)
                {
                    SaveReaction(luisExample, action, _exampleTextId);
                }
            }
            catch (Exception e)
            {
                result.StatusCode = StatusCode.Error;
                result.Message = e.StackTrace;
                Trace.TraceError(e.Message);
                Trace.TraceError(e.StackTrace);
            }

            return result;
        }

        public ApiResult BatchLearning(string _learningJson)
        {
            var learningObject = JsonConvert.DeserializeObject<JObject>(_learningJson);
            var exampleText = (learningObject["text"] ?? "").ToString();
            if (exampleText == "") { throw new Exception("text(utterance) must have some value."); }

            var result = AdvancedLearning(_learningJson, exampleText, 0);
            if (result.StatusCode == StatusCode.Success)
            {
                var messages = ConversationHistoryModel.FindAllOfUnknownMessageByDeviceIdAndFromMessage(this.DeviceId, exampleText);
                foreach (var message in messages)
                {
                    message.ToMessage = (learningObject["action"] ?? "").ToString();
                    message.Learned();
                }
            }

            return result;
        }

        public RobotBehavior CreateRobotBehavior(string talk, bool isDialogEnabled = false)
        {
            RobotBehavior result = null;
            try
            {
                result = CreateRobotBehaviorAsync(talk, false).Result;
            }
            catch (Exception e)
            {
                result = new RobotBehavior(null, null)
                {
                    FreeAction = JsonConvert.DeserializeObject<JObject>("{\"Talk\": \"\\\\rspd=110\\\\\\\\vct=135\\\\ 申し訳ありません。ご要望にお応えできません。\"}"),
                    NaturalTalkText = "申し訳ありません。ご要望にお応えできません。"
                };
                Trace.TraceError(e.Message);
                Trace.TraceError(e.StackTrace);
            }

            return result;
        }

        public RobotBehavior CreateRobotBehaviorDirectSpeech(string talk, int rspd=110, int vct=135)
        {
            var behavior = new RobotBehavior(null, null);
            behavior.FreeAction = JsonConvert.DeserializeObject<JObject>("{\"Talk\": \"\\\\rspd=" + rspd.ToString() + "\\\\\\\\vct=" + vct.ToString() + "\\\\ " + talk + "\"}");
            behavior.NaturalTalkText = talk;

            return behavior;
        }

        public async Task<RobotBehavior> CreateRobotBehaviorAsync(string talk, bool isAlwaysUseCache)
        {
            RobotBehavior behavior;
            if (talk != "")
            {
                var result = await Predict(this.luisAppId, this.luisSubscriptionKey, talk);

                return CreateRobotBehavior(result, isAlwaysUseCache);
            }
            else
            {
                behavior = new RobotBehavior(null, null);
            }

            return behavior;
        }

        public ApiResult StartWorkerProcess()
        {
            var m = SynAppsSyncStatusModel.FindBySynAppsDeviceId(this.DeviceId);
            return m.StartProcess();
        }

        public ApiResult FinishWorkerProcess()
        {
            var m = SynAppsSyncStatusModel.FindBySynAppsDeviceId(this.DeviceId);
            return m.FinishProcess();
        }

        private async Task<LuisResult> Predict(string appId, string subscriptionKey, string textToPredict)
        {
            bool _preview = true;
            LuisClient client = new LuisClient(appId, subscriptionKey, _preview);

            return await client.Predict(textToPredict).ConfigureAwait(false);
        }

        private RobotBehavior CreateRobotBehavior(LuisResult _luisResult, bool _isAlwaysUseCache)
        {
            var factory = new SynAppsIntentFactory(this.DeviceId);
            return new RobotBehavior(
                    _luisResult, 
                    factory.CreateSynAppsIntentList(_isAlwaysUseCache), 
                    PredictionIntentModel.FindAllBySynAppsDeviceIdAndIntentGroupByLuisExampleId(this.DeviceId, _luisResult.TopScoringIntent.Name),
                    PredictionEntityModel.FindAllBySynAppsDeviceIdGroupByEntityName(this.DeviceId)
                );
        }
    }
}

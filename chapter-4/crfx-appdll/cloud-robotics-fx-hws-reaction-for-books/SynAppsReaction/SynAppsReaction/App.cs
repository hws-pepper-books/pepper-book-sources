using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CloudRoboticsUtil;
using SynAppsLuis;

namespace SynAppsReaction
{
    public class App : MarshalByRefObject, IAppRouterDll
    {
        public JArrayString ProcessMessage(RbAppMasterCache rbappmc, RbAppRouterCache rbapprc, RbHeader rbh, string rbBodyString)
        {
            var appInfo = JsonConvert.DeserializeObject<JObject>(rbappmc.AppInfo);
            appInfo["DeviceId"] = rbh.SourceDeviceId;
            var appParams = JsonConvert.DeserializeObject<JObject>(rbBodyString);
            JArrayString message = null;
            if (rbh.MessageId == "Speech")
            {
                var speechAppBody = new SpeechAppBody();
                var personId = (appParams["PersonId"] ?? "").ToString();
                var talk = (appParams["talk"] ?? "").ToString();
                var luisService = new LuisService(rbh.SourceDeviceId, appInfo);
                speechAppBody.Behavior = luisService.CreateRobotBehavior(talk);
                speechAppBody.Behavior.NaturalTalkText = TextOverflow(speechAppBody.Behavior.NaturalTalkText);
                if (personId != "")
                {
                    var actionClient = new HwsRobotBehaviorApi.Person.Action.Client(appInfo["SqlConnectionString"].ToString());
                    foreach (var entity in speechAppBody.Behavior.LuisEntities)
                    {
                        actionClient.CreateTalkLog(rbh.SourceDeviceId, personId, speechAppBody.Behavior.NaturalTalkText, talk, entity);
                    }
                }

                message = new JArrayString(MakeProcessMessages(rbh, speechAppBody));
            }
            else if (rbh.MessageId == "RobotSpeech")
            {
                var speechAppBody = new SpeechAppBody();
                var talk = (appParams["talk"] ?? "").ToString();
                var luisService = new LuisService(rbh.SourceDeviceId, appInfo);
                speechAppBody.Behavior = luisService.CreateRobotBehaviorDirectSpeech(talk);
                speechAppBody.Behavior.NaturalTalkText = TextOverflow(speechAppBody.Behavior.NaturalTalkText);
                message = new JArrayString(MakeProcessMessages(rbh, speechAppBody));
            }
            else if (rbh.MessageId == "GetRobotAction")
            {
                var speechAppBody = RobotAction(rbh, appInfo, appParams);
                message = new JArrayString(MakeProcessMessages(rbh, speechAppBody));
            }

            return message;
        }

        private SpeechAppBody RobotAction(RbHeader _rbh, JObject _appInfo, JObject _appParams)
        {
            var speechAppBody = new SpeechAppBody();

            var actionClient = new HwsRobotBehaviorApi.Person.Action.Client(_appInfo["SqlConnectionString"].ToString());
            var actionResult = actionClient.Get(
                _rbh.SourceDeviceId,
                _rbh.SourceDeviceId,
                _appParams["PersonId"].ToString(),
                _appParams["PersonReply"].ToString(),
                _appParams["PersonTalk"].ToString(),
                null,
                "晴れ", // OpenWeatherMap API等を利用して天気情報を取得してください.
                false
                );

            if (actionResult.apiResult.IsSuccessStatusCode)
            {
                var luisService = new LuisService(_rbh.SourceDeviceId, _appInfo);

                speechAppBody.IsNoReaction = actionResult.appBody.IsNoReaction;
                if (_appParams["PersonReply"].ToString() != "" && speechAppBody.IsNoReaction)
                {
                    speechAppBody.Behavior = luisService.CreateRobotBehavior(_appParams["PersonTalk"].ToString());
                    speechAppBody.Behavior.NaturalTalkText = TextOverflow(speechAppBody.Behavior.NaturalTalkText);
                }
                else
                {
                    speechAppBody.Behavior = luisService.CreateRobotBehaviorDirectSpeech(actionResult.appBody.RobotTalk, 108, 165);
                }
            }

            return speechAppBody;
        }

        private JArray MakeProcessMessages(RbHeader rbh, object body)
        {
            var processMessages = new JArray();
            var message = new RbMessage
            {
                RbHeader = rbh,
                RbBody = body
            };
            var json_message = JsonConvert.SerializeObject(message);
            var jo = JsonConvert.DeserializeObject<JObject>(json_message);
            processMessages.Add(jo);

            return processMessages;
        }

        private string TextOverflow(string s, int limit = 60)
        {
            if (s == null || s.Length == 0) return s;

            if (s.Length >= limit)
            {
                s = s.Substring(0, limit - 4) + "...";
            }

            return s;
        }
    }
}

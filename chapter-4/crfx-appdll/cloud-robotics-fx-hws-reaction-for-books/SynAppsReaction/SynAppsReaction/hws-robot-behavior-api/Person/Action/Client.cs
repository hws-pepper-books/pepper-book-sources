using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace HwsRobotBehaviorApi.Person.Action
{
    public class Client
    {
        static private readonly string FirstContact = "first_contact";
        static private readonly string HasKeyphrase = "has_keyphrase";
        static private readonly string Today = "today";
        static private readonly string PastDaysClear = "past_days_clear";
        static private readonly string PastDaysNoneClear = "past_days_none_clear";
        static private readonly string FreeTalk = "free_talk";

        static private readonly string PersonReplyYes = "yes";
        static private readonly string PersonReplyNg = "ng";

        static private readonly string WeatherClear = "晴れ";

        private string SqlConnectionString = "";

        public Client(string _connectionString)
        {
            this.SqlConnectionString = _connectionString;
            RobotBehaviorModel.Connection(_connectionString);
            RobotBehaviorToPersonModel.Connection(_connectionString);
            RobotBehaviorKeyphraseModel.Connection(_connectionString);
            RobotBehaviorTalkLogModel.Connection(_connectionString);
        }

        public AppResult Get(string _synappsDeviceId, string _deviceId, string _personId, string _personReply, string _personTalk, string _personTalkKeyphrase, string _weather, bool _isFreeTalk = false)
        {
            var appResult = new AppResult();
            var apiResult = new ApiResult();
            apiResult.IsSuccessStatusCode = true;

            RobotBehaviorModel behavior = null;
            var model = RobotBehaviorToPersonModel.FindLatestOne(_deviceId, _personId);
            var numberOfMeetToday = RobotBehaviorToPersonModel.NumberOfMeetToday(_deviceId, _personId) + 1;
            if (model != null)
            {
                TimeSpan ts = DateTime.Today - model.CreatedAt.Date;
                int differenceInDays = ts.Days;

                var talklogModel = RobotBehaviorTalkLogModel.FindAllActiveKeyphrase(_deviceId, _personId);
                var talklogs = new List<string>();
                foreach (var talklog in talklogModel)
                {
                    talklogs.Add(talklog.PersonTalkKeyphrase);
                }

                var keyphraseModel = RobotBehaviorKeyphraseModel.Find(_synappsDeviceId, talklogs);
                var keyphraseText = keyphraseModel != null ? keyphraseModel.Keyphrase : "";
                var keyphraseReply = keyphraseModel != null ? keyphraseModel.KeyphraseReply : "";

                if (model.Status == RobotBehaviorToPersonModel.Active && keyphraseText != "")
                {
                    behavior = RobotBehaviorModel.Find(_synappsDeviceId, HasKeyphrase);
                    var actionBody = new ActionBody(behavior.ActionBody);
                    appResult.appBody = getAppBody(actionBody, _personReply, keyphraseText, keyphraseReply, differenceInDays.ToString(), numberOfMeetToday, model.Weather, _personId);

                    foreach (var talklog in talklogModel)
                    {
                        if (keyphraseText == talklog.PersonTalkKeyphrase && _personReply != "")
                        {
                            talklog.Status = RobotBehaviorTalkLogModel.InActive;
                            talklog.Save();
                        }
                    }
                }
                else
                {
                    if (differenceInDays == 0)
                    {
                        behavior = RobotBehaviorModel.Find(_synappsDeviceId, Today);
                        var actionBody = new ActionBody(behavior.ActionBody);
                        appResult.appBody = getAppBody(actionBody, _personReply, keyphraseText, keyphraseReply, differenceInDays.ToString(), numberOfMeetToday, model.Weather, _personId);
                    }
                    else
                    {
                        if (_isFreeTalk)
                        {
                            behavior = RobotBehaviorModel.Find(_synappsDeviceId, FreeTalk);
                            var actionBody = new ActionBody(behavior.ActionBody);
                            appResult.appBody = getAppBody(actionBody, _personTalkKeyphrase, keyphraseText, keyphraseReply, differenceInDays.ToString(), numberOfMeetToday, model.Weather, _personId);
                        }
                        else
                        {
                            if (_weather == WeatherClear)
                            {
                                behavior = RobotBehaviorModel.Find(_synappsDeviceId, PastDaysClear);
                                var actionBody = new ActionBody(behavior.ActionBody);
                                appResult.appBody = getAppBody(actionBody, _personReply, keyphraseText, keyphraseReply, differenceInDays.ToString(), numberOfMeetToday, model.Weather, _personId);
                            }
                            else
                            {
                                behavior = RobotBehaviorModel.Find(_synappsDeviceId, PastDaysNoneClear);
                                var actionBody = new ActionBody(behavior.ActionBody);
                                appResult.appBody = getAppBody(actionBody, _personReply, keyphraseText, keyphraseReply, differenceInDays.ToString(), numberOfMeetToday, model.Weather, _personId);
                            }
                        }
                    }
                }
            }
            else
            {
                behavior = RobotBehaviorModel.Find(_synappsDeviceId, FirstContact);
                var actionBody = new ActionBody(behavior.ActionBody);
                appResult.appBody = getAppBody(actionBody, _personReply, "", "", "", numberOfMeetToday, "", _personId);
            }

            if (appResult.appBody.IsNoReaction == true || _personReply != "")
            {
                SaveHistory(_deviceId, _personId, behavior.SynAppsId, _weather);
            }
            appResult.apiResult = apiResult;

            return appResult;
        }

        public AppResult CreateTalkLog(string _deviceId, string _personId, string _robotTalk, string _personTalk, string _personTalkKeyphrase)
        {
            var appResult = new AppResult();
            var apiResult = new ApiResult();
            apiResult.IsSuccessStatusCode = true;

            var model = RobotBehaviorTalkLogModel.New();
            model.DeviceId = _deviceId;
            model.PersonId = _personId;
            model.RobotTalk = _robotTalk;
            model.PersonTalk = _personTalk;
            model.PersonTalkKeyphrase = _personTalkKeyphrase;
            model.Save();

            appResult.apiResult = apiResult;

            return appResult;
        }

        private AppBody getAppBody(ActionBody _actionBody, string _personReply, string _keyphrase, string _keyphraseReply, string _differenceInDays, int _visitCount, string _lastWeather, string _personId)
        {
            AppBody appBody = null;
            if (_personReply == "") 
            {
                appBody = new AppBody(_actionBody.BehaviorId, _actionBody.RobotTalk);
            }
            else if (_actionBody.IsNoReaction == true)
            {
                appBody = new AppBody(_actionBody.BehaviorId, "");
            }
            else if(_personReply == PersonReplyYes)
            {
                appBody = new AppBody(_actionBody.BehaviorId, _actionBody.PersonReplyYes);
            }
            else if (_personReply == PersonReplyNg)
            {
                appBody = new AppBody(_actionBody.BehaviorId, _actionBody.PersonReplyNg);
            }
            else
            {
                appBody = new AppBody(_actionBody.BehaviorId, _actionBody.PersonReplyOther);
            }
            appBody.RobotTalk = FormatWording(appBody.RobotTalk, _personReply, _keyphrase, _actionBody.PersonReplyOtherKeyword, _keyphraseReply, _differenceInDays, _visitCount, _lastWeather);
            appBody.IsNoReaction = _actionBody.IsNoReaction;

            return appBody;
        }

        private string FormatWording(string word, string _personReply, string _keyphrase, string _personReplyOtherKeyword, string _keyphraseReply, string _days, int _visit_count, string _lastWeather)
        {
            word = Regex.Replace(word, "\\$\\{keyphrase\\}", _keyphrase, RegexOptions.Singleline);
            word = Regex.Replace(word, "\\$\\{keyphrase_reply\\}", _keyphraseReply, RegexOptions.Singleline);
            
            var listStrLineElements = _personReplyOtherKeyword.Split('、');
            foreach (var s in listStrLineElements)
            {
                if (s == _personReply)
                {
                    word = Regex.Replace(word, "\\$\\{keyword\\}", _personReply, RegexOptions.Singleline);
                }
            }

            word = Regex.Replace(word, "\\$\\{days\\}", _days, RegexOptions.Singleline);
            word = Regex.Replace(word, "\\$\\{visit_count\\}", _visit_count.ToString(), RegexOptions.Singleline);
            word = Regex.Replace(word, "\\$\\{last_weather\\}", _lastWeather, RegexOptions.Singleline);

            return word;
        }

        private void SaveHistory(string _deviceId, string _personId, int _robotBehaviorId, string _weather)
        {
            var model = RobotBehaviorToPersonModel.New();
            model.DeviceId = _deviceId;
            model.PersonId = _personId;
            model.RobotBehaviorId = _robotBehaviorId;
            model.Weather = _weather;

            model.Save();
        }
    }
}

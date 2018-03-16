using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HwsRobotBehaviorApi.Person.Action
{
    public class ActionBody
    {
        public int BehaviorId { set; get; }
        public string RobotTalk { set; get; }
        public string PersonReplyYes { set; get; }
        public string PersonReplyNg { set; get; }
        public string PersonReplyOther { set; get; }
        public string PersonReplyOtherKeyword { set; get; }
        public bool IsNoReaction { set; get; }

        public ActionBody(string _actionBody)
        {
            var actionBody               = JsonConvert.DeserializeObject<JObject>(_actionBody);
            this.BehaviorId              = (int)actionBody["Id"];
            this.RobotTalk               = selectTalkText(actionBody["RobotTalk"]);
            this.PersonReplyYes          = selectTalkText(actionBody["PersonReplyYes"]);
            this.PersonReplyNg           = selectTalkText(actionBody["PersonReplyNg"]);
            this.PersonReplyOther        = selectTalkText(actionBody["PersonReplyOther"]);
            this.PersonReplyOtherKeyword = (actionBody["PersonReplyOtherKeyword"] ?? "").ToString();
            this.IsNoReaction            = (this.PersonReplyYes + this.PersonReplyNg + this.PersonReplyOther).Length == 0;
        }

        private string selectTalkText(JToken json)
        {
            var list = new List<string>();
            for (int i = 1; i <= 10; i++)
            {
                var word = (json[$"Talk_{i}"] ?? "").ToString();
                if (word != "")
                {
                    list.Add(word);
                }
            }
            if (list.Count == 0) return "";

            var index = new Random().Next(list.Count);
            return list[index];
        }
    }
}
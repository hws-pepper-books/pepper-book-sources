using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace HwsRobotBehaviorApi.Person.Action
{
    public class AppBody
    {
        public int BehaviorId { set; get; }
        public string RobotTalk { set; get; }
        public bool IsNoReaction { set; get; }

        public AppBody(int _behaviorId, string _robotTalk)
        {
            this.BehaviorId   = _behaviorId;
            this.RobotTalk    = _robotTalk;
            this.IsNoReaction = false;
        }
    }
}
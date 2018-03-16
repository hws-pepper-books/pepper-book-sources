using System;
using System.Collections.Generic;
using Microsoft.Cognitive.LUIS;
using Newtonsoft.Json.Linq;

namespace SynAppsLuis
{
    [Obsolete("Please use RobotBehavior instead.")]
    public class PepperBehavior
    {
        public const string None = "None";
        public string IntentName { set; get; }
        public double Score { set; get; }
        public string ExampleText { set; get; }
        public JObject FreeAction { set; get; }
        public string LinkedEntities { set; get; }
        public List<string> LuisEntities { set; get; }
        public string NaturalTalkText { set; get; }
        public bool IsContinues { set; get; }
        public static readonly JObject ErrorAction = RobotBehavior.ErrorAction;

        public PepperBehavior(LuisResult luisResult, List<SynAppsIntentModel> synappsIntents)
        {
            var robotBehavior = new RobotBehavior(luisResult, synappsIntents);

            this.IntentName      = robotBehavior.IntentName;
            this.Score           = robotBehavior.Score;
            this.ExampleText     = robotBehavior.ExampleText;
            this.FreeAction      = robotBehavior.FreeAction;
            this.LinkedEntities  = robotBehavior.LinkedEntities;
            this.LuisEntities    = robotBehavior.LuisEntities;
            this.NaturalTalkText = robotBehavior.NaturalTalkText;
        }
    }
}

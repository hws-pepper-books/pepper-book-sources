using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HwsRobotBehaviorApi
{
    public class Client
    {
        public Client(string _connectionString)
        {
            RobotBehaviorModel.Connection(_connectionString);
            RobotBehaviorKeyphraseModel.Connection(_connectionString);
        }

        public void RefreshRobotBehaviors(string _synappsDeviceId, JArray tasks)
        {
            var behaviors = new List<RobotBehaviorModel>();
            foreach (var t in tasks)
            {
                var model = RobotBehaviorModel.New();
                model.SynAppsDeviceId = _synappsDeviceId;
                model.SynAppsId = (int)t["Id"];
                model.Status = "Active";
                model.ActionType = (t["ActionType"] ?? "").ToString();
                model.ActionBody = JsonConvert.SerializeObject(t);
                model.IsSynAppsLinked = true;

                behaviors.Add(model);
            }

            RobotBehaviorModel.Refresh(_synappsDeviceId, behaviors);
        }

        public void RefreshRobotBehaviorKeyphrases(string _synappsDeviceId, JArray _keyphrases)
        {
            var keyphrases = new List<RobotBehaviorKeyphraseModel>();
            foreach (var t in _keyphrases)
            {
                var model = RobotBehaviorKeyphraseModel.New();
                model.SynAppsDeviceId = _synappsDeviceId;
                model.SynAppsId = (int)t["Id"];
                model.Keyphrase = (t["Name"] ?? "").ToString();
                model.KeyphraseReply = (t["Reply"] ?? "").ToString();
                model.Status = "Active";
                model.IsSynAppsLinked = true;

                keyphrases.Add(model);
            }

            RobotBehaviorKeyphraseModel.Refresh(_synappsDeviceId, keyphrases);
        }
    }
}

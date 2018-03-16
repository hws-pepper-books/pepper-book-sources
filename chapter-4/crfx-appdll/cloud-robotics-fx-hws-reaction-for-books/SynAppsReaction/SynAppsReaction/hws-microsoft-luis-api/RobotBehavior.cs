using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Cognitive.LUIS;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SynAppsLuis
{
    public class RobotBehavior
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

        public static readonly JObject ErrorAction = JsonConvert.DeserializeObject<JObject>("{\"Talk\": \"\\\\rspd=110\\\\\\\\vct=135\\\\ 申し訳ありません。ご要望にお答えできません。\"}");
        private List<Entity> Memories = new List<Entity>();
        public RobotBehavior(LuisResult luisResult, List<SynAppsIntentModel> synappsIntents, Dictionary<long, List<PredictionIntentModel>> _predictions = null, Dictionary<string, List<PredictionEntityModel>> _predictionEntities = null)
        {
            this.IntentName  = None;
            this.Score       = 0.0;
            this.FreeAction  = ErrorAction;
            this.LinkedEntities = None;
            this.LuisEntities = new List<string>();
            this.IsContinues  = false;
            if (luisResult != null && luisResult.TopScoringIntent != null)
            {
                this.ExampleText  = luisResult.OriginalQuery;
                this.IntentName   = luisResult.TopScoringIntent.Name;
                this.Score        = luisResult.TopScoringIntent.Score;
                var entities      = luisResult.GetAllEntities();
                var entityValues  = entities.Select(entity => ToNarrow(Regex.Replace(Regex.Replace(entity.Value, " ", "", RegexOptions.Singleline), "　", "", RegexOptions.Singleline)));
                this.LuisEntities = entityValues.ToList();
                var targetsWithEntities = synappsIntents.Where(intent =>
                        intent.Name == this.IntentName &&
                        entityValues.Any(Regex.Replace(ToNarrow(intent.Entity.ToLower()), " ", "", RegexOptions.Singleline).Split(',').Contains)
                    ).ToList();
                targetsWithEntities = targetsWithEntities.OrderByDescending(intent =>
                        entityValues.Count(intent.Entity.Split(',').Contains)
                    ).ThenBy(intent =>
                        intent.Entity.Split(',').Count()
                    ).ToList();

                var resultWithEntities = getLinkedAction(targetsWithEntities, entities);
                if (resultWithEntities.Status == Status.Success)
                {
                    this.FreeAction = resultWithEntities.FreeAction;
                    this.LinkedEntities = resultWithEntities.LinkedEntities;
                }
                else
                {
                    var targets = synappsIntents.Where(intent =>
                            intent.Name == this.IntentName &&
                            intent.Entity == ""
                        ).ToList();
                    var result = getLinkedAction(targets, entities);
                    if (result.Status == Status.Success)
                    {
                        this.FreeAction     = result.FreeAction;
                        this.LinkedEntities = result.LinkedEntities;
                    }
                    else
                    {
                        var preview = new PreviewLabeledExampleDto();
                        preview.IntentLabel = this.IntentName;
                        var entityLabels = new List<PredictionEntityDto>();
                        foreach (var e in entities)
                        {
                            var entityLabel = new PredictionEntityDto();
                            entityLabel.EntityName = e.Name;
                            entityLabel.EntityValue = e.Value;

                            entityLabels.Add(entityLabel);
                        }
                        preview.EntityLabels = entityLabels;

                        this.FreeAction     = Predictions(preview, _predictions, _predictionEntities, targetsWithEntities) ?? this.FreeAction;
                        this.LinkedEntities = result.LinkedEntities;
                    }
                }
                
                string talk = "";
                this.IsContinues = (bool)(this.FreeAction["IsContinuance"] ?? false);
                if (this.IsContinues)
                {
                    talk = (this.FreeAction["Talk1"] ?? "").ToString();
                }
                else
                {
                    talk = (this.FreeAction["Talk"] ?? "").ToString();
                }
                if (talk != "")
                {
                    talk = Regex.Replace(talk, "\\\\rspd=.+?\\\\", "", RegexOptions.Singleline);
                    talk = Regex.Replace(talk, "\\\\vct=.+?\\\\", "", RegexOptions.Singleline);
                    this.NaturalTalkText = talk.Trim();
                }
            }
        }

        private ResultBody getLinkedAction(List<SynAppsIntentModel> targets, List<Entity> entities)
        {
            foreach (var intent in targets)
            {
                var action = intent.ReactionBody;
                if (action == null || action == "")
                {
                    continue;
                }
                var includedEntities = new List<string>();
                foreach (Match m in Regex.Matches(action, "\\$\\{(?<entity>.+?)\\}"))
                {
                    includedEntities.Add(m.Groups["entity"].Value);
                }

                if (includedEntities.Count == 0 || entities.Any(item => includedEntities.Contains(Regex.Replace(item.Name, "::.+$", "", RegexOptions.Singleline))))
                {
                    string linkedEntities = "";
                    foreach (Entity entity in entities)
                    {
                        action = Regex.Replace(action, "\\$\\{" + Regex.Replace(entity.Name, "::.+$", "", RegexOptions.Singleline) + "\\}", entity.Value, RegexOptions.Singleline);
                        linkedEntities += ToNarrow(entity.Value.ToLower()).Replace(" ", "") + ",";
                    }
                    linkedEntities = linkedEntities.TrimEnd(',');

                    if (intent.Entity.Length == 0 || intent.Entity.Length == linkedEntities.Length)
                    {
                        return new ResultBody(Status.Success, JsonConvert.DeserializeObject<JObject>(action), linkedEntities);
                    }
                }
            }

            return new ResultBody(Status.Fail, ErrorAction, None);
        }

        private string ToNarrow(string s)
        {
            s = Regex.Replace(s, "[０-９]", p => ((char)(p.Value[0] - '０' + '0')).ToString());
            s = Regex.Replace(s, "[ａ-ｚ]", p => ((char)(p.Value[0] - 'ａ' + 'a')).ToString());
            s = Regex.Replace(s, "[Ａ-Ｚ]", p => ((char)(p.Value[0] - 'Ａ' + 'A')).ToString());
            s = Regex.Replace(s, "　", " ");

            return s;
        }

        private JObject Predictions(PreviewLabeledExampleDto _dto, Dictionary<long, List<PredictionIntentModel>> _predictions, Dictionary<string, List<PredictionEntityModel>> _predictionEntities, List<SynAppsIntentModel> _targets)
        {
            if (_predictions == null || _predictions.Count == 0 || _targets == null || _targets.Count == 0) return null;

            var isEnough = true;
            var candidates = new List<long>();
            foreach (var e in _dto.EntityLabels)
            {
                var pairs = _predictions.Where(pair => pair.Value.Any(entity => entity.EntityValue == e.EntityValue));
                if (pairs.Count() > 0)
                {
                    foreach (var p in pairs)
                    {
                        candidates.Add(p.Key);
                    }
                }
                else
                {
                    isEnough = false;
                    break;
                }
            }
            if (!isEnough || candidates.Count == 0) return null;

            var g = _predictions.Where(pair => candidates.Contains(pair.Key));
            var gg = g.Where(pair => pair.Value.Count == g.Max(p => p.Value.Count));

            var i = 1;
            var freeAction = new JObject();
            freeAction["IsContinuance"] = true;
            var questions = new List<List<string>>();
            foreach (var v in gg.First().Value)
            {
                var q = _predictionEntities[v.EntityName].Select(p => p.EntityValue).Distinct();
                freeAction[$"Talk{i}"] = $"{v.EntityName} は何ですか？";
                freeAction[$"Question{i}"] = JArray.FromObject(q);
                questions.Add(q.ToList<string>());
                i++;
            }

            freeAction["Answers"] = new JObject();
            foreach (var t in _targets)
            {
                var selected = new List<string>();
                var answerKey = "";
                foreach (var q in questions)
                {
                    var entities = t.Entity.Split(',').Where(e => !selected.Contains(e));
                    foreach (var e in entities)
                    {
                        var ee = Regex.Replace(ToNarrow(e.ToLower()), " ", "", RegexOptions.Singleline);
                        if (q.Contains(ee))
                        {
                            answerKey += ee;
                            selected.Add(e);
                        }
                    }
                }
                var answer = JsonConvert.DeserializeObject<JObject>(t.ReactionBody);
                freeAction["Answers"][answerKey] = answer["Talk"];
            }

            return freeAction;
        }

        class ResultBody
        {
            public Status Status { set; get; }
            public JObject FreeAction { set; get; }
            public string LinkedEntities { set; get; }

            public ResultBody(Status status, JObject action, string linkedEntities)
            {
                this.Status = status;
                this.FreeAction = action;
                this.LinkedEntities = linkedEntities;
            }
        }

        enum Status
        {
            Success,
            Fail
        }
    }
}

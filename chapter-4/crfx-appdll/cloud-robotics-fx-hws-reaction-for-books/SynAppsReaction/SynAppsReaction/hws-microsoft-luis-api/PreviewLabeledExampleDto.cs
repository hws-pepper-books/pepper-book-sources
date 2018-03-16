using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SynAppsLuis
{
    public class PreviewLabeledExampleDto
    {
        public long Id { set; get; }
        public string IntentLabel { set; get;}
        public List<PredictionEntityDto> EntityLabels { set; get; }

        public PreviewLabeledExampleDto() { }

        public PreviewLabeledExampleDto(JToken _json)
        {
            this.Id = (long)_json["id"];
            this.IntentLabel = (_json["intentLabel"] ?? "").ToString();

            var entitiLabels = new List<PredictionEntityDto>();
            foreach (var entity in _json["entityPredictions"])
            {
                var dto = new PredictionEntityDto(entity);
                entitiLabels.Add(dto);
            }
            this.EntityLabels = entitiLabels;
        }
    }
}

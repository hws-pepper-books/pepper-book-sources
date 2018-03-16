using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SynAppsLuis
{
    public class PredictionEntityDto
    {
        public string EntityName { set; get; }
        public string EntityValue { set; get;}

        public PredictionEntityDto() { }

        public PredictionEntityDto(JToken _json)
        {
            this.EntityName  = (_json["entityName"] ?? "").ToString();
            this.EntityValue = (_json["phrase"] ?? "").ToString().Replace(" ", "");
        }
    }
}

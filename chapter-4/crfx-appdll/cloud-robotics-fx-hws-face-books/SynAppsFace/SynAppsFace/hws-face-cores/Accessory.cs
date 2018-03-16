using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class Accessory
    {
        public string Type { set; get; }
        public double Confidence { set; get; }

        public Accessory(JToken json)
        {
            this.Type = (json["type"] ?? "").ToString();
            this.Confidence = (double)json["confidence"];
        }
    }
}

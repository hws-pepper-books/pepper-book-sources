using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class HairColor
    {
        public string Color { set; get; }
        public double Confidence { set; get; }

        public HairColor(JToken json)
        {
            this.Color = (json["color"] ?? "").ToString();
            this.Confidence = (double)json["confidence"];
        }
    }
}

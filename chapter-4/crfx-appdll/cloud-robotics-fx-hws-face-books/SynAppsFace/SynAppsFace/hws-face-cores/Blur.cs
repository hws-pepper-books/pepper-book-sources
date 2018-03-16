using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class Blur
    {
        public string BlurLevel { set; get; }
        public double Value { set; get; }

        public Blur(JToken json)
        {
            this.BlurLevel = (json["blurLevel"] ?? "").ToString();
            this.Value = (double)json["value"];
        }
    }
}

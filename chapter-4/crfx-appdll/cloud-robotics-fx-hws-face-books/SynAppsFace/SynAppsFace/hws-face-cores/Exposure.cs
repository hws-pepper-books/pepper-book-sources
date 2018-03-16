using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class Exposure
    {
        public string ExposureLevel { set; get; }
        public double Value { set; get; }

        public Exposure(JToken json)
        {
            this.ExposureLevel = (json["exposureLevel"] ?? "").ToString();
            this.Value = (double)json["value"];
        }
    }
}

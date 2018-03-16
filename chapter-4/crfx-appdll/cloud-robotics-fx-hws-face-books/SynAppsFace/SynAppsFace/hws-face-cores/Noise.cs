using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class Noise
    {
        public string NoiseLevel { set; get; }
        public double Value { set; get; }

        public Noise(JToken json)
        {
            this.NoiseLevel = (json["noiseLevel"] ?? "").ToString();
            this.Value = (double)json["value"];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class Occlusion
    {
        public bool ForeheadOccluded { set; get; }
        public bool EyeOccluded { set; get; }
        public bool MouthOccluded { set; get; }

        public Occlusion(JToken json)
        {
            this.ForeheadOccluded = (bool)json["foreheadOccluded"];
            this.EyeOccluded = (bool)json["eyeOccluded"];
            this.MouthOccluded = (bool)json["mouthOccluded"];
        }
    }
}

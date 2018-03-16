using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class FacialHair
    {
        public float Moustache { set; get; }
        public float Beard { set; get; }
        public float Sideburns { set; get; }

        public FacialHair(JToken json)
        {
            this.Moustache = (float)json["moustache"];
            this.Beard = (float)json["beard"];
            this.Sideburns = (float)json["sideburns"];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class HeadPose
    {
        public float Roll { set; get; }
        public float Yaw { set; get; }
        public float Pitch { set; get; }

        public HeadPose(JToken json)
        {
            this.Roll = (float)json["roll"];
            this.Yaw = (float)json["yaw"];
            this.Pitch = (float)json["pitch"];
        }
    }
}

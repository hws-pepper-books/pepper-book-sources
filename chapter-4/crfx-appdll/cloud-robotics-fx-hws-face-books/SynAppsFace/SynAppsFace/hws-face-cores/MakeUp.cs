using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class MakeUp
    {
        public bool EyeMakeup { set; get; }
        public bool LipMakeup { set; get; }

        public MakeUp(JToken json)
        {
            this.EyeMakeup = (bool)json["eyeMakeup"];
            this.LipMakeup = (bool)json["lipMakeup"];
        }
    }
}

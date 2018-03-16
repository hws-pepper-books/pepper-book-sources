using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class Hair
    {
        public double Bald { set; get; }
        public bool Invisible { set; get; }
        public List<HairColor> HairColor { set; get; }

        public Hair(JToken json)
        {
            this.Bald = (double)json["bald"];
            this.Invisible = (bool)json["invisible"];

            var colors = new List<HairColor>();
            foreach (var color in json["hairColor"])
            {
                colors.Add(new HairColor(color));
            }
            this.HairColor = colors;
        }
    }
}

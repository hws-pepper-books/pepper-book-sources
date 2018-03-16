using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class FaceRectangle
    {
        public float Width { set; get; }
        public float Height { set; get; }
        public float Left { set; get; }
        public float Top { set; get; }

        public FaceRectangle() { }

        public FaceRectangle(JToken json)
        {
            this.Width = (float)json["width"];
            this.Height = (float)json["height"];
            this.Left = (float)json["left"];
            this.Top = (float)json["top"];
        }
    }
}

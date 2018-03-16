using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class FaceAttributes
    {
        public float Age { set; get; }
        public string Gender { set; get; }
        public float Smile { set; get; }
        public FacialHair FacialHair { set; get; }
        public string Glasses { set; get; }
        public HeadPose HeadPose { set; get; }
        public Emotion Emotion { set; get; }
        public Hair Hair { set; get; }
        public MakeUp MakeUp { set; get; }
        public Occlusion Occlusion { set; get; }
        public List<Accessory> Accessories { set; get; }
        public Blur Blur { set; get; }
        public Exposure Exposure { set; get; }
        public Noise Noise { set; get; }

        public FaceAttributes() { }

        public FaceAttributes(JToken json)
        {
            this.Age = (float)json["age"];
            this.Gender = (json["gender"] ?? "").ToString();
            this.Smile = (float)json["smile"];
            this.FacialHair = new FacialHair(json["facialHair"]);
            this.Glasses = (string)json["glasses"];
            this.HeadPose = new HeadPose(json["headPose"]);
            this.Emotion = new Emotion(json["emotion"]);
            this.Hair = new Hair(json["hair"]);
            this.MakeUp = new MakeUp(json["makeup"]);
            this.Occlusion = new Occlusion(json["occlusion"]);
            var accessories = new List<Accessory>();
            foreach (var a in json["accessories"])
            {
                accessories.Add(new Accessory(a));
            }
            this.Accessories = accessories;
            this.Blur = new Blur(json["blur"]);
            this.Exposure = new Exposure(json["exposure"]);
            this.Noise = new Noise(json["noise"]);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class PersonInfo
    {
        public string FaceId { set; get; }
        public FaceRectangle FaceRectangle { set; get; }
        public FaceLandmarks FaceLandmarks { set; get; }
        public FaceAttributes FaceAttributes { set; get; }
        public string PersonId { set; get; }
        public string PersonName { set; get; }
        public string PersonNameYomi { set; get; }
        public string FaceConfidence { set; get; }
        public long LastDetectedAt { set; get; }
        public long RegisteredAt { set; get; }

        public PersonInfo() {}

        public PersonInfo(JToken json)
        {
            this.FaceId = json["faceId"].ToString();
            this.FaceRectangle = new FaceRectangle(json["faceRectangle"]);
            if (json["faceLandmarks"] != null)
            {
                this.FaceLandmarks = new FaceLandmarks(json["faceLandmarks"]);
            }
            this.FaceAttributes = new FaceAttributes(json["faceAttributes"]);
        }
    }
}

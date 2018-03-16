using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HwsFaceCores;

namespace SynAppsFace
{
    public class AppBody
    {
        public int Code { set; get; }
        public string PersonGroupId { set; get; }
        public string PersonId { set; get; }
        public string PersonName { set; get; }
        public string PersonNameYomi { set; get; }
        public string PersonFaceId { set; get; }
        public List<HwsFaceCores.PersonInfo> PersonInfos { set; get; }
        public List<Emotion> EmotionBodies { set; get; }
        public AppBody()
        {
            this.Code = 200;
            this.PersonGroupId = string.Empty;
            this.PersonId = string.Empty;
            this.PersonName = string.Empty;
            this.PersonNameYomi = string.Empty;
            this.PersonFaceId = string.Empty;
            this.PersonInfos = new List<HwsFaceCores.PersonInfo>();
        }
    }
}
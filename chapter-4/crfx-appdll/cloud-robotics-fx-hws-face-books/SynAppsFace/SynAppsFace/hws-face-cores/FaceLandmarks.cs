using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class FaceLandmarks
    {
        public float PupilLeftX { set; get; }
        public float PupilLeftY { set; get; }
        public float PupilRightX { set; get; }
        public float PupilRightY { set; get; }
        public float NoseTipX { set; get; }
        public float NoseTipY { set; get; }
        public float MouthLeftX { set; get; }
        public float MouthLeftY { set; get; }
        public float MouthRightX { set; get; }
        public float MouthRightY { set; get; }
        public float EyebrowLeftOuterX { set; get; }
        public float EyebrowLeftOuterY { set; get; }
        public float EyebrowLeftInnerX { set; get; }
        public float EyebrowLeftInnerY { set; get; }
        public float EyeLeftOuterX { set; get; }
        public float EyeLeftOuterY { set; get; }
        public float EyeLeftTopX { set; get; }
        public float EyeLeftTopY { set; get; }
        public float EyeLeftBottomX { set; get; }
        public float EyeLeftBottomY { set; get; }
        public float EyeLeftInnerX { set; get; }
        public float EyeLeftInnerY { set; get; }
        public float EyebrowRightInnerX { set; get; }
        public float EyebrowRightInnerY { set; get; }
        public float EyebrowRightOuterX { set; get; }
        public float EyebrowRightOuterY { set; get; }
        public float EyeRightInnerX { set; get; }
        public float EyeRightInnerY { set; get; }
        public float EyeRightTopX { set; get; }
        public float EyeRightTopY { set; get; }
        public float EyeRightBottomX { set; get; }
        public float EyeRightBottomY { set; get; }
        public float EyeRightOuterX { set; get; }
        public float EyeRightOuterY { set; get; }
        public float NoseRootLeftX { set; get; }
        public float NoseRootLeftY { set; get; }
        public float NoseRootRightX { set; get; }
        public float NoseRootRightY { set; get; }
        public float NoseLeftAlarTopX { set; get; }
        public float NoseLeftAlarTopY { set; get; }
        public float NoseRightAlarTopX { set; get; }
        public float NoseRightAlarTopY { set; get; }
        public float NoseLeftAlarOutTipX { set; get; }
        public float NoseLeftAlarOutTipY { set; get; }
        public float NoseRightAlarOutTipX { set; get; }
        public float NoseRightAlarOutTipY { set; get; }
        public float UpperLipTopX { set; get; }
        public float UpperLipTopY { set; get; }
        public float UpperLipBottomX { set; get; }
        public float UpperLipBottomY { set; get; }
        public float UnderLipTopX { set; get; }
        public float UnderLipTopY { set; get; }
        public float UnderLipBottomX { set; get; }
        public float UnderLipBottomY { set; get; }

        public FaceLandmarks() { }

        public FaceLandmarks(JToken json)
        {
            this.PupilLeftX = (float)json["pupilLeft"]["x"];
            this.PupilLeftY = (float)json["pupilLeft"]["y"];
            this.PupilRightX = (float)json["pupilRight"]["x"];
            this.PupilRightY = (float)json["pupilRight"]["y"];
            this.NoseTipX = (float)json["noseTip"]["x"];
            this.NoseTipY = (float)json["noseTip"]["y"];
            this.MouthLeftX = (float)json["mouthLeft"]["x"];
            this.MouthLeftY = (float)json["mouthLeft"]["y"];
            this.MouthRightX = (float)json["mouthRight"]["x"];
            this.MouthRightY = (float)json["mouthRight"]["y"];
            this.EyebrowLeftOuterX = (float)json["eyebrowLeftOuter"]["x"];
            this.EyebrowLeftOuterY = (float)json["eyebrowLeftOuter"]["y"];
            this.EyebrowLeftInnerX = (float)json["eyebrowLeftInner"]["x"];
            this.EyebrowLeftInnerY = (float)json["eyebrowLeftInner"]["y"];
            this.EyeLeftOuterX = (float)json["eyeLeftOuter"]["x"];
            this.EyeLeftOuterY = (float)json["eyeLeftOuter"]["y"];
            this.EyeLeftTopX = (float)json["eyeLeftTop"]["x"];
            this.EyeLeftTopY = (float)json["eyeLeftTop"]["y"];
            this.EyeLeftBottomX = (float)json["eyeLeftBottom"]["x"];
            this.EyeLeftBottomY = (float)json["eyeLeftBottom"]["y"];
            this.EyeLeftInnerX = (float)json["eyeLeftInner"]["x"];
            this.EyeLeftInnerY = (float)json["eyeLeftInner"]["y"];
            this.EyebrowRightInnerX = (float)json["eyebrowRightInner"]["x"];
            this.EyebrowRightInnerY = (float)json["eyebrowRightInner"]["y"];
            this.EyebrowRightOuterX = (float)json["eyebrowRightOuter"]["x"];
            this.EyebrowRightOuterY = (float)json["eyebrowRightOuter"]["y"];
            this.EyeRightInnerX = (float)json["eyeRightInner"]["x"];
            this.EyeRightInnerY = (float)json["eyeRightInner"]["y"];
            this.EyeRightTopX = (float)json["eyeRightTop"]["x"];
            this.EyeRightTopY = (float)json["eyeRightTop"]["y"];
            this.EyeRightBottomX = (float)json["eyeRightBottom"]["x"];
            this.EyeRightBottomY = (float)json["eyeRightBottom"]["y"];
            this.EyeRightOuterX = (float)json["eyeRightOuter"]["x"];
            this.EyeRightOuterY = (float)json["eyeRightOuter"]["y"];
            this.NoseRootLeftX = (float)json["noseRootLeft"]["x"];
            this.NoseRootLeftY = (float)json["noseRootLeft"]["y"];
            this.NoseRootRightX = (float)json["noseRootRight"]["x"];
            this.NoseRootRightY = (float)json["noseRootRight"]["y"];
            this.NoseLeftAlarTopX = (float)json["noseLeftAlarTop"]["x"];
            this.NoseLeftAlarTopY = (float)json["noseLeftAlarTop"]["y"];
            this.NoseRightAlarTopX = (float)json["noseRightAlarTop"]["x"];
            this.NoseRightAlarTopY = (float)json["noseRightAlarTop"]["y"];
            this.NoseLeftAlarOutTipX = (float)json["noseLeftAlarOutTip"]["x"];
            this.NoseLeftAlarOutTipY = (float)json["noseLeftAlarOutTip"]["y"];
            this.NoseRightAlarOutTipX = (float)json["noseRightAlarOutTip"]["x"];
            this.NoseRightAlarOutTipY = (float)json["noseRightAlarOutTip"]["y"];
            this.UpperLipTopX = (float)json["upperLipTop"]["x"];
            this.UpperLipTopY = (float)json["upperLipTop"]["y"];
            this.UpperLipBottomX = (float)json["upperLipBottom"]["x"];
            this.UpperLipBottomY = (float)json["upperLipBottom"]["y"];
            this.UnderLipTopX = (float)json["underLipTop"]["x"];
            this.UnderLipTopY = (float)json["underLipTop"]["y"];
            this.UnderLipBottomX = (float)json["underLipBottom"]["x"];
            this.UnderLipBottomY = (float)json["underLipBottom"]["y"];
        }
    }
}

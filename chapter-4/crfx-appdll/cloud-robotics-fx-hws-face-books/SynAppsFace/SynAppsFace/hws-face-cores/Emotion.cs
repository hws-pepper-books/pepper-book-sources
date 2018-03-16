using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HwsFaceCores
{
    public class Emotion
    {
        public double Anger { set; get; }
        public double Contempt { set; get; }
        public double Disgust { set; get; }
        public double Fear { set; get; }
        public double Happiness { set; get; }
        public double Neutral { set; get; }
        public double Sadness { set; get; }
        public double Surprise { set; get; }
        public string PrimeCandidate { set; get; }

        public Emotion(JToken json)
        {
            this.Anger = (double)json["anger"];
            this.Contempt = (double)json["contempt"];
            this.Disgust = (double)json["disgust"];
            this.Fear = (double)json["fear"];
            this.Happiness = (double)json["happiness"];
            this.Neutral = (double)json["neutral"];
            this.Sadness = (double)json["sadness"];
            this.Surprise = (double)json["surprise"];
            this.PrimeCandidate = getPrimeCandidate();
        }

        private string getPrimeCandidate()
        {
            var dic = new Dictionary<string, double>();
            dic.Add("Anger", this.Anger);
            dic.Add("Contempt", this.Contempt);
            dic.Add("Disgust", this.Disgust);
            dic.Add("Fear", this.Fear);
            dic.Add("Neutral", this.Neutral);
            dic.Add("Sadness", this.Sadness);
            dic.Add("Surprise", this.Surprise);
            dic.Add("Happiness", this.Happiness);

            return dic.OrderByDescending((x) => x.Value).First().Key;
        }
    }
}

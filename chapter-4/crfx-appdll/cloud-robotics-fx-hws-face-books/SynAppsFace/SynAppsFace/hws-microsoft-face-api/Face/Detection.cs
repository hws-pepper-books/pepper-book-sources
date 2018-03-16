using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using HwsFaceCores;

namespace HwsMicrosoftFaceApi.Face
{
    public class Detection
    {
        private HttpClient httpClient;
        private double facialHairConfidence;

        public Detection(HttpClient _httpClient, string faceApiKey, double facialHairConfidence)
        {
            this.httpClient = _httpClient;
            if (!this.httpClient.DefaultRequestHeaders.Contains("Ocp-Apim-Subscription-Key"))
            {
                this.httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", faceApiKey);
            }
            
            this.facialHairConfidence = facialHairConfidence;
        }
        public AppResult DetectFace(byte[] buffer)
        {
            var appResult = new AppResult();
            var apiResult = new ApiResult();
            var personInfos = new List<PersonInfo>();

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request parameters
            queryString["returnFaceId"] = "true";
            queryString["returnFaceLandmarks"] = "true";
            queryString["returnFaceAttributes"] = "age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
            var uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;

            // Call REST API
            var content = new ByteArrayContent(buffer);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var response = this.httpClient.PostAsync(uri, content);
            response.Wait();

            apiResult.IsSuccessStatusCode = response.Result.IsSuccessStatusCode;
            if (apiResult.IsSuccessStatusCode)
            {
                var resdata = response.Result.Content.ReadAsStringAsync();
                resdata.Wait();
                apiResult.Result = resdata.Result.ToString();

                var results = (JArray)JsonConvert.DeserializeObject(apiResult.Result);
                foreach(JToken json in results)
                {
                    personInfos.Add(new PersonInfo(json));
                }

                if (personInfos.Count == 0)
                {
                    apiResult.IsSuccessStatusCode = false;
                }
            }
            else
            {
                apiResult.Message = response.Result.ToString();
            }

            appResult.apiResult = apiResult;
            appResult.PersonInfos = personInfos;

            return appResult;
        }
    }
}

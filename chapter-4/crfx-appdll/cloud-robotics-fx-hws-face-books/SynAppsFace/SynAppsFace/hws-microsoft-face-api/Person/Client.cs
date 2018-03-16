using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using HwsFaceCores;

namespace HwsMicrosoftFaceApi.Person
{
    public class Client
    {
        private HttpClient client;
        private string endpoint = "https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/";

        public Client(string faceApiKey)
        {
            this.client = new HttpClient();
            if (!this.client.DefaultRequestHeaders.Contains("Ocp-Apim-Subscription-Key"))
            {
                this.client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", faceApiKey);
            }
        }

        public AppResult Create(string personGroupId, string personName)
        {
            if (personGroupId == "" || personName == "")
            {
                var appResult = new AppResult();
                var apiResult = new ApiResult();
                apiResult.IsSuccessStatusCode = false;
                appResult.apiResult = apiResult;
                return appResult;
            }

            var uri = this.endpoint + personGroupId + "/persons";

            var reqestBody = new JObject();
            reqestBody["name"] = personName;
            reqestBody["userData"] = string.Empty;
            byte[] byteData = Encoding.UTF8.GetBytes((string)JsonConvert.SerializeObject(reqestBody));
            var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = this.client.PostAsync(uri, content);
            response.Wait();

            return GetAppResult(response.Result);
        }

        public AppResult Add(byte[] buffer, string personGroupId, string personId)
        {
            if (personGroupId == "" || personId == "")
            {
                var appResult = new AppResult();
                var apiResult = new ApiResult();
                apiResult.IsSuccessStatusCode = false;
                appResult.apiResult = apiResult;
                return appResult;
            }

            var uri = this.endpoint + personGroupId + "/persons/" + personId + "/persistedFaces";

            var content = new ByteArrayContent(buffer);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var response = this.client.PostAsync(uri, content);
            response.Wait();

            return GetAppResult(response.Result);
        }

        public AppResult Delete(string personGroupId, string personId)
        {
            if (personGroupId == "" || personId == "")
            {
                var appResult = new AppResult();
                var apiResult = new ApiResult();
                apiResult.IsSuccessStatusCode = false;
                appResult.apiResult = apiResult;
                return appResult;
            }

            var uri = this.endpoint + personGroupId + "/persons/" + personId;

            var response = this.client.DeleteAsync(uri);
            response.Wait();

            return GetAppResult(response.Result);
        }

        private AppResult GetAppResult(HttpResponseMessage result)
        {
            var appResult = new AppResult();
            var apiResult = new ApiResult();

            apiResult.IsSuccessStatusCode = result.IsSuccessStatusCode;
            if (apiResult.IsSuccessStatusCode)
            {
                var resdata = result.Content.ReadAsStringAsync();
                resdata.Wait();
                apiResult.Result = resdata.Result.ToString();
            }
            else
            {
                apiResult.Message = result.ToString();
            }
            appResult.apiResult = apiResult;

            return appResult;
        }
    }
}

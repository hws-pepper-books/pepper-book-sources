using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using HwsFaceCores;

namespace HwsMicrosoftFaceApi.PersonGroup
{
    public class Client
    {
        private HttpClient client;
        private string endpoint;

        public Client(string faceApiKey)
        {
            this.client = new HttpClient();
            if (!this.client.DefaultRequestHeaders.Contains("Ocp-Apim-Subscription-Key"))
            {
                this.client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", faceApiKey);
            }
            this.endpoint = "https://westus.api.cognitive.microsoft.com/face/v1.0/persongroups/";
        }

        public AppResult Create(string id)
        {
            var uri = this.endpoint + id;

            var reqestBody = new JObject();
            reqestBody["name"] = "group_" + id;
            reqestBody["userData"] = string.Empty;
            byte[] byteData = Encoding.UTF8.GetBytes((string)JsonConvert.SerializeObject(reqestBody));
            var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = client.PutAsync(uri, content);
            response.Wait();

            return GetAppResult(response.Result);
        }

        public AppResult Train(string id)
        {
            var uri = this.endpoint + id + "/train";

            var reqestBody = new JObject();
            reqestBody["body"] = "{body}";
            byte[] byteData = Encoding.UTF8.GetBytes((string)JsonConvert.SerializeObject(reqestBody));
            var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = client.PostAsync(uri, content);
            response.Wait();

            return GetAppResult(response.Result);
        }

        public AppResult Training(string id)
        {
            var uri = this.endpoint + id + "/training";

            var response = client.GetAsync(uri);
            response.Wait();

            return GetAppResult(response.Result);
        }

        public AppResult Delete(string id)
        {
            var uri = this.endpoint + id;

            var response = client.DeleteAsync(uri);
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

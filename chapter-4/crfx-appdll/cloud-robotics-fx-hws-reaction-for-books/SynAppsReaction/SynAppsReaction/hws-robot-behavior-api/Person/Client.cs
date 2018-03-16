using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace HwsRobotBehaviorApi.Person
{
    public class Client
    {
        public Client(string _connectionString)
        {
            PersonInfoModel.Connection(_connectionString);
        }

        public AppResult Get(string _personId)
        {
            var appResult = new AppResult();
            var apiResult = new ApiResult();

            var model = PersonInfoModel.Find(_personId);
            if (model != null)
            {
                var list = new List<PersonBody>
                {
                    new PersonBody(model)
                };
                appResult.appBody = new AppBody(list);
            }
            else
            {
                apiResult.IsSuccessStatusCode = false;
                apiResult.Message = "Not Found.";
            }
            appResult.apiResult = apiResult;

            return appResult;
        }
    }
}

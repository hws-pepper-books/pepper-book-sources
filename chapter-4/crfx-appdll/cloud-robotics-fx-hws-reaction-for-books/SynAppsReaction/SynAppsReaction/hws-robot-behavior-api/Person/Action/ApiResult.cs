using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwsRobotBehaviorApi.Person.Action
{
    public class ApiResult
    {
        public bool IsSuccessStatusCode { set; get; }
        public string Message { set; get; }
        public string Result { set; get; }

        public ApiResult()
        {
            this.IsSuccessStatusCode = false;
            this.Message = string.Empty;
            this.Result = string.Empty;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwsFaceCores
{
    public class AppResult
    {
        public ApiResult apiResult { set; get; }
        public List<PersonInfo> PersonInfos { set; get; }

        public AppResult()
        {
            this.PersonInfos = new List<PersonInfo>();
        }
    }
}

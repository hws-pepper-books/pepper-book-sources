using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace HwsRobotBehaviorApi.Person
{
    public class PersonBody
    {
        public string PersonGroupId { set; get; }
        public string PersonId { set; get; }
        public string PersonName { set; get; }
        public string PersonNameYomi { set; get; }

        public PersonBody(PersonInfoModel model)
        {
            this.PersonGroupId  = model.PersonGroupId ?? "";
            this.PersonId       = model.PersonId ?? "";
            this.PersonName     = model.PersonName ?? "";
            this.PersonNameYomi = model.PersonNameYomi ?? "";
        }
    }
}
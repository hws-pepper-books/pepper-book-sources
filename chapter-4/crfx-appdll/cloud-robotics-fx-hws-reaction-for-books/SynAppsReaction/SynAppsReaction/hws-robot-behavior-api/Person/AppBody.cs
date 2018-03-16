using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace HwsRobotBehaviorApi.Person
{
    public class AppBody
    {
        public List<PersonBody> Persons { set; get; }

        public AppBody(List<PersonBody> _persons)
        {
            this.Persons = _persons;
        }
    }
}
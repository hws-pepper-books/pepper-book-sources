using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwsRobotBehaviorApi.Person
{
    public class PersonInfoModel
    {
        private static string SqlConnectionString;

        public long Id { set; get; }
        public string PersonGroupId { set; get; }
        public string PersonId { set; get; }
        public string PersonName { set; get; }
        public string PersonNameYomi { set; get; }
        public bool IsDeleted { set; get; }

        PersonInfoModel()
        {
            this.Id = 0;
            this.IsDeleted = false;
        }

        public static void Connection(string _connectionString)
        {
            SqlConnectionString = _connectionString;
        }

        public static PersonInfoModel New()
        {
            return new PersonInfoModel();
        }

        public static PersonInfoModel Find(string _personId)
        {
            var dc = new PersonInfosDataContext(SqlConnectionString);
            var records =
                (from n in dc.PersonInfos
                where n.PersonId == _personId
                where n.IsDeleted == false
                select n).Take(1);

            return Build(records);
        }

        private static PersonInfoModel Build(IQueryable<PersonInfo> records)
        {
            if (records.Count() == 0) { return PersonInfoModel.New(); }

            var model            = PersonInfoModel.New();
            model.Id             = records.First<PersonInfo>().Id;
            model.PersonGroupId  = records.First<PersonInfo>().PersonGroupId;
            model.PersonId       = records.First<PersonInfo>().PersonId;
            model.PersonName     = records.First<PersonInfo>().PersonName;
            model.PersonNameYomi = records.First<PersonInfo>().PersonNameYomi;

            return model;
        }
    }
}

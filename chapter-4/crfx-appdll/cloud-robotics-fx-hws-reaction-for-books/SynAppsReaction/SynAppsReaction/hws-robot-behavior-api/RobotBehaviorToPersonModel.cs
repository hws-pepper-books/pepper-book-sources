using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwsRobotBehaviorApi
{
    public class RobotBehaviorToPersonModel
    {
        public static readonly string Active = "Active";
        public static readonly string InActive = "InActive";
        private static string SqlConnectionString;

        public long Id { set; get; }
        public string DeviceId { set; get; }
        public string PersonId { set; get; }
        public int RobotBehaviorId { set; get; }
        public string Weather { set; get; }
        public string Status { set; get; }
        public bool IsDeleted { set; get; }
        public DateTimeOffset CreatedAt { set; get; }

        //Relation
        public string ActionType { set; get; }
        public string ActionBody { set; get; }

        RobotBehaviorToPersonModel()
        {
            this.Id = 0;
            this.Status = Active;
            this.IsDeleted = false;
        }

        public static void Connection(string _connectionString)
        {
            SqlConnectionString = _connectionString;
        }

        public static RobotBehaviorToPersonModel New()
        {
            return new RobotBehaviorToPersonModel();
        }

        public static List<RobotBehaviorToPersonModel> FindAllByDeviceIdAndPersonId(string _deviceId, string _personId)
        {
            List<RobotBehaviorToPersonModel> list = new List<RobotBehaviorToPersonModel>();

            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var records =
                from n in dc.RobotBehaviorToPersons
                join b in dc.RobotBehaviors on n.RobotBehaviorId equals b.SynAppsId
                where n.DeviceId == _deviceId
                where n.PersonId == _personId
                where n.IsDeleted == false
                orderby n.CreatedAt descending
                select new {
                    n.Id,
                    n.DeviceId,
                    n.PersonId,
                    n.RobotBehaviorId,
                    n.Weather,
                    n.Status,
                    n.CreatedAt,
                    b.ActionType,
                    b.ActionBody
                };

            foreach (var r in records)
            {
                var model = RobotBehaviorToPersonModel.New();
                model.Id       = r.Id;
                model.DeviceId = r.DeviceId;
                model.PersonId = r.PersonId;
                model.RobotBehaviorId = r.RobotBehaviorId;
                model.Status = r.Status;
                model.Weather = r.Weather;
                model.CreatedAt = r.CreatedAt;
                model.ActionType = r.ActionType;
                model.ActionBody = r.ActionBody;

                list.Add(model);
            }

            return list;
        }

        public static RobotBehaviorToPersonModel FindLatestOne(string _deviceId, string _personId)
        {
            var list = FindAllByDeviceIdAndPersonId(_deviceId, _personId);
            return list.Count > 0 ? list.First() : null;
        }

        public static int NumberOfMeetToday(string _deviceId, string _personId)
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var count =
                (from n in dc.RobotBehaviorToPersons
                 where n.DeviceId == _deviceId
                 where n.PersonId == _personId
                 where n.IsDeleted == false
                 where n.CreatedAt >= DateTime.Now.Date
                 select n).Count();

            return count;
        }

        public void Save()
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            if (this.Id == 0)
            {
                var robotBehaviorToPerson       = new RobotBehaviorToPerson();
                robotBehaviorToPerson.DeviceId  = this.DeviceId;
                robotBehaviorToPerson.PersonId  = this.PersonId;
                robotBehaviorToPerson.RobotBehaviorId = this.RobotBehaviorId;
                robotBehaviorToPerson.Status    = this.Status;
                robotBehaviorToPerson.Weather   = this.Weather;
                robotBehaviorToPerson.IsDeleted = this.IsDeleted;
                robotBehaviorToPerson.CreatedAt = DateTime.Now;

                dc.RobotBehaviorToPersons.InsertOnSubmit(robotBehaviorToPerson);
                dc.SubmitChanges();

                this.Id        = robotBehaviorToPerson.Id;
                this.CreatedAt = robotBehaviorToPerson.CreatedAt;
            }
            else
            {
                var records =
                    from n in dc.RobotBehaviorToPersons
                    where n.Id == this.Id
                    select n;

                foreach (var r in records)
                {
                    r.PersonId = this.PersonId;
                    r.RobotBehaviorId = this.RobotBehaviorId;
                    r.Status = this.Status;
                    r.Weather = this.Weather;
                    r.IsDeleted = this.IsDeleted;
                    r.UpdatedAt = DateTime.Now;
                }
                dc.SubmitChanges();
            }
            
        }
    }
}

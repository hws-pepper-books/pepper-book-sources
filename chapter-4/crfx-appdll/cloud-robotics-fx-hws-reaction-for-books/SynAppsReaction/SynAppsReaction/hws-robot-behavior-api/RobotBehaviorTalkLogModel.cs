using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwsRobotBehaviorApi
{
    public class RobotBehaviorTalkLogModel
    {
        public static readonly string Active = "Active";
        public static readonly string InActive = "InActive";
        private static string SqlConnectionString;

        public long Id { set; get; }
        public string DeviceId { set; get; }
        public string PersonId { set; get; }
        public string RobotTalk { set; get; }
        public string PersonTalk { set; get; }
        public string PersonTalkKeyphrase { set; get; }
        public string Status { set; get; }
        public bool IsDeleted { set; get; }
        public DateTimeOffset CreatedAt { set; get; }

        //Relation
        public string ActionType { set; get; }
        public string ActionBody { set; get; }

        RobotBehaviorTalkLogModel()
        {
            this.Id = 0;
            this.Status = Active;
            this.IsDeleted = false;
        }

        public static void Connection(string _connectionString)
        {
            SqlConnectionString = _connectionString;
        }

        public static RobotBehaviorTalkLogModel New()
        {
            return new RobotBehaviorTalkLogModel();
        }

        public static List<RobotBehaviorTalkLogModel> FindAll(string _deviceId, string _personId)
        {
            List<RobotBehaviorTalkLogModel> list = new List<RobotBehaviorTalkLogModel>();

            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var records =
                from n in dc.RobotBehaviorTalkLogs
                where n.DeviceId == _deviceId
                where n.PersonId == _personId
                where n.IsDeleted == false
                orderby n.CreatedAt descending
                select n;

            return Build(records);
        }

        public static List<RobotBehaviorTalkLogModel> FindAllActive(string _deviceId, string _personId, string _keyphrase)
        {
            List<RobotBehaviorTalkLogModel> list = new List<RobotBehaviorTalkLogModel>();

            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var records =
                from n in dc.RobotBehaviorTalkLogs
                where n.DeviceId == _deviceId
                where n.PersonId == _personId
                where n.PersonTalkKeyphrase == _keyphrase
                where n.Status == Active
                where n.IsDeleted == false
                orderby n.CreatedAt descending
                select n;

            return Build(records);
        }

        public static List<RobotBehaviorTalkLogModel> FindAllActiveKeyphrase(string _deviceId, string _personId)
        {
            List<RobotBehaviorTalkLogModel> list = new List<RobotBehaviorTalkLogModel>();

            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var records =
                from n in dc.RobotBehaviorTalkLogs
                where n.DeviceId == _deviceId
                where n.PersonId == _personId
                where n.PersonTalkKeyphrase != null
                where n.Status == Active
                where n.IsDeleted == false
                orderby n.CreatedAt descending
                select n;

            return Build(records);
        }

        public void Save()
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            if (this.Id == 0)
            {
                var robotBehaviorTalkLog = new RobotBehaviorTalkLog();
                robotBehaviorTalkLog.DeviceId = this.DeviceId;
                robotBehaviorTalkLog.PersonId = this.PersonId;
                robotBehaviorTalkLog.RobotTalk = this.RobotTalk;
                robotBehaviorTalkLog.PersonTalk = this.PersonTalk;
                robotBehaviorTalkLog.PersonTalkKeyphrase = this.PersonTalkKeyphrase;
                robotBehaviorTalkLog.Status = this.Status;
                robotBehaviorTalkLog.IsDeleted = this.IsDeleted;
                robotBehaviorTalkLog.CreatedAt = DateTime.Now;

                dc.RobotBehaviorTalkLogs.InsertOnSubmit(robotBehaviorTalkLog);
                dc.SubmitChanges();

                this.Id = robotBehaviorTalkLog.Id;
                this.CreatedAt = robotBehaviorTalkLog.CreatedAt;
            }
            else
            {
                var records =
                    from n in dc.RobotBehaviorTalkLogs
                    where n.Id == this.Id
                    select n;

                foreach (var r in records)
                {
                    r.PersonId = this.PersonId;
                    r.RobotTalk = this.RobotTalk;
                    r.PersonTalk = this.PersonTalk;
                    r.PersonTalkKeyphrase = this.PersonTalkKeyphrase;
                    r.Status = this.Status;
                    r.IsDeleted = this.IsDeleted;
                    r.UpdatedAt = DateTime.Now;
                }
                dc.SubmitChanges();
            }
        }

        private static List<RobotBehaviorTalkLogModel> Build(IQueryable<RobotBehaviorTalkLog> records)
        {
            List<RobotBehaviorTalkLogModel> list = new List<RobotBehaviorTalkLogModel>();

            foreach (var r in records)
            {
                var model = RobotBehaviorTalkLogModel.New();
                model.Id = r.Id;
                model.DeviceId = r.DeviceId;
                model.PersonId = r.PersonId;
                model.RobotTalk = r.RobotTalk;
                model.PersonTalk = r.PersonTalk;
                model.PersonTalkKeyphrase = r.PersonTalkKeyphrase;
                model.Status = r.Status;

                list.Add(model);
            }

            return list;
        }
    }
}

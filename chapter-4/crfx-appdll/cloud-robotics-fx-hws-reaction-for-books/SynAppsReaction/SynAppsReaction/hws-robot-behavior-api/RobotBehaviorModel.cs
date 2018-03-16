using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwsRobotBehaviorApi
{
    public class RobotBehaviorModel
    {
        public static readonly string Active = "Active";
        public static readonly string InActive = "InActive";
        private static string SqlConnectionString;

        public long Id { set; get; }
        public int SynAppsId { set; get; }
        public string SynAppsDeviceId { set; get; }
        public string Status { set; get; }
        public string ActionType { set; get; }
        public string ActionBody { set; get; }
        public bool IsSynAppsLinked { set; get; }
        public bool IsDeleted { set; get; }
        public DateTimeOffset CreatedAt { set; get; }

        RobotBehaviorModel()
        {
            this.Id = 0;
            this.Status = Active;
            this.IsSynAppsLinked = false;
            this.IsDeleted = false;
        }

        public static void Connection(string _connectionString)
        {
            SqlConnectionString = _connectionString;
        }

        public static RobotBehaviorModel New()
        {
            return new RobotBehaviorModel();
        }

        public static List<RobotBehaviorModel> FindAll(string _synappsDeviceId)
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var records =
                from n in dc.RobotBehaviors
                where n.SynAppsDeviceId == _synappsDeviceId
                where n.IsDeleted == false
                select n;

            return Build(records);
        }

        public static RobotBehaviorModel Find(string _synappsDeviceId, string _actionType)
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var records =
                from n in dc.RobotBehaviors
                where n.SynAppsDeviceId == _synappsDeviceId
                where n.ActionType == _actionType
                where n.IsDeleted == false
                select n;

            var results = Build(records);
            return results.Count > 0 ? results.First() : null;
        }

        public static void Refresh(string _synappsDeviceId, List<RobotBehaviorModel> list)
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var synAppsIds = (from l in list select l.SynAppsId).ToList<int>();
            var forDelete =
                from n in dc.RobotBehaviors
                where n.SynAppsDeviceId == _synappsDeviceId
                where n.IsSynAppsLinked == true
                where !synAppsIds.Contains(n.SynAppsId)
                select n;
            Delete(forDelete);

            var newBehaviors = new List<RobotBehavior>();
            foreach (var l in list)
            {
                var behaviors =
                    from n in dc.RobotBehaviors
                    where n.SynAppsDeviceId == _synappsDeviceId
                    where n.IsSynAppsLinked == true
                    where n.SynAppsId == l.SynAppsId
                    select n;

                if (behaviors.Count() > 0)
                {
                    behaviors.First<RobotBehavior>().IsDeleted = false;
                    behaviors.First<RobotBehavior>().Status = l.Status;
                    behaviors.First<RobotBehavior>().ActionType = l.ActionType;
                    behaviors.First<RobotBehavior>().ActionBody = l.ActionBody;
                    behaviors.First<RobotBehavior>().UpdatedAt = DateTime.Now;
                }
                else
                {
                    newBehaviors.Add(new RobotBehavior
                    {
                        SynAppsId = l.SynAppsId,
                        SynAppsDeviceId = l.SynAppsDeviceId,
                        Status = l.Status,
                        ActionType = l.ActionType,
                        ActionBody = l.ActionBody,
                        IsSynAppsLinked = l.IsSynAppsLinked,
                        IsDeleted = l.IsDeleted,
                        CreatedAt = DateTime.Now
                    });
                }
            }
            dc.RobotBehaviors.InsertAllOnSubmit(newBehaviors);
            dc.SubmitChanges();
        }

        public void Save()
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            if (this.Id == 0)
            {
                var robotBehavior = new RobotBehavior();
                robotBehavior.SynAppsId = this.SynAppsId;
                robotBehavior.SynAppsDeviceId = this.SynAppsDeviceId;
                robotBehavior.Status = this.Status;
                robotBehavior.ActionType = this.ActionType;
                robotBehavior.ActionBody = this.ActionBody;
                robotBehavior.IsSynAppsLinked = this.IsSynAppsLinked;
                robotBehavior.IsDeleted = this.IsDeleted;
                robotBehavior.CreatedAt = DateTime.Now;

                dc.RobotBehaviors.InsertOnSubmit(robotBehavior);
                dc.SubmitChanges();

                this.Id = robotBehavior.Id;
                this.CreatedAt = robotBehavior.CreatedAt;
            }
            else
            {
                var records =
                    from n in dc.RobotBehaviors
                    where n.Id == this.Id
                    select n;

                foreach (var r in records)
                {
                    r.SynAppsId = this.SynAppsId;
                    r.SynAppsDeviceId = this.SynAppsDeviceId;
                    r.Status = this.Status;
                    r.ActionType = this.ActionType;
                    r.ActionBody = this.ActionBody;
                    r.IsSynAppsLinked = this.IsSynAppsLinked;
                    r.IsDeleted = this.IsDeleted;
                    r.UpdatedAt = DateTime.Now;
                }
                dc.SubmitChanges();
            }
        }

        private static List<RobotBehaviorModel> Build(IQueryable<RobotBehavior> records)
        {
            List<RobotBehaviorModel> list = new List<RobotBehaviorModel>();

            foreach (var r in records)
            {
                var model = RobotBehaviorModel.New();
                model.Id = r.Id;
                model.SynAppsId = r.SynAppsId;
                model.Status = r.Status;
                model.ActionType = r.ActionType;
                model.ActionBody = r.ActionBody;
                model.IsSynAppsLinked = r.IsSynAppsLinked;

                list.Add(model);
            }

            return list;
        }

        private static void Delete(IQueryable<RobotBehavior> records)
        {
            foreach (var r in records)
            {
                r.IsDeleted = true;
                r.UpdatedAt = DateTime.Now;
            }
        }
    }
}

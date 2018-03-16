using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwsRobotBehaviorApi
{
    public class RobotBehaviorKeyphraseModel
    {
        static public readonly string Active = "Active";
        static public readonly string InActive = "InActive";
        private static string SqlConnectionString;

        public long Id { set; get; }
        public int SynAppsId { set; get; }
        public string SynAppsDeviceId { set; get; }
        public string Keyphrase { set; get; }
        public string KeyphraseReply { set; get; }
        public string Status { set; get; }
        public bool IsSynAppsLinked { set; get; }
        public bool IsDeleted { set; get; }
        public DateTimeOffset CreatedAt { set; get; }

        RobotBehaviorKeyphraseModel()
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

        public static RobotBehaviorKeyphraseModel New()
        {
            return new RobotBehaviorKeyphraseModel();
        }

        public static List<RobotBehaviorKeyphraseModel> FindAll(string _synappsDeviceId)
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var records =
                from n in dc.RobotBehaviorKeyphrases
                where n.SynAppsDeviceId == _synappsDeviceId
                where n.IsDeleted == false
                select n;

            return Build(records);
        }

        public static RobotBehaviorKeyphraseModel Find(string _synappsDeviceId, string _keyphrase)
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var records =
                from n in dc.RobotBehaviorKeyphrases
                where n.SynAppsDeviceId == _synappsDeviceId
                where n.Keyphrase == _keyphrase
                where n.IsDeleted == false
                select n;

            var results = Build(records);
            return results.Count > 0 ? results.First() : null;
        }

        public static RobotBehaviorKeyphraseModel Find(string _synappsDeviceId, List<string> _keyphrases)
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var records =
                from n in dc.RobotBehaviorKeyphrases
                where n.SynAppsDeviceId == _synappsDeviceId
                where _keyphrases.Contains(n.Keyphrase)
                where n.IsDeleted == false
                select n;

            var results = Build(records);
            return results.Count > 0 ? results.First() : null;
        }

        public static void Refresh(string _synappsDeviceId, List<RobotBehaviorKeyphraseModel> list)
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            var synAppsIds = (from l in list select l.SynAppsId).ToList<int>();
            var forDelete =
                from n in dc.RobotBehaviorKeyphrases
                where n.SynAppsDeviceId == _synappsDeviceId
                where n.IsSynAppsLinked == true
                where !synAppsIds.Contains(n.SynAppsId)
                select n;
            Delete(forDelete);

            var newKeyphrases = new List<RobotBehaviorKeyphrase>();
            foreach (var l in list)
            {
                var keyphrases =
                    from n in dc.RobotBehaviorKeyphrases
                    where n.SynAppsDeviceId == _synappsDeviceId
                    where n.IsSynAppsLinked == true
                    where n.SynAppsId == l.SynAppsId
                    select n;

                if (keyphrases.Count() > 0)
                {
                    keyphrases.First<RobotBehaviorKeyphrase>().Keyphrase = l.Keyphrase;
                    keyphrases.First<RobotBehaviorKeyphrase>().KeyphraseReply = l.KeyphraseReply;
                    keyphrases.First<RobotBehaviorKeyphrase>().Status = l.Status;
                    keyphrases.First<RobotBehaviorKeyphrase>().UpdatedAt = DateTime.Now;
                }
                else
                {
                    newKeyphrases.Add(new RobotBehaviorKeyphrase
                    {
                        SynAppsId = l.SynAppsId,
                        SynAppsDeviceId = l.SynAppsDeviceId,
                        Keyphrase = l.Keyphrase,
                        KeyphraseReply = l.KeyphraseReply,
                        Status = l.Status,
                        IsSynAppsLinked = l.IsSynAppsLinked,
                        IsDeleted = l.IsDeleted,
                        CreatedAt = DateTime.Now
                    });
                }
            }
            dc.RobotBehaviorKeyphrases.InsertAllOnSubmit(newKeyphrases);
            dc.SubmitChanges();
        }
        
        public void Save()
        {
            var dc = new RobotBehaviorsDataContext(SqlConnectionString);
            if (this.Id == 0)
            {
                var robotBehaviorKeyphrase = new RobotBehaviorKeyphrase();
                robotBehaviorKeyphrase.SynAppsId = this.SynAppsId;
                robotBehaviorKeyphrase.SynAppsDeviceId = this.SynAppsDeviceId;
                robotBehaviorKeyphrase.Keyphrase = this.Keyphrase;
                robotBehaviorKeyphrase.KeyphraseReply = this.KeyphraseReply;
                robotBehaviorKeyphrase.Status = this.Status;
                robotBehaviorKeyphrase.IsSynAppsLinked = this.IsSynAppsLinked;
                robotBehaviorKeyphrase.IsDeleted = this.IsDeleted;
                robotBehaviorKeyphrase.CreatedAt = DateTime.Now;

                dc.RobotBehaviorKeyphrases.InsertOnSubmit(robotBehaviorKeyphrase);
                dc.SubmitChanges();

                this.Id        = robotBehaviorKeyphrase.Id;
                this.CreatedAt = robotBehaviorKeyphrase.CreatedAt;
            }
            else
            {
                var records =
                    from n in dc.RobotBehaviorKeyphrases
                    where n.Id == this.Id
                    select n;

                foreach (var r in records)
                {
                    r.SynAppsId = this.SynAppsId;
                    r.SynAppsDeviceId = this.SynAppsDeviceId;
                    r.Keyphrase = this.Keyphrase;
                    r.KeyphraseReply = this.KeyphraseReply;
                    r.Status = this.Status;
                    r.IsSynAppsLinked = this.IsSynAppsLinked;
                    r.IsDeleted = this.IsDeleted;
                    r.UpdatedAt = DateTime.Now;
                }
                dc.SubmitChanges();
            }
        }

        private static List<RobotBehaviorKeyphraseModel> Build(IQueryable<RobotBehaviorKeyphrase> records)
        {
            var list = new List<RobotBehaviorKeyphraseModel>();
            foreach (var r in records)
            {
                var model = RobotBehaviorKeyphraseModel.New();
                model.Id = r.Id;
                model.SynAppsId = r.SynAppsId;
                model.Keyphrase = r.Keyphrase;
                model.KeyphraseReply = r.KeyphraseReply;
                model.Status = r.Status;
                model.IsSynAppsLinked = r.IsSynAppsLinked;

                list.Add(model);
            }

            return list;
        }

        private static void Delete(IQueryable<RobotBehaviorKeyphrase> records)
        {
            foreach (var r in records)
            {
                r.IsDeleted = true;
                r.UpdatedAt = DateTime.Now;
            }
        }
    }
}

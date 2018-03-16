using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynAppsLuis
{
    public class SynAppsIntentModel
    {
        private static string SqlConnectionString;

        public long Id { set; get; }
        public string SynAppsDeviceId { set; get; }
        public string Name { set; get; }
        public string ReactionBody { set; get; }
        public string Entity { set; get; }
        public bool IsSynAppsLinked { set; get; }
        public bool IsDeleted { set; get; }

        SynAppsIntentModel()
        {
            this.Id = 0;
            this.IsSynAppsLinked = false;
            this.IsDeleted = false;
        }

        public static void Connection(string _connectionString)
        {
            SqlConnectionString = _connectionString;
        }

        public static SynAppsIntentModel New()
        {
            return new SynAppsIntentModel();
        }

        public static List<SynAppsIntentModel> FindAllByDeviceId(string _deviceId)
        {
            List<SynAppsIntentModel> list = new List<SynAppsIntentModel>();

            var dc = new SynAppsIntentDataContext(SqlConnectionString);
            var intents =
                from n in dc.SynAppsIntents
                where n.SynAppsDeviceId == _deviceId
                select n;

            foreach (var intent in intents)
            {
                var model = SynAppsIntentModel.New();
                model.SynAppsDeviceId = intent.SynAppsDeviceId;
                model.Name = intent.Name;
                model.ReactionBody = intent.ReactionBody;
                model.Entity = intent.Entity ?? "";

                list.Add(model);
            }

            return list;
        }

        public static List<SynAppsIntentModel> GetUnSyncedReactions(string deviceId)
        {
            var results = new List<SynAppsIntentModel>();
            var dc = new SynAppsIntentDataContext(SqlConnectionString);
            var reactions =
                from n in dc.SynAppsIntents
                where n.SynAppsDeviceId == deviceId
                where n.IsSynAppsLinked == false
                where n.IsDeleted == false
                select n;

            foreach (var reaction in reactions)
            {
                var model = New();
                model.Id = reaction.Id;
                model.SynAppsDeviceId = reaction.SynAppsDeviceId;
                model.Name = reaction.Name;
                model.ReactionBody = reaction.ReactionBody;
                model.Entity = reaction.Entity;

                results.Add(model);
            }

            return results;
        }

        public static void Refresh(string _deviceId, List<SynAppsIntentModel> list)
        {
            var dc = new SynAppsIntentDataContext(SqlConnectionString);
            var intentForDelete =
                from n in dc.SynAppsIntents
                where n.SynAppsDeviceId == _deviceId
                where n.IsSynAppsLinked == true
                select n;
            dc.SynAppsIntents.DeleteAllOnSubmit(intentForDelete);

            var intents = new List<SynAppsIntent>();
            foreach (var intent in list)
            {
                intents.Add(new SynAppsIntent
                {
                    SynAppsDeviceId = intent.SynAppsDeviceId,
                    Name = intent.Name,
                    ReactionBody = intent.ReactionBody,
                    Entity = intent.Entity,
                    IsSynAppsLinked = intent.IsSynAppsLinked,
                    CreatedAt = DateTime.UtcNow
                });
            }
            dc.SynAppsIntents.InsertAllOnSubmit(intents);

            dc.SubmitChanges();
        }

        public static bool IsRefresh(string _deviceId)
        {
            bool isRefresh = true;

            var dc = new SynAppsIntentDataContext(SqlConnectionString);
            var intents =
                (from n in dc.SynAppsIntents
                 where n.SynAppsDeviceId == _deviceId
                 select n.CreatedAt).Take(1);

            foreach (var createAt in intents)
            {
                TimeSpan diff = DateTime.UtcNow - createAt.Value;
                if (diff.TotalMinutes < 60)
                {
                    isRefresh = false;
                }
            }

            return isRefresh;
        }

        public void Save()
        {
            var dc = new SynAppsIntentDataContext(SqlConnectionString);
            if (this.Id == 0)
            {
                dc.SynAppsIntents.InsertOnSubmit(new SynAppsIntent
                    {
                        SynAppsDeviceId = this.SynAppsDeviceId,
                        Name = this.Name,
                        ReactionBody = this.ReactionBody,
                        Entity = this.Entity,
                        IsSynAppsLinked = this.IsSynAppsLinked,
                        IsDeleted = this.IsDeleted,
                        CreatedAt = DateTime.UtcNow
                    });
            }
            else
            {
                var reactions =
                    from n in dc.SynAppsIntents
                    where n.Id == this.Id
                    select n;

                foreach (var r in reactions)
                {
                    r.Name = this.Name;
                    r.ReactionBody = this.ReactionBody;
                    r.Entity = this.Entity;
                    r.IsSynAppsLinked = this.IsSynAppsLinked;
                    r.IsDeleted = this.IsDeleted;
                    r.UpdatedAt = DateTime.UtcNow;
                }
            }
            dc.SubmitChanges();
        }
    }
}

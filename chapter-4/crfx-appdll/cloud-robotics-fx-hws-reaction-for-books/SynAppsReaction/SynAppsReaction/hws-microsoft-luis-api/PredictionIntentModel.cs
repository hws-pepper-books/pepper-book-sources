using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynAppsLuis
{
    public class PredictionIntentModel
    {
        private static string SqlConnectionString;
        public long Id { set; get; }
        public string SynAppsDeviceId { set; get; }
        public long LuisExampleId { set; get; }
        public string Intent { set; get; }
        public DateTimeOffset CreatedAt { set; get; }
        public DateTimeOffset UpdatedAt { set; get; }

        public string EntityName { set; get; } // Relation

        public string EntityValue { set; get; } // Relation

        PredictionIntentModel()
        {
            this.Id = 0;
        }

        public static void Connection(string _connectionString)
        {
            SqlConnectionString = _connectionString;
        }

        public static PredictionIntentModel New()
        {
            return new PredictionIntentModel();
        }

        public static PredictionIntentModel FindBySynAppsDeviceIdAndLuisExampleId(string _synappsDeviceId, long _luisExampleId)
        {
            var dc = new PredictionExamplesDataContext(SqlConnectionString);
            var records =
                from n in dc.PredictionIntents
                where n.SynAppsDeviceId == _synappsDeviceId
                where n.LuisExampleId == _luisExampleId
                select n;

            var list = Build(records);
            if (list.Count > 0)
            {
                return list.First();
            }
            else
            {
                return null;
            }
        }

        public static List<PredictionIntentModel> FindAllBySynAppsDeviceIdAndIntentOrderByLuisExampleId(string _synappsDeviceId, string _intent)
        {
            List<PredictionIntentModel> list = new List<PredictionIntentModel>();

            var dc = new PredictionExamplesDataContext(SqlConnectionString);
            var records =
                from n in dc.PredictionIntents
                join b in dc.PredictionEntities on n.LuisExampleId equals b.LuisExampleId
                where n.SynAppsDeviceId == _synappsDeviceId
                where n.Intent == _intent
                where b.SynAppsDeviceId == _synappsDeviceId
                orderby n.LuisExampleId
                select new
                {
                    Id = n.Id,
                    SynAppsDeviceId = n.SynAppsDeviceId,
                    LuisExampleId   = n.LuisExampleId,
                    Intent          = n.Intent,
                    EntityName      = b.EntityName,
                    EntityValue     = b.EntityValue,
                    CreatedAt       = n.CreatedAt,
                    UpdatedAt       = n.UpdatedAt
                };

            foreach (var r in records)
            {
                var model = PredictionIntentModel.New();
                model.Id              = r.Id;
                model.SynAppsDeviceId = r.SynAppsDeviceId;
                model.LuisExampleId   = r.LuisExampleId;
                model.Intent          = r.Intent;
                model.EntityName      = r.EntityName;
                model.EntityValue     = r.EntityValue;
                model.CreatedAt       = r.CreatedAt;
                model.UpdatedAt       = r.UpdatedAt;

                list.Add(model);
            }

            return list;
        }

        public static Dictionary<long, List<PredictionIntentModel>> FindAllBySynAppsDeviceIdAndIntentGroupByLuisExampleId(string _synappsDeviceId, string _intent)
        {
            var results = new Dictionary<long, List<PredictionIntentModel>>();
            var list = FindAllBySynAppsDeviceIdAndIntentOrderByLuisExampleId(_synappsDeviceId, _intent);
            foreach (var model in list)
            {
                if (results.ContainsKey(model.LuisExampleId))
                {
                    var l = results[model.LuisExampleId];
                    l.Add(model);
                    results[model.LuisExampleId] = l;
                }
                else
                {
                    var l = new List<PredictionIntentModel>();
                    l.Add(model);
                    results.Add(model.LuisExampleId, l);
                }
            }

            return results;
        }

        public static ApiResult Refresh(string _synappsDeviceId, List<PredictionIntentModel> _list)
        {
            var apiResult = new ApiResult { StatusCode = StatusCode.Success };
            var dc = new PredictionExamplesDataContext(SqlConnectionString);
            dc.Connection.Open();
            using (dc.Transaction = dc.Connection.BeginTransaction())
            {
                try
                {
                    var forDelete =
                        from n in dc.PredictionIntents
                        where n.SynAppsDeviceId == _synappsDeviceId
                        select n;
                    dc.PredictionIntents.DeleteAllOnSubmit(forDelete);

                    var now = DateTime.UtcNow;
                    var intents = new List<PredictionIntent>();
                    foreach (var intent in _list)
                    {
                        intents.Add(new PredictionIntent
                        {
                            SynAppsDeviceId = intent.SynAppsDeviceId,
                            LuisExampleId = intent.LuisExampleId,
                            Intent = intent.Intent,
                            CreatedAt = now,
                            UpdatedAt = now
                        });
                    }
                    dc.PredictionIntents.InsertAllOnSubmit(intents);
                    dc.SubmitChanges();
                    dc.Transaction.Commit();
                }
                catch (Exception e)
                {
                    dc.Transaction.Rollback();
                    apiResult.StatusCode = StatusCode.Error;
                    apiResult.Message = e.Message;
                }
            }
            dc.Connection.Close();

            return apiResult;
        }

        public ApiResult Save()
        {
            var apiResult = new ApiResult() { StatusCode = StatusCode.Success };
            var now = DateTime.UtcNow;
            var dc = new PredictionExamplesDataContext(SqlConnectionString);
            if (this.Id == 0)
            {
                var r = new PredictionIntent();
                r.SynAppsDeviceId = this.SynAppsDeviceId;
                r.LuisExampleId = this.LuisExampleId;
                r.Intent = this.Intent;
                r.CreatedAt = now;
                r.UpdatedAt = now;

                try
                {
                    dc.PredictionIntents.InsertOnSubmit(r);
                    dc.SubmitChanges();

                    this.Id = r.Id;
                    this.CreatedAt = r.CreatedAt;
                    this.UpdatedAt = r.UpdatedAt;
                }
                catch (Exception e)
                {
                    apiResult.StatusCode = StatusCode.Error;
                    apiResult.Message = e.Message;
                }
            }
            else
            {
                var records =
                    from n in dc.PredictionIntents
                    where n.Id == this.Id
                    select n;

                foreach (var r in records)
                {
                    r.SynAppsDeviceId = this.SynAppsDeviceId;
                    r.LuisExampleId   = this.LuisExampleId;
                    r.Intent          = this.Intent;
                    r.UpdatedAt       = now;
                }

                try
                {
                    dc.SubmitChanges();
                }
                catch (Exception e)
                {
                    apiResult.StatusCode = StatusCode.Error;
                    apiResult.Message = e.Message;
                }
            }

            return apiResult;
        }

        private static List<PredictionIntentModel> Build(IQueryable<PredictionIntent> records)
        {
            var list = new List<PredictionIntentModel>();

            foreach (var r in records)
            {
                var model = PredictionIntentModel.New();
                model.Id = r.Id;
                model.SynAppsDeviceId = r.SynAppsDeviceId;
                model.LuisExampleId = r.LuisExampleId;
                model.Intent = r.Intent;
                model.CreatedAt = r.CreatedAt;
                model.UpdatedAt = r.UpdatedAt;

                list.Add(model);
            }

            return list;
        }
    }
}

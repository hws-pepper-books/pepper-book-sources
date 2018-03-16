using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynAppsLuis
{
    public class PredictionEntityModel
    {
        private static string SqlConnectionString;

        public long Id { set; get; }
        public string SynAppsDeviceId { set; get; }
        public long LuisExampleId { set; get; }
        public string EntityName { set; get; }
        public string EntityValue { set; get; }
        public DateTimeOffset CreatedAt { set; get; }
        public DateTimeOffset UpdatedAt { set; get; }

        PredictionEntityModel()
        {
            this.Id = 0;
        }

        public static void Connection(string _connectionString)
        {
            SqlConnectionString = _connectionString;
        }

        public static PredictionEntityModel New()
        {
            return new PredictionEntityModel();
        }

        public static List<PredictionEntityModel> FindAllBySynAppsDeviceId(string _synappsDeviceId)
        {
            var dc = new PredictionExamplesDataContext(SqlConnectionString);
            var records =
                from n in dc.PredictionEntities
                where n.SynAppsDeviceId == _synappsDeviceId
                select n;

            return Build(records);
        }

        public static Dictionary<string, List<PredictionEntityModel>> FindAllBySynAppsDeviceIdGroupByEntityName(string _synappsDeviceId)
        {
            var results = new Dictionary<string, List<PredictionEntityModel>>();
            var list = FindAllBySynAppsDeviceId(_synappsDeviceId);
            foreach (var model in list)
            {
                if (results.ContainsKey(model.EntityName))
                {
                    var l = results[model.EntityName];
                    l.Add(model);
                    results[model.EntityName] = l;
                }
                else
                {
                    var l = new List<PredictionEntityModel>();
                    l.Add(model);
                    results.Add(model.EntityName, l);
                }
            }

            return results;
        }

        public static PredictionEntityModel FindBySynAppsDeviceIdAndLuisExampleId(string _synappsDeviceId, long _luisExampleId)
        {
            var dc = new PredictionExamplesDataContext(SqlConnectionString);
            var records =
                from n in dc.PredictionEntities
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

        public static ApiResult Refresh(string _synappsDeviceId, List<PredictionEntityModel> _list)
        {
            var apiResult = new ApiResult { StatusCode = StatusCode.Success };
            var dc = new PredictionExamplesDataContext(SqlConnectionString);
            dc.Connection.Open();
            using (dc.Transaction = dc.Connection.BeginTransaction())
            {
                try
                {
                    var forDelete =
                        from n in dc.PredictionEntities
                        where n.SynAppsDeviceId == _synappsDeviceId
                        select n;
                    dc.PredictionEntities.DeleteAllOnSubmit(forDelete);

                    var now = DateTime.UtcNow;
                    var entities = new List<PredictionEntity>();
                    foreach (var entity in _list)
                    {
                        entities.Add(new PredictionEntity
                        {
                            SynAppsDeviceId = entity.SynAppsDeviceId,
                            LuisExampleId   = entity.LuisExampleId,
                            EntityName      = entity.EntityName,
                            EntityValue     = entity.EntityValue,
                            CreatedAt       = now,
                            UpdatedAt       = now
                        });
                    }
                    dc.PredictionEntities.InsertAllOnSubmit(entities);
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
                var r = new PredictionEntity();
                r.SynAppsDeviceId = this.SynAppsDeviceId;
                r.LuisExampleId   = this.LuisExampleId;
                r.EntityName      = this.EntityName;
                r.EntityValue     = this.EntityValue;
                r.CreatedAt       = now;
                r.UpdatedAt       = now;

                try
                {
                    dc.PredictionEntities.InsertOnSubmit(r);
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
                    from n in dc.PredictionEntities
                    where n.Id == this.Id
                    select n;

                foreach (var r in records)
                {
                    r.SynAppsDeviceId = this.SynAppsDeviceId;
                    r.LuisExampleId   = this.LuisExampleId;
                    r.EntityName      = this.EntityName;
                    r.EntityValue     = this.EntityValue;
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

        private static List<PredictionEntityModel> Build(IQueryable<PredictionEntity> records)
        {
            var list = new List<PredictionEntityModel>();

            foreach (var r in records)
            {
                var model = PredictionEntityModel.New();
                model.Id = r.Id;
                model.SynAppsDeviceId = r.SynAppsDeviceId;
                model.LuisExampleId = r.LuisExampleId;
                model.EntityName = r.EntityName;
                model.EntityValue = r.EntityValue;
                model.CreatedAt = r.CreatedAt;
                model.UpdatedAt = r.UpdatedAt;

                list.Add(model);
            }

            return list;
        }
    }
}

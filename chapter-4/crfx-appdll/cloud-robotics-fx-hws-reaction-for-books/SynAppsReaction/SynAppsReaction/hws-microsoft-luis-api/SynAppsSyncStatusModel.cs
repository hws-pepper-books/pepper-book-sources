using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynAppsLuis
{
    public class SynAppsSyncStatusModel
    {
        private static string SqlConnectionString;

        public long Id { set; get; }
        public string SynAppsDeviceId { set; get; }
        public string Status { set; get; }
        public DateTimeOffset LastTrainingDateTime { set; get; }
        public DateTimeOffset CreatedAt { set; get; }
        public DateTimeOffset UpdatedAt { set; get; }

        private const string None = "None";
        private const string Processing = "Processing";

        SynAppsSyncStatusModel()
        {
            this.Id = 0;
            this.Status = None;
            this.LastTrainingDateTime = DateTimeOffset.Parse("1970-01-01 00:00:00");
        }

        public static void Connection(string _connectionString)
        {
            SqlConnectionString = _connectionString;
        }

        public static SynAppsSyncStatusModel New()
        {
            return new SynAppsSyncStatusModel();
        }

        private static List<SynAppsSyncStatusModel> FindAllBySynAppsDeviceId(string _synappsDeviceId)
        {
            var dc = new SynAppsSyncStatusesDataContext(SqlConnectionString);
            var records =
                from n in dc.SynAppsSyncStatuses
                where n.SynAppsDeviceId == _synappsDeviceId
                select n;

            return Build(records);
        }

        public static SynAppsSyncStatusModel FindBySynAppsDeviceId(string _synappsDeviceId)
        {
            var list = FindAllBySynAppsDeviceId(_synappsDeviceId);
            if (list.Count > 0)
            {
                return list.First();
            }
            else
            {
                var m = New();
                m.SynAppsDeviceId = _synappsDeviceId;

                return m;
            }
        }

        public ApiResult StartProcess()
        {
            var apiResult = new ApiResult() { StatusCode = StatusCode.Success };
            if (this.Status == None)
            {
                this.Status = Processing;
                apiResult   = Save();
            }
            else
            {
                apiResult.StatusCode = StatusCode.Error;
                apiResult.Message = "Processing.";
            }
            return apiResult;
        }

        public ApiResult FinishProcess()
        {
            var apiResult = new ApiResult() { StatusCode = StatusCode.Success };

            this.Status = None;
            apiResult   = Save();

            return apiResult;
        }

        public ApiResult Save()
        {
            var apiResult = new ApiResult() { StatusCode = StatusCode.Success };
            var now = DateTime.UtcNow;
            var dc = new SynAppsSyncStatusesDataContext(SqlConnectionString);
            if (this.Id == 0)
            {
                var r = new SynAppsSyncStatuse();
                r.SynAppsDeviceId      = this.SynAppsDeviceId;
                r.Status               = this.Status;
                r.LastTrainingDateTime = this.LastTrainingDateTime.DateTime;
                r.CreatedAt            = now;
                r.UpdatedAt            = now;

                try
                {
                    dc.SynAppsSyncStatuses.InsertOnSubmit(r);
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
                    from n in dc.SynAppsSyncStatuses
                    where n.Id == this.Id
                    select n;

                foreach (var r in records)
                {
                    r.SynAppsDeviceId      = this.SynAppsDeviceId;
                    r.Status               = this.Status;
                    r.LastTrainingDateTime = this.LastTrainingDateTime.DateTime;
                    r.UpdatedAt            = now;
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

        private static List<SynAppsSyncStatusModel> Build(IQueryable<SynAppsSyncStatuse> records)
        {
            var list = new List<SynAppsSyncStatusModel>();

            foreach (var r in records)
            {
                var model = SynAppsSyncStatusModel.New();
                model.Id                   = r.Id;
                model.SynAppsDeviceId      = r.SynAppsDeviceId;
                model.Status               = r.Status;
                model.LastTrainingDateTime = r.LastTrainingDateTime;
                model.CreatedAt            = r.CreatedAt;
                model.UpdatedAt            = r.UpdatedAt;

                list.Add(model);
            }

            return list;
        }
    }
}

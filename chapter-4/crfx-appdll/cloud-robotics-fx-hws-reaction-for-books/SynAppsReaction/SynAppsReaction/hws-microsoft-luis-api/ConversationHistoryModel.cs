using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynAppsLuis
{
    public class ConversationHistoryModel
    {
        private static string SqlConnectionString;

        public long Id { set; get; }
        public string DeviceId { set; get; }
        public string FromUserId { set; get; }
        public string FromMessage { set; get; }
        public string ToMessage { set; get; }
        public string Intent { set; get; }
        public double Score { set; get; }
        public long SynAppsAccountId { set; get; }
        public string SynAppsAccountName { set; get; }
        public long SynAppAssetId { set; get; }
        public ConversationStatus Status { set; get; }
        public bool IsSynAppsLinked { set; get; }
        public DateTimeOffset CreatedAt { set; get; }
        public DateTimeOffset UpdatedAt { set; get; }

        ConversationHistoryModel()
        {
            this.Id = 0;
            this.SynAppsAccountId = 0;
            this.SynAppAssetId = 0;
        }

        public static void Connection(string _connectionString)
        {
            SqlConnectionString = _connectionString;
        }

        public static ConversationHistoryModel New()
        {
            return new ConversationHistoryModel();
        }

        public static ConversationHistoryModel FindById(long _id)
        {
            var dc = new ConversationHistoriesDataContext(SqlConnectionString);
            var histories =
                from n in dc.ConversationHistories
                where n.Id == _id
                select n;

            var list = Build(histories);
            if (list.Count > 0)
            {
                return list.First<ConversationHistoryModel>();
            }
            else
            {
                return null;
            }
        }

        public static List<ConversationHistoryModel> FindAllByDeviceId(string _deviceId)
        {
            var dc = new ConversationHistoriesDataContext(SqlConnectionString);
            var histories =
                from n in dc.ConversationHistories
                where n.DeviceId == _deviceId
                select n;

            return Build(histories);
        }

        public static List<ConversationHistoryModel> FindAllByDeviceId(string _deviceId, ConversationStatus _status)
        {
            var dc = new ConversationHistoriesDataContext(SqlConnectionString);
            var histories =
                from n in dc.ConversationHistories
                where n.DeviceId == _deviceId
                where n.Status == _status.ObtainStatus()
                select n;

            return Build(histories);
        }

        public static List<ConversationHistoryModel> FindAllByDeviceId(string _deviceId, ConversationStatus _status, string _fromMessage)
        {
            var dc = new ConversationHistoriesDataContext(SqlConnectionString);
            var histories =
                from n in dc.ConversationHistories
                where n.DeviceId == _deviceId
                where n.Status == _status.ObtainStatus()
                where n.FromMessage == _fromMessage
                select n;

            return Build(histories);
        }

        public static List<ConversationHistoryModel> FindAllByDeviceIdAndSynAppsUnlink(string _deviceId)
        {
            var dc = new ConversationHistoriesDataContext(SqlConnectionString);
            var histories =
                from n in dc.ConversationHistories
                where n.DeviceId == _deviceId
                where n.IsSynAppsLinked == false
                select n;

            return Build(histories);
        }
        
        public static ConversationHistoryModel FindByDeviceId(string _deviceId)
        {
            var list = FindAllByDeviceId(_deviceId);
            if (list.Count > 0)
            {
                return list.First<ConversationHistoryModel>();
            }
            else
            {
                return null;
            }
        }

        public static ConversationHistoryModel FindByDeviceId(string _deviceId, ConversationStatus _status)
        {
            var list = FindAllByDeviceId(_deviceId, _status);
            if (list.Count > 0)
            {
                return list.First<ConversationHistoryModel>();
            }
            else
            {
                return null;
            }
        }

        public static List<ConversationHistoryModel> FindAllOfUnknownMessageByDeviceIdAndFromMessage(string _deviceId, string _fromMessage)
        {
            var dc = new ConversationHistoriesDataContext(SqlConnectionString);
            var histories =
                from n in dc.ConversationHistories
                where n.DeviceId == _deviceId
                where (n.Status == ConversationStatus.None.ObtainStatus() || n.Status == ConversationStatus.Miss.ObtainStatus())
                where n.FromMessage == _fromMessage
                select n;

            return Build(histories);
        }

        public static List<ConversationHistoryModel> FindAllOfUnknownMessageByDeviceId(string _deviceId)
        {
            var dc = new ConversationHistoriesDataContext(SqlConnectionString);
            var histories =
                from n in dc.ConversationHistories
                where n.DeviceId == _deviceId
                where (n.Status == ConversationStatus.None.ObtainStatus() || n.Status == ConversationStatus.Miss.ObtainStatus())
                select n;

            return Build(histories);
        }

        public static ConversationHistoryModel FindOneOfUnknownMessageByDeviceId(string _deviceId)
        {
            var list = FindAllOfUnknownMessageByDeviceId(_deviceId);
            if (list.Count > 0)
            {
                return list.First<ConversationHistoryModel>();
            }
            else
            {
                return null;
            }
        }

        public static ApiResult AddMessageText(string _deviceId, string _fromUserId, string _fromMessage, string _toMessage, string _intent, double _score, long _synAppsAccountId, string _synAppsAccountName, long _assetId, ConversationStatus _status)
        {
            var apiResult = new ApiResult() { StatusCode = StatusCode.Success };
            var now = DateTime.UtcNow;
            var dc = new ConversationHistoriesDataContext(SqlConnectionString);
            var conversationHistory = new ConversationHistory
            {
                DeviceId           = _deviceId,
                FromUserId         = _fromUserId,
                FromMessage        = _fromMessage,
                ToMessage          = _toMessage,
                Intent             = _intent,
                Score              = _score,
                SynAppsAccountId   = _synAppsAccountId,
                SynAppsAccountName = _synAppsAccountName,
                SynAppAssetId      = _assetId,
                Status             = _status.ObtainStatus(),
                UpdatedAt          = now,
                CreatedAt          = now
            };

            try
            {
                dc.ConversationHistories.InsertOnSubmit(conversationHistory);
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                apiResult.StatusCode = StatusCode.Error;
                apiResult.Message = e.Message;
            }

            return apiResult;
        }

        public void Learned()
        {
            this.Status = ConversationStatus.Learned;
            this.IsSynAppsLinked = false;
            Save();
        }

        public void DenyTeach()
        {
            this.Status = ConversationStatus.DenyTeach;
            this.IsSynAppsLinked = false;
            Save();
        }

        public ApiResult Save()
        {
            var apiResult = new ApiResult() { StatusCode = StatusCode.Success };
            var now = DateTime.UtcNow;
            var dc = new ConversationHistoriesDataContext(SqlConnectionString);
            if (this.Id == 0)
            {
                var r = new ConversationHistory();
                r.DeviceId    = this.DeviceId;
                r.FromUserId  = this.FromUserId;
                r.FromMessage = this.FromMessage;
                r.ToMessage   = this.ToMessage;
                r.Intent      = this.Intent;
                r.Score       = this.Score;
                r.Status      = this.Status.ObtainStatus();
                r.IsSynAppsLinked = this.IsSynAppsLinked;
                r.CreatedAt   = now;
                r.UpdatedAt   = now;

                try
                {
                    dc.ConversationHistories.InsertOnSubmit(r);
                    dc.SubmitChanges();

                    this.Id = r.Id;
                    this.CreatedAt = r.CreatedAt;
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
                    from n in dc.ConversationHistories
                    where n.Id == this.Id
                    select n;

                foreach (var r in records)
                {
                    r.DeviceId    = this.DeviceId;
                    r.FromUserId  = this.FromUserId;
                    r.FromMessage = this.FromMessage;
                    r.ToMessage   = this.ToMessage;
                    r.Intent      = this.Intent;
                    r.Score       = this.Score;
                    r.Status      = this.Status.ObtainStatus();
                    r.IsSynAppsLinked = this.IsSynAppsLinked;
                    r.UpdatedAt   = now;
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

        private static List<ConversationHistoryModel> Build(IQueryable<ConversationHistory> records)
        {
            var list = new List<ConversationHistoryModel>();

            foreach (var r in records)
            {
                var model = ConversationHistoryModel.New();
                model.Id                 = r.Id;
                model.DeviceId           = r.DeviceId;
                model.FromUserId         = r.FromUserId;
                model.FromMessage        = r.FromMessage;
                model.ToMessage          = r.ToMessage;
                model.Intent             = r.Intent;
                model.Score              = r.Score;
                model.SynAppsAccountId   = r.SynAppsAccountId;
                model.SynAppsAccountName = r.SynAppsAccountName;
                model.SynAppAssetId      = r.SynAppAssetId;
                model.Status             = ConversationStatusExt.ToEnum<ConversationStatus>(r.Status);
                model.IsSynAppsLinked    = r.IsSynAppsLinked;
                model.CreatedAt          = r.CreatedAt;
                model.UpdatedAt          = r.UpdatedAt;

                list.Add(model);
            }

            return list;
        }
    }

    public enum ConversationStatus
    {
        None,
        Learning,
        Learned,
        Notified,
        DenyTeach,
        Miss,
        Closed
    }

    static class ConversationStatusExt
    {
        public static string ObtainStatus(this ConversationStatus value)
        {
            string[] values = { "None", "Learning", "Learned", "Notified", "DenyTeach", "Miss", "Closed" };
            return values[(int)value];
        }

        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}

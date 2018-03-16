using System;

namespace SynAppsLuis
{
    [Serializable]
    public class MessageTextDto
    {
        public string FromUserId { set; get; }
        public string FromMessage { set; get; }
        public string ToMessage { set; get; }
        public string Intent { set; get; }
        public double Score { set; get; }
        public long SynAppsAccountId { set; get; }
        public string SynAppsAccountName { set; get; }
        public long SynAppsAssetId { set; get; }
        public ConversationStatus Status { set; get; }
    }
}

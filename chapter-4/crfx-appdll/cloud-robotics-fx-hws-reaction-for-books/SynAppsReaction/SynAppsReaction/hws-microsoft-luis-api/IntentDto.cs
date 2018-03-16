using System;

namespace SynAppsLuis
{
    [Serializable]
    public class IntentDto
    {
        public string Name { set; get; }
        public string ReactionBody { set; get; }
        public string Entity { set; get; }
    }
}

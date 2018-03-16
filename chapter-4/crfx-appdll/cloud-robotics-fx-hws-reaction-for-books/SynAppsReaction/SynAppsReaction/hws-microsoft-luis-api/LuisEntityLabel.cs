using System;

namespace SynAppsLuis
{
    [Serializable]
    public class LuisEntityLabel
    {
        public string EntityName { set; get; }
        public int StartCharIndex { set; get; }
        public int EndCharIndex { set; get; }
    }
}

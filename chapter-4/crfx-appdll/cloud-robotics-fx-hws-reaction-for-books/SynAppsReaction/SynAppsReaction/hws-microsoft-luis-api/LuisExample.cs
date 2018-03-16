using System;
using System.Collections.Generic;

namespace SynAppsLuis
{
    [Serializable]
    public class LuisExample
    {
        public string Text { set; get; }
        public string IntentName { set; get; }
        public List<LuisEntityLabel> EntityLabels { set; get; }

        public LuisExample()
        {
            this.EntityLabels = new List<LuisEntityLabel>();
        }
    }
}

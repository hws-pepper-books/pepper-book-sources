using SynAppsLuis;

namespace SynAppsReaction
{
    class SpeechAppBody
    {
        public RobotBehavior Behavior { set; get; }
        public bool IsNoReaction { set; get; }

        public SpeechAppBody ()
        {
            this.IsNoReaction = false;
        }
    }
}

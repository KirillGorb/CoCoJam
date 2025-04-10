using System;
using CodeScripts.Timeline.Model;

namespace CodeScripts.Timeline
{
    [Serializable]
    public struct CollapseData
    {
        public ECollapseMode Mode;
        public bool IsActivate;
    }
}
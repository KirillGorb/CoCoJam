using System.Collections.Generic;
using CodeScripts.Abstraction;
using CodeScripts.SaveLoadSystem;

namespace CodeScripts.Timeline
{
    public class TimelineSD : IData
    {
        public int IdOpenCollapse;
        public List<CollapseData> AllCollapseMode;
        public List<int> KeyCollapseActivate;
    }
}
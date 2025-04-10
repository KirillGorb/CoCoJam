using System.Collections.Generic;
using CodeScripts.SaveLoadSystem;

namespace CodeScripts.Timeline
{
    public class TimelineSD : ISaveData
    {
        public int IdOpenCollapse;
        public List<CollapseData> AllCollapseMode;
        public List<int> KeyCollapseActivate;

        public void LoadCdOnKey()
        {
            foreach (var key in KeyCollapseActivate)
            {
                var m = AllCollapseMode[key];
                m.IsActivate = false;
                AllCollapseMode[key] = m;
            }
        }
    }
}
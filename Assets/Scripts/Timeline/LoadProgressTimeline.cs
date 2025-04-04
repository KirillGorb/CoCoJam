using CodeScripts.SaveLoadSystem;
using CodeScripts.Timeline.Model;
using UnityEngine;

namespace CodeScripts.Timeline
{
    public class TimelineSD : ISaveData
    {
        public int IdOpenCollapse;
        public ECollapseMode[] AllCollapseMode;
    }

    public class LoadProgressTimeline
    {
        private readonly TimelineSD _data;
        private readonly GraphModel _model;
        private readonly Save<TimelineSD> _saver;

        public LoadProgressTimeline(Save<TimelineSD> saver, GraphModel model)
        {
            Debug.Log(11);
            _model = model;
            _saver = saver;
            _model.AllCollapse.LoadID();

            _data = saver.LoadData();
            if (_data?.AllCollapseMode is null)
            {
                var allCollapseMode = new ECollapseMode[model.AllCollapse.Containers.Count];

                allCollapseMode[0] = ECollapseMode.Active;
                for (var i = 1; i < allCollapseMode.Length; i++)
                    allCollapseMode[i] = ECollapseMode.Inactive;

                _data = new TimelineSD()
                {
                    IdOpenCollapse = 0,
                    AllCollapseMode = allCollapseMode
                };
                _saver.SaveData(_data);
            }
        }

        public void Next()
        {
            var i = _data.IdOpenCollapse;
            _data.AllCollapseMode[i] = ECollapseMode.End;
            _model.Find(i).Next.ForEach(e =>
                _data.AllCollapseMode[e.ID] =
                    _data.AllCollapseMode[e.ID] != ECollapseMode.End ? ECollapseMode.Active : ECollapseMode.End);

            _saver.SaveData(_data);
        }
    }
}
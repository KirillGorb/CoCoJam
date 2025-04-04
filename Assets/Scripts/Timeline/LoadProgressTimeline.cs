using CodeScripts.SaveLoadSystem;
using CodeScripts.Timeline.Model;
using UnityEngine;
using Zenject;

namespace CodeScripts.Timeline
{
    public class TimelineSD : ISaveData
    {
        public int IdOpenCollapse;
        public ECollapseMode[] AllCollapseMode;
    }

    public class LoadProgressTimeline : IInitializable
    {
        [Inject] private readonly GraphModel _model;
        [Inject] private readonly Save<TimelineSD> _saver;

        private TimelineSD _data;
        
        public void Initialize()
        {
            Debug.Log(11);
            _model.AllCollapse.LoadID();

            _data = _saver.LoadData();
            if (_data?.AllCollapseMode is null)
            {
                var allCollapseMode = new ECollapseMode[_model.AllCollapse.Containers.Count];

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
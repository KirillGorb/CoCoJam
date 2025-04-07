using System.Collections.Generic;
using System.Linq;
using CodeScripts.SaveLoadSystem;
using CodeScripts.Timeline.Model;
using ModestTree;
using Sirenix.Utilities;
using UniRx;
using Zenject;

namespace CodeScripts.Timeline
{
    public class TimelineSD : ISaveData
    {
        public int IdOpenCollapse;
        public List<ECollapseMode> AllCollapseMode;
    }

    public class LoadProgressTimeline : IInitializable
    {
        [Inject] private readonly TimelineData _data;
        [Inject] private readonly Save<TimelineSD> _saver;

        private readonly CompositeDisposable _disposable = new();

        public TimelineSD Load { get; private set; }

        public void Initialize()
        {
            var con = _data.AllCollapse.Containers;
            Load = _saver.LoadData();

            if (Load?.AllCollapseMode is null)
            {
                Load = new TimelineSD
                {
                    IdOpenCollapse = 0,
                    AllCollapseMode = con.Select(e => e.Mode.Value).ToList()
                };

                Sub();
                con[0].Mode.Value = ECollapseMode.Active;
                for (var i = 1; i < con.Length; i++)
                    con[i].Mode.Value = ECollapseMode.Inactive;

                _saver.SaveData(Load);
            }
            else
            {
                Sub();
                int i = 0;
                foreach (var mode in Load.AllCollapseMode)
                    con[i++].Mode.Value = mode;
            }
        }

        private void Sub()
        {
            int ik = 0;
            foreach (var collapse in _data.AllCollapse.Containers)
            {
                var ij = ik;
                collapse.ID = ik;
                collapse.Mode.Subscribe(e => Load.AllCollapseMode[ij] = e).AddTo(_disposable);
                ik++;
            }
        }

        public void SetID(CollapseModel collapse)
        {
            Load.IdOpenCollapse = collapse.ID;
            _saver.SaveData(Load);
        }

        public void Next()
        {
            var i = Load.IdOpenCollapse;
            var c = _data.AllCollapse.Containers[i];

            c.SetActiveModeOnBranch();

            _data.Timelines.Select(e =>
            {
                if (e.Timeline.Contains(c))
                    return (e.Timeline, e.Timeline.IndexOf(c));
                return default;
            }).ForEach(e =>
            {
                if (e.Item1 != null && e.Item2 + 1 < e.Item1.Length &&
                    e.Item1[e.Item2 + 1].Mode.Value is not (ECollapseMode.Ends or ECollapseMode.Cansel))
                {
                    e.Item1[e.Item2 + 1].Mode.Value = ECollapseMode.Active;
                }
            });

            var a = _data.GetAgeId(c);
            if (a > 0)
                for (int j = 0; j < a; j++)
                    foreach (var age in _data.Ages[j].Ages)
                        if (age.Mode.Value is not (ECollapseMode.Ends or ECollapseMode.Cansel))
                            age.Mode.Value = ECollapseMode.Rollback;

            _data.AllCollapse.Containers[i].Mode.Value = ECollapseMode.Ends;
            _saver.SaveData(Load);
        }
    }
}
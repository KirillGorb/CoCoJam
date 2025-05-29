using System;
using System.Linq;
using CodeScripts.PlayerInputs;
using CodeScripts.SaveLoadSystem;
using CodeScripts.Scene;
using CodeScripts.Timeline.Model;
using ModestTree;
using Sirenix.Utilities;
using UniRx;
using Zenject;

namespace CodeScripts.Timeline
{
    public class LoadProgressTimeline : IInitializable, IDisposable
    {
        [Inject] private readonly TimelineData _data;
        [Inject] private readonly KeyActivate _key;
        [Inject] private readonly SceneController _sceneManager;
        [Inject] private readonly Save<TimelineSD> _saver;

        private readonly CompositeDisposable _disposable = new();

        public TimelineSD Load { get; private set; }

        public void Initialize()
        {
            LoadOnSave();
            Subscribe();
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

            c.SetModeOnBranch(ECollapseMode.Active);

            _data.Timelines.Select(e =>
            {
                if (e.Timeline.Contains(c))
                    return (e.Timeline, e.Timeline.IndexOf(c));
                return default;
            }).ForEach(e =>
            {
                if (e.Item1 != null && e.Item2 + 1 < e.Item1.Length)
                    e.Item1[e.Item2 + 1].IsSetData(ECollapseMode.Active);
            });

            var a = _data.GetAgeId(c);
            if (a > 0)
                for (int j = 0; j <= a; j++)
                    foreach (var age in _data.Ages[j].Ages)
                        age.IsSetData(ECollapseMode.Rollback);

            c.IsSetData(ECollapseMode.Ends);
            _saver.SaveData(Load);
        }

        public void SetActive(int id, bool isActivate)
        {
            _data.AllCollapse.Containers[id].SetActivate(isActivate);
            _saver.SaveData(Load);
        }

        public bool GetActive(int id) => !_data.AllCollapse.Containers[id].Data.Value.IsActivate;

        public void SetActive((int id, bool isActivate)[] value)
        {
            foreach (var item in value)
                _data.AllCollapse.Containers[item.id].SetActivate(item.isActivate);
            _saver.SaveData(Load);
        }

        private void Sub()
        {
            int ik = 0;
            foreach (var collapse in _data.AllCollapse.Containers)
            {
                var ij = ik;
                collapse.ID = ik;
                collapse.Data.Subscribe(e => Load.AllCollapseMode[ij] = e).AddTo(_disposable);
                ik++;
            }
        }

        private void LoadActiveOnKey()
        {
            var con = _data.AllCollapse.Containers;
            foreach (var key in Load.KeyCollapseActivate)
            {
                var m = con[key].Data.Value;
                m.IsActivate = false;
                con[key].Data.Value = m;
            }
        }
        
        private void LoadOnSave()
        {
            var con = _data.AllCollapse.Containers;
            
            _saver.SetSave("collapse");
            Load = _saver.LoadData();

            if (Load?.AllCollapseMode is null)
            {
                Load = new TimelineSD
                {
                    IdOpenCollapse = 0,
                    AllCollapseMode = con.Select(e => e.Data.Value).ToList(),
                };

                Sub();
                Load.KeyCollapseActivate = _key.IdCollapse.Select(e => e.ID).ToList();
                con[0].Data.Value = new CollapseData { Mode = ECollapseMode.Active, IsActivate = true };
                for (var i = 1; i < con.Length; i++)
                    con[i].Data.Value = new CollapseData { Mode = ECollapseMode.Inactive, IsActivate = true };

                LoadActiveOnKey();
                _saver.SaveData(Load);
            }
            else
            {
                LoadActiveOnKey();
                int i = 0;
                foreach (var mode in Load.AllCollapseMode)
                    con[i++].Data.Value = mode;
                Sub();
            }
        }

        private void Subscribe()
        {
            InputCallback.Rallback.WhereU(e => e).Subscribe(_ =>
            {
                _data.AllCollapse.Containers[Load.IdOpenCollapse].IsSetData(ECollapseMode.Cansel, false);
                _saver.SaveData(Load);
                _sceneManager.SetScene(0);
            }).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
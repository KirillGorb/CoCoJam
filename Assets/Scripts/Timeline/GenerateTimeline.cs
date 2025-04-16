using CodeScripts.Timeline.Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using CodeScripts.Scene;
using UniRx;

namespace CodeScripts.Timeline
{
    public class GenerateTimeline : MonoBehaviour
    {
        [SerializeField] private MapItemView[] map;
        [SerializeField] private Scrollbar scroll;

        [Inject] private readonly TimelineData _data;
        [Inject] private readonly LoadProgressTimeline _loadProgressTimeline;
        [Inject] private readonly SceneController _scene;

        private int _ageLoad;

        private void Start()
        {
            scroll.onValueChanged.AddListener(e =>
            {
                var old = _ageLoad;
                _ageLoad = Mathf.FloorToInt(e * (_data.Ages.Length - 1));
                Debug.Log($"Selected Age Index: {_ageLoad}");

                if (_ageLoad != old) Render();
            });

            foreach (var item in map)
            {
                item.AgeFind += () => _ageLoad;
                
                item.Open
                    .WhereU(_ => item.InAge(out var c) && 
                                 c.Data.Value is { IsActivate: true, Mode: ECollapseMode.Active or ECollapseMode.Rollback })
                    .Subscribe(_ =>
                    {
                        item.InAge(out var col);
                        _loadProgressTimeline.SetID(col);
                        _scene.SetScene(col.ScenePlay);
                        _scene.SetScene(col.ScenePlay);
                    }).AddTo(this);
            }
            Render();
        }

        private void Render()
        {
            foreach (var item in map)
                item.RenderAge();
        }
    }
}
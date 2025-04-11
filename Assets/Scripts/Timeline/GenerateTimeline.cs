using System.Collections.Generic;
using CodeScripts.Scene;
using CodeScripts.Timeline.Model;
using CodeScripts.Timeline.View;
using Cysharp.Threading.Tasks;
using Plugins.Other;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeScripts.Timeline
{
    public class GenerateTimeline : MonoBehaviour
    {
        [SerializeField] private CollapseContainerView containerView;
        [SerializeField] private Transform container;
        [SerializeField] private Vector2 offset;
        [SerializeField] private float step;

        [Space] [SerializeField] private UILineRenderer rendererLine;
        [SerializeField] private Transform lineContainer;

        [Inject] private readonly TimelineData _data;
        [Inject] private readonly SceneController _sceneController;
        [Inject] private readonly LoadProgressTimeline _loadProgressTimeline;
        [Inject] private readonly DiContainer _container;

        private readonly CompositeDisposable _disposables = new();

        public readonly List<CollapseView> Views = new();

        private void Awake()
        {
            SpawnCollapse();
        }

        private void Start()
        {
            SpawnLine().Forget();
        }

        private void SpawnCollapse()
        {
            int i = 0;
            foreach (var age in _data.Ages)
            {
                var c = Instantiate(containerView, container);
                c.transform.position = offset + (i++) * Vector2.right * step;
                foreach (var collapse in age.Ages)
                {
                    var v = c.Spawn(_container, collapse, _sceneController,
                        e => _loadProgressTimeline.SetID(e), _disposables);
                    if (v is not null)
                        Views.Add(v);
                }
            }
        }

        private async UniTaskVoid SpawnLine()
        {
            await UniTask.DelayFrame(1);

            foreach (var timeline in _data.Timelines)
            {
                var l = Instantiate(rendererLine, rendererLine.transform);
                var p = new Vector2[timeline.Timeline.Length];
                int i = 0;
                foreach (var collapse in timeline.Timeline)
                {
                    var v = Views.Find(e => e.Content == collapse).transform;
                    Vector2 canvasStart = RectTransformUtility.WorldToScreenPoint(Camera.main, v.position);
                    p[i++] = canvasStart;
                    if (collapse.Branches != null && collapse.Branches.Length > 0)
                    {
                        var l2 = Instantiate(rendererLine, rendererLine.transform);
                        var p2 = new Vector2[collapse.Branches.Length + 1];
                        p2[0] = canvasStart;
                        int i2 = 1;
                        foreach (var branch in collapse.Branches)
                        {
                            var v2 = Views.Find(e => e.Content == branch).transform;
                            Vector2 canvasStart2 = RectTransformUtility.WorldToScreenPoint(Camera.main, v2.position);
                            p2[i2++] = canvasStart2;
                        }

                        l2.points = p2;
                        l2.transform.SetParent(lineContainer);
                    }
                }

                l.points = p;
                l.transform.SetParent(lineContainer);
            }
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
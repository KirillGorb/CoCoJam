using System.Collections.Generic;
using CodeScripts.Timeline.Model;
using CodeScripts.Timeline.View;
using Cysharp.Threading.Tasks;
using Plugins.Other;
using UniRx;
using UnityEngine;

namespace CodeScripts.Timeline
{
    public class GenerateTimeline : MonoBehaviour
    {
        [SerializeField] private TimelineModel[] timelines;
        [SerializeField] private AgesModel[] ages;

        [Space] [SerializeField] private CollapseContainerView containerView;
        [SerializeField] private Transform container;
        [SerializeField] private Vector2 offset;
        [SerializeField] private float step;

        [Space] [SerializeField] private UILineRenderer rendererLine;
        [SerializeField] private Transform lineContainer;

        private readonly List<CollapseView> _views = new();
        private readonly CompositeDisposable _disposables = new();

        private void Awake()
        {
            int i = 0;
            foreach (var age in ages)
            {
                var c = Instantiate(containerView, container);
                c.transform.position = offset + (i++) * Vector2.right * step;
                foreach (var collapse in age.Ages)
                {
                    var v = c.Spawn(collapse, _disposables);
                    if (v is not null)
                        _views.Add(v);
                }
            }
        }

        private async void Start()
        {
            await UniTask.DelayFrame(1);

            foreach (var timeline in timelines)
            {
                var l = Instantiate(rendererLine, rendererLine.transform);
                var p = new Vector2[timeline.Timeline.Count];
                int i = 0;
                foreach (var collapse in timeline.Timeline)
                {
                    var v = _views.Find(e => e.Content == collapse).transform;
                    Vector2 canvasStart = RectTransformUtility.WorldToScreenPoint(Camera.main, v.position);
                    p[i++] = canvasStart;
                    if (collapse.Branches != null && collapse.Branches.Count > 0)
                    {
                        var l2 = Instantiate(rendererLine, rendererLine.transform);
                        var p2 = new Vector2[collapse.Branches.Count+1];
                        p2[0] = canvasStart;
                        int i2 = 1;
                        foreach (var branch in collapse.Branches)
                        {
                            var v2 = _views.Find(e => e.Content == branch).transform;
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
using System;
using System.Linq;
using CodeScripts.Abstraction;
using CodeScripts.Timeline.Model;
using CodeScripts.Timeline.View;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeScripts.Timeline
{
    public class MapItemView : MonoBehaviour, IData
    {
        [SerializeField] private Image view;
        [SerializeField] private Button button;

        [Inject] private readonly ViewCollapseData _data;
        [Inject] private readonly TimelineData _timeline;

        private CollapseModel _model;

        [field: SerializeField] public CollapseModel[] Collapses { get; private set; }

        public IObservable<Unit> Open => button.OnClickAsObservable();

        public event Func<int> AgeFind;

        public void RenderAge()
        {
            if (InAge(out var c))
                SetActiveView(true, c.Data.Value.Mode);
            else
                SetActiveView(false, ECollapseMode.Inactive);
        }

        public bool InAge(out CollapseModel model)
        {
            model = null;

            if (FindAgesModel().Ages.Contains(_model))
            {
                model = _model;
                return true;
            }

            foreach (var collapse in Collapses)
                if (FindAgesModel().Ages.Contains(collapse))
                {
                    model = _model = collapse;
                    return true;
                }

            return false;
        }

        private void SetActiveView(bool inAge, ECollapseMode mode)
        {
            view.color = _data.Search(mode);
            if (!inAge)
                view.color = _data.Search(ECollapseMode.Inactive);
        }

        private AgesModel FindAgesModel() => _timeline.Ages[AgeFind?.Invoke() ?? 0];
    }
}
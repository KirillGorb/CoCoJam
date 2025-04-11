using System;
using CodeScripts.Abstraction;
using CodeScripts.Timeline.Model;
using CodeScripts.UI.TooltipSystem;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CodeScripts.Timeline.View
{
    public class CollapseView : GuidanceObject, IData
    {
        [SerializeField] private Button open;
        [SerializeField] private Image view;
        [SerializeField] private CollapseData data;

        public CollapseModel Content { get; set; }
        public IObservable<Unit> Open => open.OnClickAsObservable();
        public override IData Data => this;

        public void SetView(ECollapseMode mode) => view.color = data.Search(mode);
    }
}
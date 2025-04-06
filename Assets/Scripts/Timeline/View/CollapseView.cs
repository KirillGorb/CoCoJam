using System;
using CodeScripts.Timeline.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CodeScripts.Timeline.View
{
    public class CollapseView : MonoBehaviour
    {
        [SerializeField] private Button open;
        [SerializeField] private Image view;
        [SerializeField] private CollapseData data;

        public CollapseModel Content { get; set; }

        public IObservable<Unit> Open => open.OnClickAsObservable();

        public void SetView(ECollapseMode mode) => view.color = data.Search(mode);
    }
}
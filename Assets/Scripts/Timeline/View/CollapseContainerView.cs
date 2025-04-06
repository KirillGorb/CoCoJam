using System.Collections.Generic;
using CodeScripts.Scene;
using CodeScripts.Timeline.Model;
using UniRx;
using UnityEngine;

namespace CodeScripts.Timeline.View
{
    public class CollapseContainerView : MonoBehaviour
    {
        [SerializeField] private CollapseView collapse;

        private readonly List<CollapseModel> _repeats = new();

        public CollapseView Spawn(CollapseModel model, CompositeDisposable disposable)
        {
            if (_repeats.Contains(model)) 
                return null;
            _repeats.Add(model);

            var c = Instantiate(collapse, transform);
            c.Content = model;
            c.SetView(model.Mode);
            c.Open.Subscribe(_ => { SceneController.SetScene(model.ScenePlay); }).AddTo(disposable);
            return c;
        }
    }
}
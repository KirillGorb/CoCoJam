using System;
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

        public CollapseView Spawn(CollapseModel model, SceneController scene, Action<CollapseModel> action,
            CompositeDisposable disposable)
        {
            if (_repeats.Contains(model))
                return null;
            _repeats.Add(model);

            var c = Instantiate(collapse, transform);
            c.Content = model;
            model.Mode.Subscribe(e => c.SetView(e)).AddTo(disposable);
            c.Open
                .Where(_ => model.Mode.Value is ECollapseMode.Active or ECollapseMode.Rollback)
                .Subscribe(_ =>
                {
                    action(model);
                    scene.SetScene(model.ScenePlay);
                }).AddTo(disposable);

            return c;
        }
    }
}
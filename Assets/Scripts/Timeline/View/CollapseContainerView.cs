using System;
using System.Collections.Generic;
using CodeScripts.Scene;
using CodeScripts.Timeline.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeScripts.Timeline.View
{
    public class CollapseContainerView : MonoBehaviour
    {
        [SerializeField] private CollapseView collapse;

        private readonly List<CollapseModel> _repeats = new();

        public CollapseView Spawn(DiContainer container, CollapseModel model, SceneController scene,
            Action<CollapseModel> action,
            CompositeDisposable disposable)
        {
            if (_repeats.Contains(model))
                return null;
            _repeats.Add(model);

            var c = container.InstantiatePrefab(collapse, transform).GetComponent<CollapseView>();
            c.Content = model;
            model.Data.Subscribe(e => c.SetView(e.Mode)).AddTo(disposable);
            c.Open
                .Where(_ => model.Data.Value is
                    { IsActivate: true, Mode: ECollapseMode.Active or ECollapseMode.Rollback })
                .Subscribe(_ =>
                {
                    action(model);
                    scene.SetScene(model.ScenePlay);
                }).AddTo(disposable);

            return c;
        }
    }
}
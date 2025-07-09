using System;
using CodeScripts.PlayerInputs;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeScripts.Skill
{
    public class ClickService : IInitializable, IDisposable
    {
        public ReactiveCommand<(Collider2D, Vector2)> RaycastClick { get; } = new();
        public ReactiveCommand AirClick { get; } = new();

        private readonly CompositeDisposable _disposable = new();

        public void Initialize() => InputCallback.Skill.Subscribe(Detect).AddTo(_disposable);

        private void Detect(Vector2 posMouse)
        {
            posMouse = Camera.main.ScreenToWorldPoint(posMouse);
            var hit = Physics2D.OverlapCircle(posMouse, 0.01f);
            if (hit != null)
                RaycastClick.Execute((hit, posMouse));
            AirClick.Execute();
        }

        public void Dispose() => _disposable?.Dispose();
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerInteraction
{
    public static class Dispose
    {
        public static void AddTo(this IDisposable disposables, DisposableCollection disposable) =>
            disposable.Add(disposables);
    }

    public class DisposableCollection : IDisposable
    {
        private List<IDisposable> _disposables = new();

        public void Add(IDisposable disposable) => _disposables.Add(disposable);

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }

    public class ReactiveCollision<T> : IDisposable
    {
        private Action<T> _observer;
        private Action<T> _endObserver;

        private Action _detectUse;
        private Func<T, bool> _isObserver;

        public ReactiveCollision<T> Where(Func<T, bool> isObserver)
        {
            _isObserver += isObserver;
            return this;
        }

        public ReactiveCollision<T> Subscribe(Action<T> observer = default, Action<T> endObserver = default,
            Action use = default)
        {
            _observer += observer;
            _detectUse += use;
            _endObserver += endObserver;

            return this;
        }

        public void ExecuteAdd(T data)
        {
            if (!_isObserver?.Invoke(data) ?? false) return;
            _observer?.Invoke(data);
            _detectUse?.Invoke();
        }

        public void ExecuteRemove(T data)
        {
            if (!_isObserver?.Invoke(data) ?? false) return;
            _endObserver?.Invoke(data);
            _detectUse?.Invoke();
        }

        public void Dispose()
        {
            _observer = null;
            _detectUse = null;
            _endObserver = null;
        }
    }

    public class PlayerCollisionDetector : MonoBehaviour
    {
        [Inject(Id = "Scene")] private readonly DisposableCollection _disposables = new();

        public ReactiveCollision<Collider2D> IncomingColliders = new();
        public ReactiveCollision<Collision2D> IncomingCollisions = new();

        private void OnCollisionEnter2D(Collision2D collision) => IncomingCollisions?.ExecuteAdd(collision);
        private void OnCollisionExit2D(Collision2D collision) => IncomingCollisions?.ExecuteRemove(collision);

        private void OnTriggerEnter2D(Collider2D collision) => IncomingColliders?.ExecuteAdd(collision);
        private void OnTriggerExit2D(Collider2D collision) => IncomingColliders?.ExecuteRemove(collision);

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
    }
}
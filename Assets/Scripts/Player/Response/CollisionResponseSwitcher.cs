using CodeScripts.PlayerInteraction;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using System;
using System.Collections.Generic;
using UnityEngine;
using CodeScripts.Abstraction;
using UniRx;

namespace CodeScripts.PlayerResponse
{
    public abstract class CollisionResponseSwitcher<T> : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly Dictionary<T, List<IPlayerResponseService>> _responses = new();

        public CollisionResponseSwitcher(PlayerCollisionDetector playerCollisionDetector, DataSwitcher dataSwitcher)
        {
            playerCollisionDetector.IncomingColliders
                .ObserveAdd()
                .Subscribe(e =>
                {
                    var v = KeyCollider(e.Value);
                    if (v != null)
                        if (dataSwitcher.Container.TryGetValue(v, out var data))
                            Use(v, data);
                        else
                            Use<IData>(v, null);
                }).AddTo(_disposables);

            playerCollisionDetector.IncomingColliders
                .ObserveRemove()
                .Subscribe(e =>
                {
                    var v = KeyCollider(e.Value);
                    if (v != null)
                        if (dataSwitcher.Container.TryGetValue(v, out var data))
                            StopUse(v, data);
                        else
                            StopUse<IData>(v, null);
                }).AddTo(_disposables);


            playerCollisionDetector.IncomingCollisions
                .ObserveAdd()
                .Subscribe(e =>
                {
                    var v = KeyCollision(e.Value);
                    if (v != null)
                        if (dataSwitcher.Container.TryGetValue(v, out var data))
                            Use(v, data);
                        else
                            Use<IData>(v, null);
                }).AddTo(_disposables);

            playerCollisionDetector.IncomingCollisions
                .ObserveRemove()
                .Subscribe(e =>
                {
                    var v = KeyCollision(e.Value);
                    if (v != null)
                        if (dataSwitcher.Container.TryGetValue(v, out var data))
                            StopUse(v, data);
                        else
                            StopUse<IData>(v, null);
                }).AddTo(_disposables);
        }

        public abstract T KeyCollider(Collider2D content);
        public abstract T KeyCollision(Collision2D content);

        public void AddResponse(T key, IPlayerResponseService responseService)
        {
            if (!_responses.ContainsKey(key))
                _responses.Add(key, new());

            _responses[key].Add(responseService);
        }

        public void RemoveResponse(T key, IPlayerResponseService responseService)
        {
            if (!_responses.ContainsKey(key))
                return;

            foreach (var service in _responses[key])
                if (responseService.GetType() == service.GetType())
                {
                    _responses[key].Remove(service);
                    return;
                }
        }

        private void Use<Q>(T key, Q data) where Q : IData
        {
            if (_responses.TryGetValue(key, out var responses))
                foreach (var response in responses)
                    response.Response(data);
        }

        private void StopUse<Q>(T key, Q data) where Q : IData
        {
            if (_responses.TryGetValue(key, out var responses))
                foreach (var response in responses)
                    response.StopResponse(data);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
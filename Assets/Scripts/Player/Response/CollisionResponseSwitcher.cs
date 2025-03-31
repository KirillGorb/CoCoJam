using CodeScripts.PlayerInteraction;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using System;
using System.Collections.Generic;
using UnityEngine;
using CodeScripts.Abstraction;
using Zenject;

namespace CodeScripts.PlayerResponse
{
    public sealed class DataSwitcher
    {
        public readonly Dictionary<object, IData> Container = new();
    }

    public sealed class LayerResponseSwitcher : CollisionResponseSwitcher<LayerMask>
    {
        [Inject(Id = "Scene")] private readonly DisposableCollection _disposables = new();

        public LayerResponseSwitcher(PlayerCollisionDetector playerCollisionDetector, DataSwitcher dataSwitcher)
        {
            playerCollisionDetector.IncomingColliders
                .Where(e => dataSwitcher.Container.ContainsKey(e.gameObject.layer))
                .Subscribe(
                    e => Use(e.gameObject.layer, dataSwitcher.Container[e.gameObject.layer]),
                    endE => StopUse(endE.gameObject.layer,
                        dataSwitcher.Container.ContainsKey(endE.gameObject.layer)
                            ? dataSwitcher.Container[endE.gameObject.layer]
                            : null))
                .AddTo(_disposables);
        }
    }

    public abstract class CollisionResponseSwitcher<T>
    {
        private readonly Dictionary<T, List<IPlayerResponseService>> _responses = new();

        protected void Use<TQ>(T layer, TQ data) where TQ : IData
        {
            if (!_responses.ContainsKey(layer))
                return;

            foreach (var response in _responses[layer])
                response.Response(data);
        }

        protected void StopUse<TQ>(T layer, TQ data) where TQ : IData
        {
            if (!_responses.ContainsKey(layer))
                return;

            foreach (var response in _responses[layer])
                response.StopResponse(data);
        }

        public void AddResponse(T layer, IPlayerResponseService responseService)
        {
            if (!_responses.ContainsKey(layer))
                _responses.Add(layer, new());

            _responses[layer].Add(responseService);
        }

        public void RemoveResponse(T layer, IPlayerResponseService responseService)
        {
            if (!_responses.ContainsKey(layer))
                throw new InvalidOperationException();

            foreach (var service in _responses[layer])
                if (responseService.GetType() == service.GetType())
                {
                    _responses[layer].Remove(service);
                    return;
                }

            throw new InvalidOperationException();
        }
    }
}
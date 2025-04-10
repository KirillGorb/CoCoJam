using System;
using System.Threading;
using CodeScripts.Abstraction;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using CodeScripts.PlayerResponse.Implementations.Conditions;
using CodeScripts.Timeline;
using Cysharp.Threading.Tasks;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeScripts.PlayerResponse.Implementations
{
    public class KeyService : IPlayerResponseService, IInitializable
    {
        [Inject] private readonly ComponentResponseSwitcher _componentResponseSwitcher;
        [Inject] private readonly LoadProgressTimeline _timeline;

        public void Initialize()
        {
            _componentResponseSwitcher.AddResponse(typeof(KeyObject), this);
        }

        public UniTask Response<T>(T data, CancellationToken token = default) where T : IData
        {
            if (data is not DataKey dk) return UniTask.CompletedTask;

            _timeline.SetActive(dk.IdKey, true);
            Object.Destroy(dk.KeyGo);

            return UniTask.CompletedTask;
        }

        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData
        {
            return UniTask.CompletedTask;
        }
    }
}
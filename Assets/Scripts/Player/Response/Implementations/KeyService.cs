using System.Threading;
using CodeScripts.Abstraction;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using CodeScripts.PlayerResponse.Implementations.Conditions;
using CodeScripts.PlayerResponse.Player.Response.Implementations.Conditions;
using CodeScripts.Timeline;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
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

            Object.FindObjectsOfType<KeyObject>().ForEach(e =>  e.gameObject.SetActive(_timeline.GetActive(e.Key)));
        }

        public UniTask Response<T>(T data, CancellationToken token = default) where T : IData
        {
            if (data is not ServiceInteraction s)
                return UniTask.CompletedTask;

            _timeline.SetActive(s.key.IdKey, true);
            Object.Destroy(s.key.KeyGo);

            return UniTask.CompletedTask;
        }

        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData
        {
            return UniTask.CompletedTask;
        }
    }
}
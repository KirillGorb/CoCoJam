using System.Threading;
using CodeScripts.Abstraction;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using CodeScripts.Scene;
using CodeScripts.Timeline;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations
{
    public class FinishLevelService : IPlayerResponseService, IInitializable
    {
        public const string Finish = nameof(Finish);

        [Inject] private TagResponseSwitcher _switcher;
        [Inject] private LoadProgressTimeline _timeline;
        [Inject] private SceneController _scene;

        public void Initialize()
        {
            _switcher.AddResponse(Finish,this);
        }

        public UniTask Response<T>(T data, CancellationToken token = default) where T : IData
        {
            _timeline.Next();
            _scene.SetScene(0);
            return UniTask.CompletedTask;
        }

        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData
        {
            return UniTask.CompletedTask;
        }
    }
}
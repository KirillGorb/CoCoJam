using System.Threading;
using CodeScripts.Abstraction;
using CodeScripts.PlayerInteraction;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using CodeScripts.Scene;
using CodeScripts.Timeline;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations
{
    public sealed class TagResponseSwitcher : CollisionResponseSwitcher<string>
    {
        public TagResponseSwitcher(PlayerCollisionDetector playerCollisionDetector, DataSwitcher dataSwitcher) : base(
            playerCollisionDetector, dataSwitcher)
        {
        }

        public override string KeyCollider(Collider2D content) => content.gameObject.tag;
        public override string KeyCollision(Collision2D content) => content.gameObject.tag;
    }

    public class FinishLevelService : IPlayerResponseService, IInitializable
    {
        public const string Finish = nameof(Finish);

        [Inject] private TagResponseSwitcher _switcher;
        [Inject] private LoadProgressTimeline _timeline;
        [Inject] private SceneController _scene;

        public void Initialize()
        {
            _switcher.AddResponse(Finish, this);
        }

        public UniTask Response<T>(T data, CancellationToken token = default) where T : IData
        {
            Debug.Log(Finish);
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
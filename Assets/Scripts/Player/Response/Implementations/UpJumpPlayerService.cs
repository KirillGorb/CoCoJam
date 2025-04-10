using CodeScripts.Abstraction;
using CodeScripts.PlayerMove.State.Logics;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations
{
    public class UpJumpPlayerService : IPlayerResponseService, IInitializable
    {
        [Inject] private MoveY _moveY;
        [Inject] private LayerResponseSwitcher _switcher;

        public void Initialize()
        {
            _switcher.AddResponse(12, this);
        }

        public UniTask Response<T>(T data, CancellationToken token = default) where T : IData
        {
            if (data is not DataUpJump upJump) return UniTask.CompletedTask;

            _moveY._timeJump = upJump.JumpTime;
            Debug.Log("UpJump");

            return UniTask.CompletedTask;
        }

        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData
        {
            Debug.Log("UpJumpEnd");
            _moveY._timeJump = _moveY._config.timeJump;

            return UniTask.CompletedTask;
        }
    }
}
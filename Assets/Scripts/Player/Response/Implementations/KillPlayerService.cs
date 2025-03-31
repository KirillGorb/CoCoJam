using System;
using System.Threading;
using CodeScripts.Abstraction;
using CodeScripts.PlayerMove;
using CodeScripts.PlayerMove.State.Datas;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using CodeScripts.Respawn;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations
{
  /*  public class UpJumpPlayerService : IPlayerResponseService
    {
        //  [Inject] private PlayerMoveController _playerMoveController;

        public UniTask Response<T>(T data) where T : IData
        {
            if (data is not DataUpJump upJump) return UniTask.CompletedTask;

            return UniTask.CompletedTask;
        }
    }

    public class DataUpJump : IData, IInitializable
    {
        public float JumpHeight;
        public float JumpTime;

        public void Initialize()
        {
        }
    }*/

    public class KillPlayerService : IPlayerResponseService, IInitializable
    {
        [Inject] private RespawnController _respawnController;
        [Inject] private LayerResponseSwitcher _switcher;
        [Inject] private PlayerMoveController _playerMoveController;

        public void Initialize()
        {
            _switcher.AddResponse(13, this);
        }

        public async UniTask Response<T>(T data, CancellationToken token = default) where T : IData
        {
            if (data is not DataKill killer) return;

            _playerMoveController.SetState(new NoMoveData());
            await UniTask.Delay(TimeSpan.FromSeconds(killer.KillTime));
            _respawnController.Respawn();
            _playerMoveController.SetBaseState();
        }

        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData
        {
            return UniTask.CompletedTask;
        }
    }

    public class DataKill : IData, IInitializable
    {
        public float KillTime = 0.5f;

        [Inject] private DataSwitcher _dataSwitcher;

        public void Initialize()
        {
            _dataSwitcher.Container.Add(13, this);
        }
    }
}
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
    public class KillPlayerService : IPlayerResponseService, IInitializable
    {
        [Inject] private RespawnController _respawnController;
        [Inject] private LayerResponseSwitcher _switcher;
        [Inject] private PlayerMoveController _playerMoveController;

        private CancellationTokenSource _cancellationTokenSource;

        public void Initialize()
        {
            _switcher.AddResponse(13, this);
        }

        public async UniTask Response<T>(T data, CancellationToken token = default) where T : IData
        {
            if (data is not DataKill killer) return;

            _cancellationTokenSource = new();
            _playerMoveController.SetState(new NoMoveData());
            await UniTask.Delay(TimeSpan.FromSeconds(killer.KillTime),
                cancellationToken: _cancellationTokenSource.Token);
            _respawnController.Respawn();
            _playerMoveController.SetBaseState();
        }

        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData
        {
            _cancellationTokenSource.Cancel();
            return UniTask.CompletedTask;
        }
    }
}
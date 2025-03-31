using CodeScripts.Abstraction;
using CodeScripts.PlayerMove;
using CodeScripts.PlayerMove.State.Logics;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using Cysharp.Threading.Tasks;
using System;
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

            #region Dedug
            Debug.Log("UpJump");
            #endregion

            return UniTask.CompletedTask;
        }

        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData
        {
            #region Dedug
            Debug.Log("UpJumpEnd");
            #endregion

            _moveY._timeJump = _moveY._config.timeJump;

            return UniTask.CompletedTask;
        }
    }

    public class DataUpJump : IData, IInitializable
    {
        public TimeSpan TimeToEnd = TimeSpan.FromSeconds(3);

        public float JumpTime = 15;

        [Inject] private DataSwitcher _dataSwitcher;

        public void Initialize()
        {
            _dataSwitcher.Container.Add(12, this);
        }
    }
}
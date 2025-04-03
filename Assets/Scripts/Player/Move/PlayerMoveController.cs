using CodeScripts.PlayerMove.Config;
using CodeScripts.PlayerMove.State;
using CodeScripts.Abstraction;
using CodeScripts.PlayerMove.State.Datas;
using CodeScripts.PlayerMove.State.Logics;
using System;
using System.Collections.Generic;
using CodeScripts.PlayerInteraction;
using UniRx;
using UnityEngine;
using Zenject;


namespace CodeScripts.PlayerMove
{
    public class PlayerMoveController : IFixedTickable
    {
        private readonly Dictionary<Type, IState> _containerStates = new();
        private IState _state;

        [Inject] private ConfigMove _configMove;
        [Inject] private Rigidbody2D _rigidbody2D;
        private readonly CompositeDisposable _disposables = new();

        public void SetBaseState()
        {
            _state.Abort();
            _state = _containerStates[typeof(MoveData)];
        }

        public void SetState(IData newData)
        {
            if (_state == _containerStates[newData.GetType()])
                return;

            if (_state <= _containerStates[newData.GetType()])
            {
                _state.Abort();
                _state = _containerStates[newData.GetType()];
                _state.Overview(newData);
            }
        }

        [Inject]
        private void Initialize(MoveX moveX, MoveY moveY, HookConfig hook, Rigidbody2D rb,
            PlayerCollisionDetector playerCollisionDetector, InterectiveDetect interective, HookDetect detect)
        {
            _containerStates.Add(typeof(MoveData), _state = new NewBaseMove(moveX, moveY, _rigidbody2D, interective, playerCollisionDetector, _disposables, detect));

            _containerStates.Add(typeof(HookData), new HookMoveState(_rigidbody2D, hook, this));

            _containerStates.Add(typeof(MovePlatformData), new StateToMovablePlatform(this, _rigidbody2D, moveX, moveY));

            _containerStates.Add(typeof(NoMoveData), new NoMoveState(rb));
        }

        public void FixedTick() => _state.Call();
    }
}
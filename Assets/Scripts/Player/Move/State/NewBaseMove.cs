using CodeScripts.Abstraction;
using CodeScripts.PlayerInteraction;
using CodeScripts.PlayerMove.State.Logics;
using UniRx;
using UnityEngine;

namespace CodeScripts.PlayerMove.State
{
    public class NewBaseMove : IState
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly InterectiveDetect _interective;
        private readonly MoveY _moveY;
        private readonly MoveX _moveX;
        private readonly HookDetect _hook;

        private readonly AngleToCollider _angleToCollider = new();

        private Collision2D _collider;

        public NewBaseMove(MoveX moveX, MoveY moveY, Rigidbody2D rigidbody,
            InterectiveDetect interective, PlayerCollisionDetector playerCollisionDetector,
            CompositeDisposable disposables, HookDetect hook)
        {
            _hook = hook;
            _moveX = moveX;
            _moveY = moveY;
            _interective = interective;
            _rigidbody = rigidbody;

            playerCollisionDetector.IncomingCollisions
                .ObserveAdd().Subscribe(e => _collider = e.Value)
                .AddTo(disposables);
            playerCollisionDetector.IncomingCollisions
                .ObserveRemove().Subscribe(_ => _collider = null)
                .AddTo(disposables);
        }

        public override void Call()
        {
            //   if (_hook.DetectHook())
            //return;

            var y = _moveY.Value();
            if (_collider is not null && !_moveY.IsJump)
                _rigidbody.velocity = _angleToCollider.GetMoveVector(_collider, new Vector2(_moveX.Value(), _rigidbody.velocity.y));
            else
                _rigidbody.velocity = new Vector2(_moveX.Value(), y);

            _interective.Detect(_rigidbody);
        }

        public override void Overview(IData data)
        {
        }
    }
}
using System;
using System.Threading;
using CodeScripts.PlayerInputs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerInteraction
{
    public class ObjectDrag : IFixedTickable
    {
        [Inject] private Rigidbody2D _rd;
        [Inject] private DragDropConfig _config;

        private Rigidbody2D _target;
        private bool _isMove;
        private bool _interactionStarted;

        public async UniTaskVoid SetData(bool isMove, Rigidbody2D target, CancellationToken cancellationToken = default)
        {
            if (!isMove)
            {
                if (_target != null)
                {
                    _target.velocity = Vector2.zero;
                    _target.bodyType = RigidbodyType2D.Kinematic;
                    _target = null;
                }

                _isMove = false;
                _interactionStarted = false;
                return;
            }

            try
            {
                await UniTask.WaitUntil(() => InputCallback.PuckUpInput && !InputCallback.JumpInput, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                SetData(false, target).Forget();
                return;
            }

            if (_interactionStarted)
            {
                SetData(false, target).Forget();
                return;
            }

            _interactionStarted = true;
            _isMove = true;
            _target = target;
            _target.bodyType = RigidbodyType2D.Dynamic;
        }

        public void FixedTick()
        {
            if (!_isMove) return;

            if (InputCallback.JumpInput || !InputCallback.PuckUpInput)
            {
                SetData(false, _target).Forget();
                return;
            }

            var tPosX = _target.position;

            int lookDirection = _rd.position.x < tPosX.x ? 1 : -1;

            var targetPosition = new Vector2(_rd.position.x +_config.MaxDist* lookDirection, _target.position.y - _config.Gravity);

            _target.MovePosition(Vector2.MoveTowards(tPosX, targetPosition, Time.fixedDeltaTime * _config.Speed));
        }
    }
}
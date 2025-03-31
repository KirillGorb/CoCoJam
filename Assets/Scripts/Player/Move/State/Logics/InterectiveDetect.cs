using CodeScripts.PlayerInputs;
using CodeScripts.PlayerInteraction;
using System.Threading;
using CodeScripts.PlayerMove.Config;
using CodeScripts.PlayerMove.State.Logics;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerMove
{
    public class InterectiveDetect
    {
        private class Target
        {
            public Rigidbody2D Rigidbody { get; private set; }
            public Collider2D Collider { get; private set; }

            public Target(Rigidbody2D rigidbody, Collider2D collider)
            {
                Rigidbody = rigidbody;
                Collider = collider;
            }

            public static implicit operator bool(Target target) => target != null;
        }

        [Inject] private readonly InterectiveConfig _config;
        [Inject] private readonly ObjectDrag _objectDrag;
        [Inject] private readonly MoveX _configMove;


        private CancellationTokenSource _cts;
        private Target _lastTarget;

        private bool _isUsing = false;

        public bool Detect(Rigidbody2D player) => Use(GetPermission() && DetectInterectiveObject(player.position));

        private bool Use(bool isCanUse)
        {
            if (!isCanUse && !_isUsing)
                return false;

            if (isCanUse && _isUsing)
                return true;

            if (!isCanUse && _isUsing)
            {
                _cts?.Cancel();
            }
            else
                _cts = new CancellationTokenSource();

            if (_lastTarget)
                _objectDrag.SetData(isCanUse, _lastTarget.Rigidbody, _cts.Token).Forget();

            return _isUsing = isCanUse;
        }

        private bool GetPermission() => InputCallback.PuckUpInput;

        private bool DetectInterectiveObject(Vector3 playerPosition)
        {
            if (_isUsing)
                return CheckDistanceInterection(playerPosition);

            return BoxcastInterection(playerPosition, ref _lastTarget);
        }

        private bool BoxcastInterection(Vector3 playerPosition, ref Target target)
        {
            var hit = Physics2D.BoxCast(playerPosition + new Vector3(_config.OffSet.x, _config.OffSet.y, 0),
                _config.Size,
                -_config.Size.x,
                Vector2.right,
                0,
                _config.InteractiveLayers);

            if (!hit)
            {
                _configMove.SpeedSedMod.RemoveMod(_config.AddSpeedMove);
                return false;
            }

            _configMove.SpeedSedMod.AddMod(_config.AddSpeedMove);
            target = new Target(hit.rigidbody, hit.collider);

            return CheckDistanceInterection(playerPosition);
        }

        private bool CheckDistanceInterection(Vector3 playerPosition)
        {
            Vector3 posToCheck = playerPosition +
                                 new Vector3(InputCallback.HorizontalInput * _config.OffSet.x, _config.OffSet.y, 0);
            Vector3 positionPointInTarget = _lastTarget.Collider.bounds.ClosestPoint(posToCheck);

            Vector3 direction = (posToCheck - (Vector3)_lastTarget.Rigidbody.position).normalized;

            float angle = Vector3.Angle(Vector2.down, direction);

            if (angle > _config.MaxAngle || angle < _config.MinAngle)
                return false;

            float distance = Vector3.Distance(posToCheck, positionPointInTarget);

            float sin = Mathf.Sin(angle) * distance;
            float cos = Mathf.Cos(angle) * distance;

            if (sin > _config.Size.x / 2 || cos > _config.Size.y / 2)
                return false;

            return true;
        }
    }
}
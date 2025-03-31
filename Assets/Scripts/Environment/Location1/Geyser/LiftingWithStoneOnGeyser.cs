using CodeScripts.Respawn;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeScripts.Environment.Location1.Geyser
{
    public class LiftingWithStoneOnGeyser : MonoBehaviour
    {
        [SerializeField] private Transform pointFixStone;
        [SerializeField] private Transform pointMaxStone;

        [SerializeField] private float startVelocity = 2000;
        [SerializeField] private float groupDown = 1900;
        [SerializeField] private float speedFix = 2;
        [SerializeField] private float notValue = -1;

        private Rigidbody2D _target;
        private Animator _stoneAnim;

        private float _velocity;
        private bool _isActive;

        public void SetActiveGeyser(bool isActive)
        {
            _isActive = isActive;

            if (_isActive)
            {
                _velocity = startVelocity;
                MoveToFixPoint().Forget();
            }
            else
                _velocity = 0;
        }

        private void Start() => _velocity = startVelocity;

        private void MoveToNew()
        {
            if (_isActive)
            {
                if (pointMaxStone.position.y <= _target.position.y)
                {
                    _velocity = notValue;
                }
                else
                {
                    _velocity -= Time.fixedDeltaTime * groupDown;
                    _velocity = Mathf.Max(_velocity, -notValue);
                }

                _target.velocity = new(_target.velocity.x, _velocity);
            }
            else
            {
                if (pointFixStone.position.y <= _target.position.y)
                {
                    _velocity += Time.fixedDeltaTime * groupDown;
                    _velocity = Mathf.Min(_velocity, startVelocity);
                }
                else
                {
                    _velocity = notValue;
                }

                _target.velocity = new(_target.velocity.x, -_velocity);
            }
        }

        private void FixedUpdate()
        {
            if (_target == null) return;
            MoveToNew();
        }

        private async UniTaskVoid MoveToFixPoint()
        {
            while (_target != null && Vector2.Distance(pointFixStone.position, _target.position) > 0.1f)
            {
                _target.MovePosition(Vector2.MoveTowards(_target.position, pointFixStone.position,
                    speedFix * Time.fixedDeltaTime));

                await UniTask.NextFrame();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            /*if (other.TryGetComponent(out _player) && _isActive)
            {
                if (_target == null)
                    _player.ResetScene();
                else
                {
                    if (_player.transform.position.y < _target.position.y)
                        _player.ResetScene();
                }
            }*/
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PuckUp") && other.TryGetComponent(out Rigidbody2D target))
            {
                _target = target;
                MoveToFixPoint().Forget();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("PuckUp") && other.TryGetComponent(out Rigidbody2D _))
                _target = null;
        }
    }
}
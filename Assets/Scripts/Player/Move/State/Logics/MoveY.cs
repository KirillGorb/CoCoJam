using CodeScripts.PlayerInputs;
using CodeScripts.PlayerMove.Config;
using CodeScripts.PlayerMove.Config.Data;
using UnityEngine;
using Zenject;
using CodeScripts.PlayerInteraction;
using UniRx;

namespace CodeScripts.PlayerMove.State.Logics
{
    public class MoveY : IInitializable
    {
        private const double Tolerance = 0.000001;

        [Inject] private readonly Rigidbody2D _rigidbody2D;
        [Inject] private readonly ConfigMove _configMove;

        [Inject] private readonly PlayerCollisionDetector _playerCollisionDetector;
        private readonly CompositeDisposable _disposables = new();

        public ConfigMoveY _config;

        private int _countDopJump;

        private float _velocity;
        public float _timeJump;

        private bool _dirJumpClick;
        private bool _isClickDown;

        private bool _isJump;
        private bool _isActivePoolClick;

        private float _value;

        private float _poolClickJump;
        private float _lastGroundExitTime;
        private float _timeOnDownMove;

        private bool _isGrounded;
        private bool _isGround;
        public bool IsOnSlope { get; private set; }
        public bool IsJump => _isJump || (InputCallback.JumpInput || _isActivePoolClick) && !InputCallback.SedInput;

        public float Value()
        {
            JumpMove();
            return _value;
        }

        public void Initialize()
        {
            _config = _configMove.MoveY;
            _playerCollisionDetector.IncomingCollisions
                .ObserveAdd().Subscribe(_ => _isGrounded = true)
                .AddTo(_disposables);
            _playerCollisionDetector.IncomingCollisions
                .ObserveRemove().Subscribe(_ => _isGrounded = false)
                .AddTo(_disposables);
        }

        private void HandleCollision()
        {
            var groundCheckOriginPosition = _rigidbody2D.position + _config.groundCheckOffset;
            IsOnSlope = _isGrounded = Physics2D.OverlapCircle(groundCheckOriginPosition, _config.groundCheckRadius,
                _config.groundLayerMask);

            if (!_isGrounded)
                return;

            var onSlopeRight = Physics2D.Raycast(groundCheckOriginPosition + Vector2.right * _config.slopeCheckOffset,
                Vector2.down, Mathf.Infinity, _config.groundLayerMask);

            var onSlopeLeft = Physics2D.Raycast(groundCheckOriginPosition + Vector2.left * _config.slopeCheckOffset,
                Vector2.down, Mathf.Infinity, _config.groundLayerMask);

            IsOnSlope = Mathf.Abs(onSlopeLeft.distance - onSlopeRight.distance) > Tolerance && _isGrounded &&
                        _rigidbody2D.velocity.y <= 0;
        }

        private void JumpMove()
        {
            HandleCollision();
            var input = InputCallback.JumpInput;
            var isCheckGroundDown = CheckGround;

            PoolClick(input);
            //DopJump(input);

            if (_isJump)
            {
                if (_timeJump >= 0)
                {
                    _timeJump -= Time.fixedDeltaTime * _config.velocityJump;
                    _value = _timeJump;
                    return;
                }

                _isJump = false;
            }

            if (IsJump && isCheckGroundDown)
            {
                DataJump();
                _rigidbody2D.gravityScale = 0;
                return;
            }


            if (isCheckGroundDown)
                _countDopJump = _config.countDopJump;

            if (isCheckGroundDown && InputCallback.HorizontalInput == 0)
            {
                if (_timeOnDownMove <= 0)
                {
                    _timeOnDownMove = 0;
                    _rigidbody2D.gravityScale = 0;
                    _value = 0;
                    return;
                }

                _timeOnDownMove -= Time.fixedDeltaTime;
            }
            else
            {
                _rigidbody2D.gravityScale = _config.gravityScale;
                _value = _rigidbody2D.velocity.y;
            }

            if (IsLedgeAhead())
                _velocity = _config.ledgeHeight;
        }

        private void DataJump()
        {
            _timeOnDownMove = _config.timeOnDownMove;
            _countDopJump--;
            _isJump = true;
            _isActivePoolClick = false;
            _timeJump = _config.timeJump;
        }

        private bool IsLedgeAhead()
        {
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody2D.position + Vector2.up * _config.upOffsetHeight,
                Vector2.right * InputCallback.HorizontalInput, _config.ledgeCheckDistance, _config.groundLayerMask);
            return hit.collider != null && hit.point.y <= _rigidbody2D.position.y + _config.ledgeHeight;
        }

        private void DopJump(bool jumpClick)
        {
            if (jumpClick && _dirJumpClick)
                _isClickDown = true;
            _dirJumpClick = !jumpClick;

            if (_isClickDown && _countDopJump > 0)
                DataJump();
            _isClickDown = false;
        }

        private bool CheckGround
        {
            get
            {
                _isGround = _isGrounded;

                if (!_isGround)
                {
                    if (Time.time - _lastGroundExitTime < _config.groundExitCooldown)
                        _isGround = true;
                }
                else
                    _lastGroundExitTime = Time.time;

                return _isGround;
            }
        }


        private void PoolClick(bool input)
        {
            if (input)
            {
                _poolClickJump = _config.poolTimeClick;
                _isActivePoolClick = true;
            }

            if (_poolClickJump >= 0 && _isActivePoolClick)
                _poolClickJump -= Time.fixedDeltaTime;
            else
                _isActivePoolClick = false;
        }
    }
}
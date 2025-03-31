using CodeScripts.Abstraction;
using CodeScripts.PlayerMove.State.Datas;
using CodeScripts.PlayerMove.Config;
using UnityEngine;

namespace CodeScripts.PlayerMove.State
{
    public class HookMoveState : IState
    {
        private HookData _data;
        private float _elapsedTime = 0f;

        private readonly Rigidbody2D _rigidbody2D;
        private readonly HookConfig _config;
        private readonly PlayerMoveController _controller;

        public HookMoveState(Rigidbody2D rigidbody2D, HookConfig config, PlayerMoveController controller) : base(2)
        {
            _rigidbody2D = rigidbody2D;
            _config = config;
            _controller = controller;
        }

        public override void Overview(IData data)
        {
            if (data is HookData datas)
                _data = datas;

            _elapsedTime = 0;
        }

        public override void Call()
        {
            if (_elapsedTime < _config.Duration)
            {
                _elapsedTime += Time.deltaTime * _config.Speed;
                float t = _elapsedTime / _config.Duration;

                Vector2 newPosition = CalculateBezierPoint(t, _data.StartPoint, _data.ControlPoint, _data.MoveToPoint);
                _rigidbody2D.MovePosition(newPosition);
            }
            else
            {
                _controller.SetBaseState();
            }
        }

        private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector2 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;

            return p;
        }
    }
}
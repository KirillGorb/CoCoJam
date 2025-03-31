using CodeScripts.PlayerInputs;
using CodeScripts.PlayerMove.State.Datas;
using CodeScripts.PlayerMove.Config;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerMove.State.Logics
{
    public class HookDetect
    {
        [Inject] private readonly Rigidbody2D _rigidbody2D;
        [Inject] private readonly PlayerMoveController _controller;
        [Inject] private readonly HookConfig _config;

        public bool DetectHook()
        {
            var pointStart = Physics2D.Raycast(_rigidbody2D.position + _config.MaxPosCheck,
                                Vector2.right * InputCallback.DirHorizontalInput,
                                _config.DistCheck,
                                _config.LayerCheck);
            if (!pointStart)
                return false;

            var pointUp = Physics2D.Raycast(_rigidbody2D.position + _config.MaxPosCheck + Vector2.right * InputCallback.DirHorizontalInput,
                                Vector2.up,
                                _config.MaxDistUpCheck,
                                _config.LayerCheck);
            if (!pointUp)
                return false;

            var pointControll = new Vector2(_rigidbody2D.position.x, pointUp.point.y)
                                + new Vector2(InputCallback.DirHorizontalInput * _config.ControlPointOffset.x, _config.ControlPointOffset.y);

            _controller.SetState(new HookData() { MoveToPoint = pointUp.point, ControlPoint = pointControll, StartPoint = _rigidbody2D.position });
            return true;
        }
    }
}
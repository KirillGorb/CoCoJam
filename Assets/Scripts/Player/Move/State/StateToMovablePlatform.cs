using CodeScripts.PlayerInputs;
using CodeScripts.Abstraction;
using CodeScripts.PlayerMove.State.Datas;
using CodeScripts.PlayerMove.State.Logics;
using UnityEngine;

namespace CodeScripts.PlayerMove.State
{
    public class StateToMovablePlatform : IState
    {
        private readonly Rigidbody2D _target;
        private readonly MoveX _moveX;
        private readonly MoveY _moveY;
        private readonly PlayerMoveController _controller;

        private MovePlatformData _data;

        private float _valueDown;
        private Collision2D _collider;


        public StateToMovablePlatform(
            PlayerMoveController controller,
            Rigidbody2D target,
            MoveX moveX,
            MoveY moveY) : base(1)
        {
            _controller = controller;
            _target = target;
            _moveX = moveX;
            _moveY = moveY;
        }

        public override void Overview(IData data)
        {
            _data = (MovePlatformData)data;
        }

        public override void Call()
        {
            if (InputCallback.JumpInput)
            {
                _controller.SetBaseState();
                return;
            }

            _moveY.Value();
           if (InputCallback.HorizontalInput != 0)
                _target.AddForce(Vector3.up*.1f, ForceMode2D.Impulse);
            
            _data.Offset += _moveX.Value() * Time.fixedDeltaTime;
            _target.transform.position = new Vector2
            (
                _data.Offset + _data.Platform.position.x,
                _target.position.y
            );
        }
    }
}
using CodeScripts.Abstraction;
using UnityEngine;

namespace CodeScripts.PlayerMove.State
{
    public class NoMoveState : IState
    {
        private readonly Rigidbody2D _rigidbody;

        public NoMoveState(Rigidbody2D rb, int priority = 0) : base(priority) => _rigidbody = rb;

        public override void Abort()
        {
            _rigidbody.isKinematic = false;
        }

        public override void Call()
        {
        }

        public override void Overview(IData data)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.isKinematic = true;
        }
    }
}
using CodeScripts.PlayerMove;
using CodeScripts.PlayerMove.State.Datas;
using UnityEngine;
using Zenject;

namespace Envir.Platform
{
    public class PuckTarget : IState
    {
        [SerializeField] private bool isDrop;
        [SerializeField] private ETargetType upType = ETargetType.Player;

        private IDropPuckLogic _dropPuckLogic;

        [Inject]
        public void Constructor(DiContainer container) => _dropPuckLogic = container.ResolveId<IDropPuckLogic>(upType);

        public override void Init()
        {
            IsNext.Value = false;
            if (isDrop)
            {
                Target.transform.parent = null;
                _dropPuckLogic.Down();
            }
            else
            {
                Target.transform.parent = Transform.transform;
                _dropPuckLogic.Puck();
            }

            IsNext.Value = true;
        }

        public override void Abort()
        {
            Target.transform.parent = null;
            _dropPuckLogic.Down();
        }
    }

    public interface IDropPuckLogic
    {
        public void Puck();
        public void Down();
    }

    public class PlayerUpDown : IDropPuckLogic
    {
        [Inject] private PlayerMoveController _playerMove;
        [Inject] private Rigidbody2D _rigidbody2D;

        public void Puck() => _playerMove.SetState(new NoMoveData());

        public void Down() => _playerMove.SetBaseState();
    }
}
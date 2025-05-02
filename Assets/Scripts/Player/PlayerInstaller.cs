using CodeScripts.PlayerInputs;
using CodeScripts.PlayerInteraction;
using CodeScripts.PlayerMove;
using CodeScripts.PlayerMove.Config;
using CodeScripts.PlayerMove.State.Logics;
using CodeScripts.PlayerResponse;
using CodeScripts.PlayerResponse.Implementations;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using CodeScripts.PlayerResponse.Player.Response.Implementations.Conditions;
using CodeScripts.Respawn;
using UnityEngine;
using Zenject;

namespace CodeScripts.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private ConfigMove _moveConfig;
        [SerializeField] private HookConfig _hookConfig;
        [SerializeField] private InterectiveConfig _interectiveConfig;
        [SerializeField] private DragDropConfig _configDrag;

        [SerializeField] private Rigidbody2D _rd2D;

        [SerializeField] private RespawnController _respawnController;
        [SerializeField] private PlayerCollisionDetector _playerCollisionDetector;

        public override void InstallBindings()
        {
            BindData();
            BindInstances();
            BindServices();
        }

        private void BindInstances()
        {
            Container.BindInstance(_rd2D).AsSingle();
        }

        private void BindData()
        {
            Container.BindInstance(_moveConfig).AsSingle();
            Container.BindInstance(_hookConfig).AsSingle();
            Container.BindInstance(_interectiveConfig).AsSingle();
            Container.BindInstance(_configDrag).AsSingle();
        }

        private readonly AngleToCollider _angleToCollider = new();

        private Collision2D _collider;

        private void OnCollisionEnter2D(Collision2D other)
        {
            _collider = other;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            if (_collider is not null)
                Gizmos.DrawLine(transform.position, transform.position + (Vector3)_angleToCollider.GetMoveVector(_collider, new Vector2(_rd2D.velocity.x, _rd2D.velocity.y)));

            Gizmos.color = Color.red;
            Vector3 groundCheckOriginPosition = _rd2D.position + _moveConfig.MoveY.groundCheckOffset;
            Gizmos.DrawLine(groundCheckOriginPosition + Vector3.right * _moveConfig.MoveY.slopeCheckOffset, groundCheckOriginPosition + Vector3.right * _moveConfig.MoveY.slopeCheckOffset + Vector3.down);
            Gizmos.DrawLine(groundCheckOriginPosition + Vector3.left * _moveConfig.MoveY.slopeCheckOffset, groundCheckOriginPosition + Vector3.left * _moveConfig.MoveY.slopeCheckOffset + Vector3.down);
            Gizmos.DrawSphere(groundCheckOriginPosition, _moveConfig.MoveY.groundCheckRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_rd2D.position + Vector2.up * _moveConfig.MoveY.upOffsetHeight, _rd2D.position + Vector2.up * _moveConfig.MoveY.upOffsetHeight + Vector2.right * InputCallback.HorizontalInput * _moveConfig.MoveY.ledgeCheckDistance);
            Gizmos.DrawLine(groundCheckOriginPosition, groundCheckOriginPosition + Vector3.up * _moveConfig.MoveY.ledgeHeight);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(_rd2D.position +  _interectiveConfig.OffSet, _interectiveConfig.Size);
        }

        private void BindServices()
        {
            Container.BindInstance(_playerCollisionDetector).AsSingle();
            Container.BindInstance(_respawnController).AsSingle();

            Container.BindInterfacesAndSelfTo<MoveX>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveY>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<HookDetect>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<ObjectDrag>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<InterectiveDetect>().FromNew().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerMoveController>().FromNew().AsSingle();

            BindAndConfigureCollisionResponseSwitcherService();
        }

        private void BindAndConfigureCollisionResponseSwitcherService()
        {
            Container.Bind<DataSwitcher>().FromNew().AsSingle();
            Container.Bind<ServiceInteraction>().FromNew().AsSingle();
            Container.Bind<ComponentResponseSwitcher>().FromNew().AsSingle();
            Container.Bind<LayerResponseSwitcher>().FromNew().AsSingle();
            Container.Bind<TagResponseSwitcher>().FromNew().AsSingle();

            Container.BindInterfacesAndSelfTo<FinishLevelService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<KeyService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<DetectTaskService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<DialogActiveService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<KillPlayerService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<UpJumpPlayerService>().FromNew().AsSingle();

            Container.BindInterfacesAndSelfTo<DataKill>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<DataUpJump>().FromNew().AsSingle();
        }
    }
}
using CodeScripts.PlayerMove;
using CodeScripts.PlayerInputs;
using CodeScripts.PlayerMove.State.Datas;
using UnityEngine;
using Zenject;

namespace CodeScripts.Environment.Location1.Jelly
{
    public class JellyCheckPlayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rd2D;

        [Inject] private PlayerMoveController _move;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")
                && !InputCallback.JumpInput)
                _move.SetState(
                    new MovePlatformData
                    {
                        Offset = other.transform.position.x - transform.position.x,
                        Platform = rd2D,
                    });
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                _move.SetBaseState();
        }
    }

    /*  public class ComponentResponseSwitcher : CollisionResponseSwitcher<Component>
      {
          [Inject(Id = "Scene")] private readonly DisposableCollection _disposables = new();

          public ComponentResponseSwitcher(PlayerCollisionDetector playerCollisionDetector, DataSwitcher dataSwitcher)
          {
              playerCollisionDetector.IncomingColliders
                  .Subscribe(e =>
                  {
                      if (e.TryGetComponent<JellyCheckPlayer>(out var collider))
                          Use(collider, dataSwitcher.Container[collider]);
                  })
                  .AddTo(_disposables);
          }
      }*/
}
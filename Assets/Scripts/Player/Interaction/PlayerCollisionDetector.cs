using UniRx;
using UnityEngine;

namespace CodeScripts.PlayerInteraction
{
    public class PlayerCollisionDetector : MonoBehaviour
    {
        private readonly CompositeDisposable _disposables = new();

        public readonly ReactiveCollection<Collider2D> IncomingColliders = new();
        public readonly ReactiveCollection<Collision2D> IncomingCollisions = new();

        private void OnCollisionEnter2D(Collision2D collision) => IncomingCollisions?.Add(collision);
        private void OnCollisionExit2D(Collision2D collision) => IncomingCollisions?.Remove(collision);

        private void OnTriggerEnter2D(Collider2D collision) => IncomingColliders?.Add(collision);
        private void OnTriggerExit2D(Collider2D collision) => IncomingColliders?.Remove(collision);

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
    }
}
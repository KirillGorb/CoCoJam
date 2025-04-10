using CodeScripts.PlayerInteraction;
using UnityEngine;

namespace CodeScripts.PlayerResponse.Implementations
{
    public sealed class LayerResponseSwitcher : CollisionResponseSwitcher<LayerMask>
    {
        public LayerResponseSwitcher(PlayerCollisionDetector playerCollisionDetector, DataSwitcher dataSwitcher) : base(
            playerCollisionDetector, dataSwitcher)
        {
        }

        public override LayerMask KeyCollider(Collider2D content) => content.gameObject.layer;
        public override LayerMask KeyCollision(Collision2D content) => content.gameObject.layer;
    }
}
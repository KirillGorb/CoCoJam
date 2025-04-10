using CodeScripts.PlayerInteraction;
using UnityEngine;

namespace CodeScripts.PlayerResponse.Implementations
{
    public sealed class TagResponseSwitcher : CollisionResponseSwitcher<string>
    {
        public TagResponseSwitcher(PlayerCollisionDetector playerCollisionDetector, DataSwitcher dataSwitcher) : base(
            playerCollisionDetector, dataSwitcher)
        {
        }

        public override string KeyCollider(Collider2D content) => content.gameObject.tag;
        public override string KeyCollision(Collision2D content) => content.gameObject.tag;
    }
}
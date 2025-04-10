using System;
using CodeScripts.PlayerInteraction;
using CodeScripts.PlayerResponse.Implementations.Conditions;
using UnityEngine;

namespace CodeScripts.PlayerResponse.Implementations
{
    public sealed class ComponentResponseSwitcher : CollisionResponseSwitcher<Type>
    {
        public ComponentResponseSwitcher(PlayerCollisionDetector playerCollisionDetector, DataSwitcher dataSwitcher) :
            base(
                playerCollisionDetector, dataSwitcher)
        {
        }

        public override Type KeyCollider(Collider2D content)
        {
            if (content.gameObject.TryGetComponent(out InteractionCondition i)) return i.GetType();
            return null;
        }

        public override Type KeyCollision(Collision2D content)
        {
            if (content.gameObject.TryGetComponent(out InteractionCondition i)) return i.GetType();
            return null;
        }
    }
}
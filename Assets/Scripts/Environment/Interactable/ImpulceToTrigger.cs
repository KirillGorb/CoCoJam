using UnityEngine;

namespace CodeScripts.Interactable
{
    public class ImpulceToTrigger : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rd2D;
        [SerializeField] private Vector2 offset;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Rigidbody2D target))
                rd2D.velocity = offset * new Vector2(target.position.x > rd2D.position.x ? -1 : 1, target.velocity.y);
        }
    }
}
using UnityEngine;

namespace CodeScripts.Environment.Location1.FlyStone
{
    public class AnyMoveDown : MonoBehaviour
    {
        [SerializeField] private float valueDown;
        private Rigidbody2D rd2D;

        private void Awake() =>
            rd2D = GetComponent<Rigidbody2D>();

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.collider.GetComponent<Rigidbody2D>() != null)
                rd2D.velocity = Vector2.down * valueDown;
        }
    }
}
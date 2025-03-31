using CodeScripts.Respawn.RespawnPoint.Abstraction;
using UnityEngine;

namespace CodeScripts.Respawn.RespawnPoint
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerRespawnPoint : IRespawnPoint
    {
        public override Vector2 RespawnPosition { get => transform.position; protected set => transform.position = value; }

        [field: SerializeField] public override IRespawnPoint Next { get; protected set; }

        private void Awake() => GetComponent<Collider2D>().isTrigger = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out RespawnController controller))
                controller.NextRespawnPoint(this);
        }
    }
}
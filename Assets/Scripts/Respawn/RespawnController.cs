using CodeScripts.Respawn.RespawnPoint;
using CodeScripts.Respawn.RespawnPoint.Abstraction;
using UnityEngine;

namespace CodeScripts.Respawn
{
    public class RespawnController : MonoBehaviour 
    {
        [SerializeField] private TriggerRespawnPoint _startCollisionRespawnPoint;

        private IRespawnPoint _expectedPoint;
        private IRespawnPoint _currentPoint;

        private void Start()
        {
            SaveRespawnPoint(_startCollisionRespawnPoint);
        }

        public void NextRespawnPoint(IRespawnPoint next)
        {
            if (next != _expectedPoint)
                return;

            SaveRespawnPoint(next);
        }

        public void SaveRespawnPoint(IRespawnPoint respawnPoint)
        {
            _expectedPoint = respawnPoint.Next;
            _currentPoint = respawnPoint;           
        }

        public void Respawn() => transform.position = _currentPoint.RespawnPosition;
    }
}

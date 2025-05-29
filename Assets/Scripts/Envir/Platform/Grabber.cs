using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Envir.Platform
{
    [Serializable]
    public class Grabber : IState
    {
        private Collider2D _collider2D;

        private Transform grabPoint;

        private bool _triggerCount;
        private Transform grabbedObject;

        public override void Init()
        {
            IsNext.Value = false;
        }

        public override void TargetingOff(GameObject target)
        {
            if (target.TryGetComponent(out Collider2D col))
                HandleTriggerEnter(col);
        }

        private void HandleTriggerEnter(Collider2D collider)
        {
            if (collider.attachedRigidbody == null)
                return;

            if (_triggerCount)
            {
                grabbedObject = collider.transform;
                MoveGrabbedObjectAsync(grabbedObject).Forget();
                _triggerCount = false;
            }
            else if (!_triggerCount)
            {
                ReleaseObject();
                _triggerCount = true;
            }
        }

        private async UniTaskVoid MoveGrabbedObjectAsync(Transform obj)
        {
            if (obj == null)
                return;

            // Перемещение объекта плавно к точке захвата (можно любое действие)
            Vector3 startPosition = obj.position;
            Vector3 endPosition = grabPoint.position;
            float duration = 0.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (obj == null) break;

                obj.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            if (obj != null)
                obj.position = endPosition;
        }

        private void ReleaseObject()
        {
            grabbedObject = null;
        }

        public override void Updater()
        {
            MoveGrabbedObjectAsync(Target);
        }
    }
}
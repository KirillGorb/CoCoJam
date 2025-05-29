using System;
using UnityEngine;

namespace Envir.Platform
{
    [Serializable]
    public class ChaseArray : IState
    {
        private PlatformChaser[] platforms;

        public override void Init()
        {
            IsNext.Value = false;
        }

        public override void Updater()
        {
            foreach (var pc in platforms)
            {
                pc.UpdateChase(Time.deltaTime);
            }
        }
    }

    [Serializable]
    public struct PlatformChaser
    {
        public Transform platform;
        public Transform target;
        public float speed;
        private float fixedY;

        public void Init()
        {
            if (platform != null)
                fixedY = platform.position.y;
        }

        public void UpdateChase(float deltaTime)
        {
            if (platform == null || target == null)
                return;
            float direction = target.position.x > platform.position.x ? 1f : -1f;
            Vector3 newPos = platform.position;
            newPos.x += direction * speed * deltaTime;
            newPos.y = fixedY;
            newPos.z = platform.position.z;
            platform.position = newPos;
        }
    }
}
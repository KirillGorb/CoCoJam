using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;

namespace CodeScripts.Camera
{
    [Serializable]
    public class ModelCamera
    {
        public List<Transform> points;
    }

    public class CameraInstaller : MonoInstaller
    {
        [SerializeField] private Transform target;
        [SerializeField] private ModelCamera[] models;

        public override void InstallBindings()
        {
            Container.BindInstance(target).WithId("Target").AsCached();
            Container.BindInstance(transform).WithId("This").AsCached();

            Container.BindInstance(models).AsCached();
        }


#if UNITY_EDITOR
        [SerializeField] private Vector3 viewType;

        private void OnDrawGizmos()
        {
            foreach (ModelCamera modelCamera in models)
            {
                if (modelCamera.points.Count > 1)
                {
                    Gizmos.color = Color.red;
                    for (int i = 1; i < modelCamera.points.Count; i++)
                        Gizmos.DrawLine(modelCamera.points[i - 1].position, modelCamera.points[i].position);

                    foreach (Transform item in modelCamera.points)
                        Gizmos.DrawWireCube(item.position, viewType * item.position.z);
                }
            }
        }
#endif
    }
}
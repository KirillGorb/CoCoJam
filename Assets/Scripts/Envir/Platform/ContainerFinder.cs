using System;
using System.Linq;
using UnityEngine;

namespace Envir.Platform
{
    public enum ETargetType
    {
        Player,
        Enemy
    }

    public class ContainerFinder : MonoBehaviour
    {
        [field: SerializeField] public Find[] Finds { get; private set; }

        [Serializable]
        public struct Find
        {
            public ETargetType type;
            public GameObject[] containers;
        }

        public GameObject[] GetContainer(ETargetType type) => Finds.FirstOrDefault(e => e.type == type).containers;
    }
}
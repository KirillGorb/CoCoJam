using System;
using UnityEngine;

namespace CodeScripts.Respawn.RespawnPoint.Abstraction
{
    public abstract class IRespawnPoint : MonoBehaviour
    {
        public abstract Vector2 RespawnPosition { get; protected set; }
        public abstract IRespawnPoint Next { get; protected set; }
    }
}
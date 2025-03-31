using CodeScripts.Abstraction;
using UnityEngine;

namespace CodeScripts.PlayerMove.State.Datas
{
    public struct MovePlatformData : IData
    {
        public float Offset;
        public Rigidbody2D Platform;
    }
}
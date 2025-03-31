using CodeScripts.Abstraction;
using UnityEngine;

namespace CodeScripts.PlayerMove.State.Datas
{
    public struct HookData : IData
    {
        public Vector2 MoveToPoint;
        public Vector2 StartPoint;
        public Vector2 ControlPoint;
    }

    public struct MoveData : IData
    { }
    public struct NoMoveData : IData
    { }
}
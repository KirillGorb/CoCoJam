using UnityEngine;

namespace CodeScripts.PlayerMove.Config
{
    [CreateAssetMenu(fileName = "HookConfig", menuName = "Player/MoveModel/HookConfig", order = 1)]
    public class HookConfig : ScriptableObject
    {
        [field: SerializeField] public Vector2 MaxPosCheck { get; private set; }
        [field: SerializeField] public Vector2 ControlPointOffset { get; private set; }
        [field: SerializeField] public float DistCheck { get; private set; }
        [field: SerializeField] public float MaxDistUpCheck { get; private set; }
        [field: SerializeField] public LayerMask LayerCheck { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
    }
}
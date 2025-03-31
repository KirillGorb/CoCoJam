using System.Diagnostics.Contracts;
using UnityEngine;

namespace CodeScripts.PlayerMove
{
    [CreateAssetMenu(fileName = "InterectiveConfig", menuName = "Player/MoveModel/InterectiveConfig", order = 2)]
    public class InterectiveConfig : ScriptableObject
    {
        [field: SerializeField] public Vector2 Size { get; private set; }
        [field: SerializeField] public Vector2 OffSet { get; private set; }
        [field: SerializeField] public float MaxAngle { get; private set; }
        [field: SerializeField] public float MinAngle { get; private set; }
        [field: SerializeField] public LayerMask InteractiveLayers { get; private set; }
        [field: SerializeField] public float AddSpeedMove { get; private set; }
    }
}

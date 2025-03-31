using CodeScripts.PlayerMove.Config.Data;
using UnityEngine;

namespace CodeScripts.PlayerMove.Config
{
    [CreateAssetMenu(fileName = "ConfigMove", menuName = "Player/MoveModel/Config", order = 1)]
    public class ConfigMove : ScriptableObject
    {
        [field: SerializeField] public ConfigMoveX MoveX { get; private set; }
        [field: SerializeField] public ConfigMoveY MoveY { get; private set; }
    }
}
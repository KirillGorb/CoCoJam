using UnityEngine;

namespace CodeScripts.Timeline.Model
{
    [CreateAssetMenu(fileName = "AgesModel", menuName = "Timeline/AgesModel")]
    public class AgesModel : ScriptableObject
    {
        [field: SerializeField] public CollapseModel[] Ages { get; set; } 
    }
}
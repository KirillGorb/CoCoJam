using UnityEngine;

namespace CodeScripts.Timeline.Model
{
    [CreateAssetMenu(fileName = "KeyActivate", menuName = "Timeline/KeyActivate")]
    public class KeyActivate : ScriptableObject
    {
        [field: SerializeField] public CollapseModel[] IdCollapse { get; set; }
    }
}
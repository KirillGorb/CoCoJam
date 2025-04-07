using UnityEngine;

namespace CodeScripts.Timeline.Model
{
    [CreateAssetMenu(fileName = "ContainerAllCollapse", menuName = "Timeline/ContainerAllCollapse")]
    public class ContainerAllCollapse : ScriptableObject
    {
        [field: SerializeField] public CollapseModel[] Containers { get; private set; }
    }
}
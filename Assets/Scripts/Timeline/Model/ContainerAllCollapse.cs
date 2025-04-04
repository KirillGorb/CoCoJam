using System.Collections.Generic;
using UnityEngine;

namespace CodeScripts.Timeline.Model
{
    [CreateAssetMenu(fileName = "ContainerAllCollapse", menuName = "Timeline/ContainerAllCollapse")]
    public class ContainerAllCollapse : ScriptableObject
    {
        [field: SerializeField] public List<CollapseModel> Containers { get; private set; }

        public void LoadID()
        {
            int i = 0;
            foreach (var item in Containers)
                item.ID = i++;
        }
    }
}
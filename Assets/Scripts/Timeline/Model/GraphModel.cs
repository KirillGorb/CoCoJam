using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeScripts.Timeline.Model
{
    [Serializable]
    public struct ItemGraph
    {
        [field: SerializeField] public CollapseModel Current { get; set; }
        [field: SerializeField] public List<CollapseModel> Next { get; set; }
    }

    [CreateAssetMenu(menuName = "Timeline/GraphModel", fileName = "GraphModel")]
    public class GraphModel : ScriptableObject
    {
        [field: SerializeField] public ContainerAllCollapse AllCollapse { get; set; }

        [field: SerializeField, ListDrawerSettings]
        public List<ItemGraph> GraphCollapse { get; private set; }

        public ItemGraph Find(int idCollapse) =>
            GraphCollapse.Find(e => e.Current.ID == idCollapse);
    }
}
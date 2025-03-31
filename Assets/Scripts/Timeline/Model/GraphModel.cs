using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeScripts.Timeline
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
        
        [field: SerializeField, ListDrawerSettings(Expanded = true)]
        public List<ItemGraph> GraphCollapse { get; private set; }
    }
}
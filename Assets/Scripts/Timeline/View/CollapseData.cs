using CodeScripts.Timeline.Model;
using ModestTree;
using UnityEngine;

namespace CodeScripts.Timeline.View
{
    [CreateAssetMenu(fileName = "CollapseData", menuName = "Timeline/CollapseData", order = 0)]
    public class CollapseData : ScriptableObject
    {
        [field: SerializeField] public Color[] Colors { get; set; }
        [field: SerializeField] public ECollapseMode[] Mode { get; set; }

        public Color Search(ECollapseMode mode) => Colors[Mode.IndexOf(mode)];
    }
}
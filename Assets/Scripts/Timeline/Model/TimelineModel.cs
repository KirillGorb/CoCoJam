using UnityEngine;

namespace CodeScripts.Timeline.Model
{
    [CreateAssetMenu(fileName = "TimelineModel", menuName = "Timeline/TimelineModel")]
    public class TimelineModel : ScriptableObject
    {
        [field: SerializeField] public CollapseModel[] Timeline { get; set; }
    }
}
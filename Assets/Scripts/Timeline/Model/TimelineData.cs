using System.Linq;
using UnityEngine;

namespace CodeScripts.Timeline.Model
{
    [CreateAssetMenu(fileName = "TimelineData", menuName = "Timeline/TimelineData")]
    public class TimelineData : ScriptableObject
    {
        [field: SerializeField] public TimelineModel[] Timelines { get; set; }
        [field: SerializeField] public AgesModel[] Ages { get; set; }
        [field: SerializeField] public ContainerAllCollapse AllCollapse { get; set; }

        public TimelineModel GetTimeline(CollapseModel collapse) =>
            Timelines.FirstOrDefault(e => e.Timeline.Contains(collapse));

        public int GetAgeId(CollapseModel collapse)
        {
            int i = 0;
            foreach (var age in Ages)
            {
                if (age.Ages.Contains(collapse))
                    return i;
                i++;
            }

            return -1;
        }
    }
}
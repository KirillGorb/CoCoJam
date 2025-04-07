using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace CodeScripts.Timeline.Model
{
    public enum ECollapseMode
    {
        Inactive,
        Active,
        Ends,
        Rollback,
        Cansel
    }

    [CreateAssetMenu(fileName = "CollapseModel", menuName = "Timeline/CollapseModel")]
    public class CollapseModel : ScriptableObject
    {
        [field: SerializeField] public int ID { get; set; }
        [field: SerializeField] public ReactiveProperty<ECollapseMode> Mode { get; private set; } = new();
        [field: SerializeField] public string ScenePlay { get; private set; }
        [field: SerializeField] public CollapseModel[] Branches { get; private set; }

        [SerializeField, LabelText("Model Name")]
        private string modelName;

        [SerializeField, Range(0, 100)] private int value;

        [SerializeField, TextArea] private string description;

        public void SetActiveModeOnBranch()
        {
            if (Branches is { Length: > 0 })
                foreach (var b in Branches)
                    if (b.Mode.Value is not (ECollapseMode.Ends or ECollapseMode.Cansel))
                        b.Mode.Value = ECollapseMode.Active;
        }
    }
}
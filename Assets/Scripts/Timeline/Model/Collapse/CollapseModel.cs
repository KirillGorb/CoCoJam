using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        [field: SerializeField] public ReactiveProperty<CollapseData> Data { get; private set; } = new();
        [field: SerializeField] public string ScenePlay { get; private set; }
        [field: SerializeField] public CollapseModel[] Branches { get; private set; }

        [SerializeField, LabelText("Model Name")]
        private string modelName;

        [SerializeField, Range(0, 100)] private int value;

        [SerializeField, TextArea] private string description;

        public void SetActivate(bool isA)
        {
            Data.Value = new CollapseData { Mode = Data.Value.Mode, IsActivate = isA };
        }

        public bool IsSetData(ECollapseMode mode, bool isA)
        {
            if (Data.Value.Mode is not (ECollapseMode.Ends or ECollapseMode.Cansel))
            {
                Data.Value = new CollapseData { Mode = mode, IsActivate = isA };
                return true;
            }

            return false;
        }

        public bool IsSetData(ECollapseMode mode)
        {
            if (Data.Value.Mode is not (ECollapseMode.Ends or ECollapseMode.Cansel))
            {
                Data.Value = new CollapseData { Mode = mode, IsActivate = Data.Value.IsActivate };
                return true;
            }

            return false;
        }

        public void SetModeOnBranch(ECollapseMode mode)
        {
            if (Branches is { Length: > 0 })
                foreach (var b in Branches)
                    b.IsSetData(mode);
        }
    }
}
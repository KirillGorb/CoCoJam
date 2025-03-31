using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeScripts.Timeline
{
    [CreateAssetMenu(fileName = "CollapseModel", menuName = "Timeline/CollapseModel")]
    public class CollapseModel : ScriptableObject
    {
        [SerializeField, LabelText("Model Name")] 
        private string modelName;

        [SerializeField, Range(0, 100)] 
        private int value;

        [SerializeField, TextArea] 
        private string description;
    }
}
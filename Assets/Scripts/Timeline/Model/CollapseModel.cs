using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CodeScripts.Timeline.Model
{
    public enum ECollapseMode
    {
        Inactive,
        Active,
        End,
    }
    
    [CreateAssetMenu(fileName = "CollapseModel", menuName = "Timeline/CollapseModel")]
    public class CollapseModel : ScriptableObject
    {
        [field: SerializeField] public int ID { get; set; }
        [field: SerializeField] public SceneAsset ScenePlay { get; set; }
        
        [SerializeField, LabelText("Model Name")] 
        private string modelName;

        [SerializeField, Range(0, 100)] 
        private int value;

        [SerializeField, TextArea] 
        private string description;
    }
}
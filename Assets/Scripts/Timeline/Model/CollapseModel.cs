using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeScripts.Timeline
{
    [CreateAssetMenu(fileName = "CollapseModel", menuName = "Timeline/CollapseModel")]
    public class CollapseModel : ScriptableObject
    {
        [field: SerializeField] public bool IsActive { get; set; }
        [field: SerializeField] public SceneAsset ScenePlay { get; set; }
        
        [SerializeField, LabelText("Model Name")] 
        private string modelName;

        [SerializeField, Range(0, 100)] 
        private int value;

        [SerializeField, TextArea] 
        private string description;
    }
}
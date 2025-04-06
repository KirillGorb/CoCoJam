using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeScripts.Timeline.Model
{
    public enum ECollapseMode
    {
        Inactive,
        Active,
        End,
        Rollback,
        Cansel
    }
    
    [CreateAssetMenu(fileName = "CollapseModel", menuName = "Timeline/CollapseModel")]
    public class CollapseModel : ScriptableObject
    {
        [field: SerializeField] public int ID { get; set; }
        [field: SerializeField] public ECollapseMode Mode { get; set; }
        [field: SerializeField] public string ScenePlay { get; set; }
        [field: SerializeField] public List<CollapseModel> Branches { get; set; }
        
        [SerializeField, LabelText("Model Name")] 
        private string modelName;

        [SerializeField, Range(0, 100)] 
        private int value;

        [SerializeField, TextArea] 
        private string description;
    }
}
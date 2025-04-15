using Sirenix.OdinInspector;
using UnityEngine;
using CodeScripts.TaskSystem;

namespace CodeScripts.Dialog
{
    /// <summary>
    /// Answer- вопрос, Question- ответ, Move- действие нпс, Task- задание
    /// </summary>
    public enum EDialogType
    {
        Answer,
        Question,
        Move,
        Task,
    }

    [CreateAssetMenu(menuName = "DialogSystem/ModelDialog", fileName = "ModelDialog")]
    public class ModelDialog : ScriptableObject
    {
        [field: SerializeField] public string Key { get; private set; }

        [field: SerializeField, Tooltip("Answer- вопрос, Question- ответ, Move- действие нпс, Task- задание")]
        public EDialogType DialogType { get; private set; }

        [field: SerializeField] public ModelDialog[] Next { get; private set; }

        [ShowIf(nameof(IsTaskDialog)), SerializeField]
        private TaskModel task;

        private bool IsTaskDialog() => DialogType == EDialogType.Task;

        public TaskModel Task => task;
    }
}
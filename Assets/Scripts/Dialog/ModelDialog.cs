using Sirenix.OdinInspector;
using UnityEngine;
using CodeScripts.TaskSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


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
        End
    }

    [CreateAssetMenu(menuName = "DialogSystem/ModelDialog", fileName = "ModelDialog")]
    public class ModelDialog : ScriptableObject
    {
        public int ID { get; set; }

        [field: SerializeField] public string Key { get; private set; }

        [field: SerializeField, Tooltip("Answer- вопрос, Question- ответ, Move- действие нпс, Task- задание")]
        public EDialogType DialogType { get; private set; }

        [field: SerializeField] public ModelDialog[] Next { get; private set; }

        [ShowIf("@DialogType == EDialogType.End || DialogType == EDialogType.Task"), SerializeField]
        private TaskModel task;

        public TaskModel Task => task;
        
        
      [SerializeField]  public BaseTask Instance;

        [SerializeField,ValueDropdown(nameof(GetDerivedTypes)), OnValueChanged(nameof(OnTypeSelected))]
        private string selectedTypeName;

        private string[] GetDerivedTypes()
        {
            var baseType = typeof(BaseTask);
            var assembly = baseType.Assembly;
            return assembly.GetTypes()
                .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract)
                .OrderBy(t => t.Name)
                .Select(t => t.FullName).ToArray(); 
        }

        private void OnTypeSelected(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                Instance = null;
                return;
            }

            Type type = Type.GetType(typeName);
            if (type != null)
                Instance = (BaseTask)Activator.CreateInstance(type);
            
            Instance.Execute();
        }
    }
}


[Serializable]
public abstract class BaseTask
{
    public abstract void Execute();
}

[Serializable]
public class ExampleTask : BaseTask
{
    public override void Execute()
    {
        Debug.Log("Executing ExampleTask");
    }
}

[Serializable]
public class AnotherTask : BaseTask
{
    public override void Execute()
    {
        Debug.Log("Executing AnotherTask");
    }
}
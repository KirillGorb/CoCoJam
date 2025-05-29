using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeScripts.TaskSystem
{
    [Serializable]
    public record TaskEnd
    {
        public string paramKey;
        public int count;
        public TaskModel next;
    }

    [CreateAssetMenu(menuName = "DialogSystem/TaskModel", fileName = "TaskModel")]
    public class TaskModel : ScriptableObject
    {
        [SerializeField] private string taskDescription;

        [field: SerializeField] public TaskEnd TaskEnd { get; private set; }
    }
}
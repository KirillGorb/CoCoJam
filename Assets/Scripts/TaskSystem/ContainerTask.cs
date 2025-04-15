using UnityEngine;

namespace CodeScripts.TaskSystem
{
    [CreateAssetMenu(menuName = "DialogSystem/ContainerTask", fileName = "ContainerTask")]
    public class ContainerTask : ScriptableObject
    {
        [field: SerializeField] public TaskModel[] Tasks { get; private set; }
    }
}
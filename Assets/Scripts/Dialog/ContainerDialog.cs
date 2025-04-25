using UnityEngine;

namespace CodeScripts.Dialog
{
    [CreateAssetMenu(menuName = "DialogSystem/ContainerDialog", fileName = "ContainerDialog")]
    public class ContainerDialog : ScriptableObject
    {
        [field: SerializeField] public ModelDialog[] Dialogs { get; private set; }
    }
}
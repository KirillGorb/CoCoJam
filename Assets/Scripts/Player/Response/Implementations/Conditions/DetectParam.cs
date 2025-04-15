using CodeScripts.Abstraction;
using CodeScripts.TaskSystem;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations.Conditions

{
    public class DetectParam : MonoBehaviour, InteractionCondition, IData
    {
        [field: SerializeField] public TaskModel ParamKey { get; private set; }
        public int ID { get; set; }

        [Inject]
        private void Init(DataSwitcher dataSwitcher)
        {
            dataSwitcher.Container.Add(typeof(KeyObject), this);
        }
    }
}
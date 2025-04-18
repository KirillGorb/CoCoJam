using CodeScripts.Abstraction;
using CodeScripts.PlayerResponse.Player.Response.Implementations.Conditions;
using CodeScripts.TaskSystem;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations.Conditions
{
    public class TaskApprove : MonoBehaviour, InteractionCondition, IData
    {
        [field: SerializeField] public TaskModel ParamKey { get; private set; }
        public int ID { get; set; }

        [Inject] private ServiceInteraction _s;

        public void Callback() => _s.task = this;
    }
}
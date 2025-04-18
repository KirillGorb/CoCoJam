using CodeScripts.Abstraction;
using CodeScripts.PlayerResponse.Implementations.Conditions;
using CodeScripts.PlayerResponse.Player.Response.Implementations.Conditions;
using UnityEngine;
using Zenject;

namespace CodeScripts.Dialog.ViewDialog
{
    public class LoadDialog : MonoBehaviour, InteractionCondition, IData
    {
        [field: SerializeField] public ModelDialog Model { get; private set; }
        [Inject] private ServiceInteraction _s;

        public void Callback() => _s.dialog = this;
    }
}
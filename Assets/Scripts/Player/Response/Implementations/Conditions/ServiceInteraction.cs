using CodeScripts.Abstraction;
using CodeScripts.Dialog.ViewDialog;
using CodeScripts.PlayerResponse.Implementations;
using CodeScripts.PlayerResponse.Implementations.Conditions;
using Zenject;

namespace CodeScripts.PlayerResponse.Player.Response.Implementations.Conditions
{
    public class ServiceInteraction : IData
    {
        public DataKey key;
        public LoadDialog dialog;
        public TaskApprove task;
        
        [Inject]
        private void Init(DataSwitcher dataSwitcher)
        {
            dataSwitcher.Container.Add(typeof(KeyObject), this);
            dataSwitcher.Container.Add(typeof(LoadDialog), this);
            dataSwitcher.Container.Add(typeof(TaskApprove), this);
        }
    }
}
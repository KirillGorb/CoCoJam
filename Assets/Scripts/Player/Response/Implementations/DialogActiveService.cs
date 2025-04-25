using System;
using System.Threading;
using CodeScripts.Abstraction;
using CodeScripts.Dialog.ViewDialog;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using CodeScripts.PlayerResponse.Player.Response.Implementations.Conditions;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations
{
    public class DialogActiveService : IPlayerResponseService, IInitializable, IDisposable
    {
        [Inject] private readonly ComponentResponseSwitcher _componentResponseSwitcher;

        [Inject] private readonly DialogUIView view;

        public void Initialize()
        {
            _componentResponseSwitcher.AddResponse(typeof(LoadDialog), this);
        }

        public UniTask Response<T>(T data, CancellationToken token = default) where T : IData
        {
            if (data is not ServiceInteraction s)
                return UniTask.CompletedTask;
            view.Load(s.dialog.ID);
            return UniTask.CompletedTask;
        }

        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData
        {
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _componentResponseSwitcher?.Dispose();
        }
    }
}
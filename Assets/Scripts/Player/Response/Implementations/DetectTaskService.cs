using System;
using System.Linq;
using System.Threading;
using CodeScripts.Abstraction;
using CodeScripts.Dialog;
using CodeScripts.Dialog.ViewDialog;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using CodeScripts.PlayerResponse.Implementations.Conditions;
using CodeScripts.PlayerResponse.Player.Response.Implementations.Conditions;
using CodeScripts.TaskSystem;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using UniRx;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeScripts.PlayerResponse.Implementations
{
    public class DetectTaskService : IPlayerResponseService, IInitializable, IDisposable
    {
        [Inject] private readonly ComponentResponseSwitcher _componentResponseSwitcher;
        [Inject] private readonly LoadTaskSave _load;
        [Inject] private readonly ContainerDialog _containerDialog;

        private readonly CompositeDisposable _disposable = new();

        public void Initialize()
        {
            _componentResponseSwitcher.AddResponse(typeof(TaskApprove), this);

            _containerDialog.Dialogs.ForEach((e, i) => e.ID = i);

            var l = Object.FindObjectsOfType<LoadDialog>();
            _load.LoadDialog = new int[l.Length];
            l.ForEach((e, i) => (e.ID, _load.LoadDialog[i]) = (i, e.Model.ID));

            var t = Object.FindObjectsOfType<TaskApprove>()
                .ForEach((e, i) =>
                {
                    e.ID = i;
                    _load.UsesTask
                        .WhereU(t => t.Item1 == i)
                        .Subscribe(t => e.gameObject.SetActive(!t.Item2))
                        .AddTo(_disposable);
                });
            _load.Activators = new bool[t.Count()];

            _load.Load();
        }

        public UniTask Response<T>(T data, CancellationToken token = default) where T : IData
        {
            if (data is not ServiceInteraction s)
                return UniTask.CompletedTask;

            if (_load.Detect(s.task.ParamKey, s.task.ID))
                Object.Destroy(s.task.gameObject);
            return UniTask.CompletedTask;
        }

        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData
        {
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _componentResponseSwitcher?.Dispose();
            _disposable?.Dispose();
        }
    }
}
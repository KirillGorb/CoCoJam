using System;
using System.Linq;
using System.Threading;
using CodeScripts.Abstraction;
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

        private readonly CompositeDisposable _disposable = new();

        public void Initialize()
        {
            _componentResponseSwitcher.AddResponse(typeof(TaskApprove), this);

            var d = Object
                .FindObjectsOfType<TaskApprove>()
                .ForEach((e, i) =>
                {
                    e.ID = i;
                    _load.UsesTask
                        .WhereU(t => t.id == i)
                        .Subscribe(t => e.gameObject.SetActive(!t.status))
                        .AddTo(_disposable);
                })
                .Select(e => new TaskStatus { id = e.ID, status = false })
                .ToArray();

            _load.Load(d);
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
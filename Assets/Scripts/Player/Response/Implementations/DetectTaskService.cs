using System;
using System.Linq;
using System.Threading;
using CodeScripts.Abstraction;
using CodeScripts.PlayerResponse.Implementations.Abstraction;
using CodeScripts.PlayerResponse.Implementations.Conditions;
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
        [Inject] private readonly ComponentResponseSwitcher _switcher;
        [Inject] private readonly LoadTask _load;

        private readonly CompositeDisposable _disposable = new();

        public void Initialize()
        {
            _switcher.AddResponse(typeof(DetectParam), this);

            var d = Object
                .FindObjectsOfType<DetectParam>()
                .ForEach((e, i) =>
                {
                    e.ID = i;
                    _load.UsesTask
                        .Where(t => t.id == i)
                        .Subscribe(t => e.gameObject.SetActive(!t.status))
                        .AddTo(_disposable);
                })
                .Select(e => new TaskStatus { id = e.ID, status = false })
                .ToArray();

            _load.Load(d);
        }

        public UniTask Response<T>(T data, CancellationToken token = default) where T : IData
        {
            if (data is not DetectParam detect)
                return UniTask.CompletedTask;

            _load.Detect(detect.ParamKey, detect.ID);
            return UniTask.CompletedTask;
        }

        public UniTask StopResponse<T>(T data, CancellationToken token = default) where T : IData
        {
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _switcher?.Dispose();
            _disposable?.Dispose();
        }
    }
}
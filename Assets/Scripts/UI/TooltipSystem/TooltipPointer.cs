using System.Threading;
using CodeScripts.Abstraction;
using Cysharp.Threading.Tasks;
using UniRx;

namespace CodeScripts.UI.TooltipSystem
{
    public class TooltipPointer
    {
        private ITooltipView _tooltip;

        private CancellationTokenSource _cancellationTokenSource;
        private CompositeDisposable _disposables = new();

        public ReactiveCommand<IData> Data { get; } = new();

        public void Init(ITooltipView tooltip)
        {
            _tooltip = tooltip;
            Data.Subscribe(tooltip.SetData).AddTo(_disposables);
        }

        public void Enter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            ShowTooltipWithDelay(_tooltip.TimeToShowMS, _cancellationTokenSource.Token).Forget();
        }

        public void Exit()
        {
            _cancellationTokenSource?.Cancel();
            _tooltip.Hide();
        }

        private async UniTaskVoid ShowTooltipWithDelay(int delayMilliseconds, CancellationToken cancellationToken)
        {
            await UniTask.Delay(delayMilliseconds, cancellationToken: cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
            {
                _tooltip.UpdatePosition();
                _tooltip.Show();
            }
        }
    }

}
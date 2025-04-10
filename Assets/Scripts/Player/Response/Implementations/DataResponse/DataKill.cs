using CodeScripts.Abstraction;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations
{
    public class DataKill : IData, IInitializable
    {
        public float KillTime = 0.5f;

        [Inject] private DataSwitcher _dataSwitcher;

        public void Initialize()
        {
            _dataSwitcher.Container.Add(13, this);
        }
    }
}
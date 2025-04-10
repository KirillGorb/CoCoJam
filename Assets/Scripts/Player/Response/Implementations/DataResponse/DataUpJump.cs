using System;
using CodeScripts.Abstraction;
using Zenject;

namespace CodeScripts.PlayerResponse.Implementations
{
    public class DataUpJump : IData, IInitializable
    {
        public TimeSpan TimeToEnd = TimeSpan.FromSeconds(3);

        public float JumpTime = 15;

        [Inject] private DataSwitcher _dataSwitcher;

        public void Initialize()
        {
            _dataSwitcher.Container.Add(12, this);
        }
    }
}
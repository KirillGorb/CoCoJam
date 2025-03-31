using CodeScripts.PlayerInputs;
using CodeScripts.PlayerMove.Config;
using CodeScripts.PlayerMove.Config.Data;
using UnityEngine;
using Zenject;

namespace CodeScripts.PlayerMove.State.Logics
{
    public class MoveX : IInitializable
    {
        [Inject] private readonly ConfigMove _config;

        private ConfigMoveX _configMoveX;
        private float _slowSpeed;

        public ModificationValue SpeedMod { get; private set; }
        public ModificationValue SpeedSedMod { get; private set; }

        public float Value()
        {
            SlowMove();
            return InputCallback.HorizontalInput * _slowSpeed *
               (InputCallback.SedInput ? SpeedSedMod.GetValue : SpeedMod.GetValue);
        }

        public void Initialize()
        {
            _configMoveX = _config.MoveX;

            SpeedMod = new(_configMoveX.speed);
            SpeedSedMod = new(_configMoveX.speedSed);
        }

        private void SlowMove()
        {
            _slowSpeed = InputCallback.HorizontalInput == 0
                ? 0
                : Mathf.Min(_slowSpeed + _configMoveX.slowVelocity * Time.fixedDeltaTime, 1);
        }
    }
}
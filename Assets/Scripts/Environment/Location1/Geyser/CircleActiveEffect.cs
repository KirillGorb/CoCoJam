using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace CodeScripts.Environment.Location1.Geyser
{
    public class CircleActiveEffect : MonoBehaviour
    {
        [SerializeField] private ModelInjectValue[] states;
        [SerializeField] private UnityEvent<float>[] eventForStates;

        [SerializeField] private bool isCircle = true;
        [SerializeField] private VisualEffect effect;

        private int numState;

        private void Start()
        {
            TimeCircle(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid TimeCircle(CancellationToken cancellationToken = default)
        {
            while (isCircle)
            {
                states[numState].SetValueMax(effect);
                var time = states[numState].timeActiveState;
                eventForStates[numState]?.Invoke(time);

                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: cancellationToken);
                }
                catch
                {
                    return;
                }

                numState++;
                if (numState >= states.Length)
                    numState = 0;
            }
        }
    }
}
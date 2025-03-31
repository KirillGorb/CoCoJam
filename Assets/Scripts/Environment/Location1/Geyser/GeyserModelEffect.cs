using System;
using UnityEngine;
using UnityEngine.VFX;

namespace CodeScripts.Environment.Location1.Geyser
{
    [CreateAssetMenu(fileName = "GeyserModelEffect", menuName = "Environment/Location1/GeyserModelEffect", order = 0)]
    public class GeyserModelEffect : ModelInjectValue
    {
        [Serializable]
        public struct Variable
        {
            public float MaxValue;
            public float MinValue;

            public float RandomValue => UnityEngine.Random.Range(MinValue, MaxValue);
        }

        [SerializeField] private Variable activeRate;
        [SerializeField] private Variable activeSize;
        [SerializeField] private Variable activeVelocity;

        public override void SetValueMax<T>(T value)
        {
            if (value is not VisualEffect visualEffect) return;

            visualEffect.SetFloat("ParticleRate", activeRate.RandomValue);
            visualEffect.SetFloat("ParticleSize", activeSize.RandomValue);
            visualEffect.SetFloat("Velocity", activeVelocity.RandomValue);
        }
    }
}
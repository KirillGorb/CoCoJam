using UnityEngine;
using UnityEngine.VFX;

namespace Folo.Code.Scripts.Environment.Location1
{
    public class GeyserParameters : MonoBehaviour
    {
        [SerializeField] private float activeRate = 30;
        [SerializeField] private float activeSize = 1;
        [SerializeField] private float activeVelocity = 3;

        [Space(4)] [SerializeField] private float inactiveRate = 10;
        [SerializeField] private float inactiveSize = 0.1f;
        [SerializeField] private float inactiveVelocity = 1;

        private VisualEffect _visual;

        private void Awake()
        {
            _visual = GetComponent<VisualEffect>();
        }

        public void SetValue(bool isActive)
        {
            if (isActive)
                SetValueMax(_visual);
            else
                SetValueMin(_visual);
        }

        private void SetValueMax(VisualEffect visualEffect)
        {
            visualEffect.SetFloat("ParticleRate", activeRate);
            visualEffect.SetFloat("ParticleSize", activeSize);
            visualEffect.SetFloat("Velocity", activeVelocity);
        }

        private void SetValueMin(VisualEffect visualEffect)
        {
            visualEffect.SetFloat("ParticleRate", inactiveRate);
            visualEffect.SetFloat("ParticleSize", inactiveSize);
            visualEffect.SetFloat("Velocity", inactiveVelocity);
        }
    }
}
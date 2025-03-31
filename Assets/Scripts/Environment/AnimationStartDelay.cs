using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeScripts.Environment
{
    public class AnimationStartDelay : MonoBehaviour
    {
        [SerializeField] private float timeStart;

        private Animation _anim;

        private void Start()
        {
            _anim = GetComponent<Animation>();
            StartAnim().Forget();
        }

        private async UniTaskVoid StartAnim()
        {
            try
            {
                await Task.Delay((int)(1000 * timeStart), this.GetCancellationTokenOnDestroy());
                _anim.Play();
            }
            catch
            {
            }
        }
    }
}
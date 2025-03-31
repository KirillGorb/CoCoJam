using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CodeScripts.Environment.Location1.AppleMan
{
    public class AppleMansInteractionWithPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject _target;

        [SerializeField] private UnityEvent _conectTarget;
        [SerializeField] private UnityEvent _conectPlayer;

        [SerializeField] private float _timeLateConecttarget;
        [SerializeField] private UnityEvent _conectLateTargetActivation;

        private void OnTriggerEnter2D(Collider2D other)
        {
           // if (other.TryGetComponent(out SceneController player))
           //     _conectPlayer?.Invoke();

            if (other.gameObject == _target)
                StartCoroutine(ConectTarget());
        }

        private IEnumerator ConectTarget()
        {
            _conectTarget?.Invoke();
            yield return new WaitForSeconds(_timeLateConecttarget);
            _conectLateTargetActivation?.Invoke();
        }
    }
}
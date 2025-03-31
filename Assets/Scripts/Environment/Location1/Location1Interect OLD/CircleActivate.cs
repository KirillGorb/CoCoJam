using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Folo.Code.Scripts.Environment.Location1
{
    public class CircleActivate : MonoBehaviour
    {
        [SerializeField] private UnityEvent<bool> isActiveCircle;
        [SerializeField] private float timerActiveSmallForm = 3;
        [SerializeField] private float timerActiveBigForm = 5;

        private bool _isActive;
        private bool _isCircle = true;

        private WaitForSeconds _timeActiveBig;
        private WaitForSeconds _timeActiveSmall;

        private void Awake()
        {
            _timeActiveBig = new(timerActiveBigForm);
            _timeActiveSmall = new(timerActiveSmallForm);
        }

        private void Start()
        {
            _isCircle = true;
            StartCoroutine(TimerActive());
        }

        private IEnumerator TimerActive()
        {
            while (_isCircle)
            {
                isActiveCircle.Invoke(_isActive);
                yield return _isActive ? _timeActiveBig : _timeActiveSmall;
                _isActive = !_isActive;
            }
        }
    }
}
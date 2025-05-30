using System.Collections.Generic;
using Envir.Platform;
using UniRx;
using UnityEngine;

namespace CodeScripts.Envir.Envir
{
    public class BaseOptionLogics : MonoBehaviour
    {
        [SerializeReference] private List<IState> statesQueue;

        private IState _current;
        private int _id;

        private void OnEnable()
        {
            _current?.Abort();
            _current = statesQueue[_id = 0];
            _current.Init();
        }

        private void OnDisable()
        {
            _current?.Abort();
            _current = new None();
            _current.Init();
        }

        private void Awake()
        {
            int i = 0;
            foreach (var state in statesQueue)
            {
                state.IsNext.WhereU(e => e).Subscribe(_ => NextState()).AddTo(this);
                var n = (i + 1) % statesQueue.Count;
                state.Next.Subscribe(e =>
                {
                    state.IsNext.Value = true;
                    if (e is Collider2D c)
                        statesQueue[n].Target = c;
                    else
                        statesQueue[n].SetNext(e);
                }).AddTo(this);

                i++;
            }
        }

        public void NextState()
        {
            _current?.Abort();
            _id = Mathf.Clamp(_id + 1, 0, statesQueue.Count - 1);
            _current = statesQueue[_id];
            _current.Init();
        }

        private void Update()
        {
            if ((_current.platform & EPlatform.Updater) is EPlatform.Updater)
                _current.Updater();
        }

        private void FixedUpdate()
        {
            if ((_current.platform & EPlatform.UpdaterFixed) is EPlatform.UpdaterFixed)
                _current.Updater();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if ((_current.platform & EPlatform.ColliderOnEnt) is EPlatform.ColliderOnEnt)
                _current.TargetingOnEnt(other.gameObject);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if ((_current.platform & EPlatform.ColliderOnEx) is EPlatform.ColliderOnEx)
                _current.TargetingOnEx(other.gameObject);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if ((_current.platform & EPlatform.ColliderOn) is EPlatform.ColliderOn)
                _current.TargetingOn(other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((_current.platform & EPlatform.TriggerOnEnt) is EPlatform.TriggerOnEnt)
                _current.TargetingOnEnt(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if ((_current.platform & EPlatform.TriggerOnEx) is EPlatform.TriggerOnEx)
                _current.TargetingOnEx(other.gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if ((_current.platform & EPlatform.TriggerOn) is EPlatform.TriggerOn)
                _current.TargetingOn(other.gameObject);
        }
    }
}
using System.Collections.Generic;
using Envir.Platform;
using UniRx;
using UnityEngine;
using Zenject;

namespace CodeScripts.Envir.Envir
{
    public class BaseOptionLogics : MonoBehaviour
    {
        [SerializeReference] private List<IState> statesQueue;

        private IState _current;
        private int _id;

     //   [Inject] private PlatformContainer _platform;

        private void Start()
        {
            foreach (var state in statesQueue)
            {
                state.IsNext.WhereU(e => e).Subscribe(_ => NextState()).AddTo(this);
     //           _platform.Subscriber(_current);
            }

       //     _platform.AddTo(this);
        }

        public void NextState()
        {
            _id = Mathf.Clamp(_id + 1, 0, statesQueue.Count - 1);
            _current = statesQueue[_id];
            _current.Init();
        }
    }
}
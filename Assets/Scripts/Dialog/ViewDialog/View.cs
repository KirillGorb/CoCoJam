using System.Linq;
using CodeScripts.TaskSystem;
using Plugins.Other;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeScripts.Dialog.ViewDialog
{
    public class View : MonoBehaviour
    {
        [SerializeField] private ModelDialog startNode;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button next;
        [SerializeField] private Transform container;

        [Inject] private readonly LoadTask _task;
        
        private readonly CompositeDisposable _disposable = new();

        private void Start()
        {
            Load(startNode);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void Spawn(ModelDialog m)
        {
            var b = Instantiate(next, container);
            b.OnClickAsObservable().Subscribe(_ => Next(m)).AddTo(_disposable);
            b.GetComponentInChildren<TMP_Text>().text = m.Key;
        }
        
        private void Load(ModelDialog m)
        {
            text.text = m.Key;

            container.ClearChild();
            foreach (var item in m.Next.Where(e => e.DialogType is EDialogType.Question))
                Spawn(item);
        }

        private void Next(ModelDialog m)
        {
            if (m.DialogType is EDialogType.Question)
                Load(m.Next.First());
            if(m.DialogType is EDialogType.Task)
                _task.AddTask(m.Task);
        }
    }
}
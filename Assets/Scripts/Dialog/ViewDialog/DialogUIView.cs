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
    public class DialogUIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button next;
        [SerializeField] private Transform container;

        [Inject] private readonly LoadTaskSave _taskSave;

        private readonly CompositeDisposable _disposable = new();

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Load(ModelDialog m)
        {
            gameObject.SetActive(true);

            text.text = m.Key;

            container.ClearChild();
            foreach (var item in m.Next.Where(e =>
                         e.DialogType is EDialogType.Question or EDialogType.Task &&
                         !_taskSave.GetTask().activeTasks.ContainsKey(e.Task.TaskEnd.paramKey)))
                Spawn(item);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void Spawn(ModelDialog m)
        {
            var b = Instantiate(next, container);
            b.gameObject.SetActive(true);
            b.OnClickAsObservable().Subscribe(_ => Next(m)).AddTo(_disposable);
            b.GetComponentInChildren<TMP_Text>().text = m.Key;
        }

        private void Next(ModelDialog m)
        {
            gameObject.SetActive(false);
            
            if (m.DialogType is EDialogType.Question)
                Load(m);
            if (m.DialogType is EDialogType.Task)
            {
                _taskSave.EndTask.WhereU(e => e == m.Task.TaskEnd.paramKey).Subscribe(k => Load(m.Next.FirstOrDefault(e=> e.DialogType is EDialogType.End && e.Task.TaskEnd.paramKey == k))).AddTo(_disposable);
                _taskSave.AddTask(m.Task);
            }
        }
    }
}
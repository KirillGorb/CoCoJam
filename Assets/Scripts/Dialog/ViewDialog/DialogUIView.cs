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

        private ModelDialog _progress;

        private void Start()
        {
            gameObject.SetActive(false);
            _taskSave.Delete.Subscribe(e=> _progress = null).AddTo(_disposable);
        }

        public void Load(ModelDialog m, bool isLoad)
        {
            if (_progress is null || isLoad)
                _progress = m;
            gameObject.SetActive(true);

            text.text = m.Key;

            container.ClearChild();
            foreach (var item in _progress.Next.Where(e =>
                         (e.DialogType is EDialogType.Question or EDialogType.Task &&
                          !_taskSave.GetTask().activeTasks.ContainsKey(e.Task.TaskEnd.paramKey)) ||
                         e.DialogType is EDialogType.End))
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
                Load(m, true);
            if (m.DialogType is EDialogType.Task)
            {
                _taskSave.EndTask.WhereU(e => e == m.Task.TaskEnd.paramKey).Subscribe(_ => _progress = m)
                    .AddTo(_disposable); //.Next.FirstOrDefault(e=> e.DialogType is EDialogType.End && e.Task.TaskEnd.paramKey == k)
                _taskSave.AddTask(m.Task);
            }
        }
    }
}
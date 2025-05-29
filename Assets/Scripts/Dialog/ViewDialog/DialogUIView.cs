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
        [Inject] private readonly ContainerDialog _containerDialog;

        private readonly CompositeDisposable _disposable = new();

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Load(int idActivator)
        {
            gameObject.SetActive(true);

            var m = _containerDialog.Dialogs[_taskSave.GetDialog(idActivator)];

            text.text = m.Key;

            container.ClearChild();
            foreach (var item in m.Next.Where(e =>
                         (e.DialogType is EDialogType.Question or EDialogType.Task &&
                          !_taskSave.GetTask().activeTasks.ContainsKey(e.Task.TaskEnd.paramKey))
                         || (e.DialogType is EDialogType.End && e.Task is null) ||(e.DialogType is EDialogType.End && e.Task is not null && _taskSave.IsEndTask(e.Task))))
                Spawn(idActivator, item);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void Spawn(int idActivator, ModelDialog m)
        {
            var b = Instantiate(next, container);
            b.gameObject.SetActive(true);
            b.OnClickAsObservable().Subscribe(_ => Next(idActivator, m)).AddTo(_disposable);
            b.GetComponentInChildren<TMP_Text>().text = m.Key;
        }

        private void Next(int idActivator, ModelDialog m)
        {
            gameObject.SetActive(false);
            _taskSave.SetDialog(idActivator, m.ID);
            if (m.DialogType is EDialogType.Question)
            {
                Load(idActivator);
            }

            if (m.DialogType is EDialogType.Task)
            {
                _taskSave.EndTask.WhereU(e => e == m.Task.TaskEnd.paramKey)
                    .Subscribe(_ => _taskSave.SetDialog(idActivator, m.ID))
                    .AddTo(_disposable);
                _taskSave.AddTask(m.Task);
            }
        }
    }
}
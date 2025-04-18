using System;
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

        [Inject] private readonly LoadTaskSave taskSave;

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
            foreach (var item in m.Next.Where(e => e.DialogType is EDialogType.Question or EDialogType.Task))
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
            if (m.DialogType is EDialogType.Question)
                Load(m.Next.First());
            if (m.DialogType is EDialogType.Task)
                taskSave.AddTask(m.Task);

            gameObject.SetActive(false);
        }
    }
}
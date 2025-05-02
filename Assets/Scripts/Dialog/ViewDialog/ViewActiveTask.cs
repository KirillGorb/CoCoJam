using CodeScripts.TaskSystem;
using Plugins.Other;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeScripts.Dialog.ViewDialog
{
    public class ViewActiveTask : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private TMP_Text textView;
        [SerializeField] private Button closeButton;

        [Inject] private readonly LoadTaskSave _taskSave;

        private void Start()
        {
            Select();

            _taskSave.TaskCheck.Subscribe(_ => Select()).AddTo(this);
            closeButton.OnClickAsObservable().Subscribe(_ => _taskSave.DeleteAll()).AddTo(this);
        }

        public void Select()
        {
            container.ClearChild();
            foreach (var (k, c) in _taskSave.GetTask().activeTasks)
            {
                var t = Instantiate(textView, container);
                t.gameObject.SetActive(true);
                t.text = k;
                t.color = c.Item1 > c.Item2 ? Color.green : Color.white;
            }
        }
    }
}
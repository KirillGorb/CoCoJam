using System;
using System.Collections.Generic;
using System.Linq;
using CodeScripts.Abstraction;
using CodeScripts.SaveLoadSystem;
using CodeScripts.Scene;
using Sirenix.Utilities;
using UniRx;
using Zenject;

namespace CodeScripts.TaskSystem
{
    [Serializable]
    public record TaskSD : IData
    {
        public Dictionary<string, int> activeTasks;
        public List<bool> allViewTask;
        public List<int> dialogProgress;
    }

    [Serializable]
    public record DictionaryTaskSD : IData
    {
        public Dictionary<int, TaskSD> sceneToTasks = new();
    }

    public class LoadTaskSave
    {
        [Inject] private readonly Save<DictionaryTaskSD> _save;
        [Inject] private readonly SceneController _scene;

        private readonly CompositeDisposable _disposable = new();

        private DictionaryTaskSD d_task;
        private TaskSD _task;

        public readonly ReactiveCommand<(int, bool)> UsesTask = new();
        public readonly ReactiveCommand<string> EndTask = new();
        public readonly ReactiveCommand<string> AddTaskCheck = new();

        public int[] LoadDialog { get; set; }
        public bool[] Activators { get; set; }

        public TaskSD GetTask() => _task ??= d_task.sceneToTasks[_scene.ThisIdScene];

        public void Load()
        {
            _save.SetSave("task");
            d_task = _save.LoadData();

            if (d_task is null || !d_task.sceneToTasks.ContainsKey(_scene.ThisIdScene))
            {
                d_task ??= new();
                _task = new();
                Delete();
                Save();
            }
            else
            {
                if (d_task.sceneToTasks.TryGetValue(_scene.ThisIdScene, out _task))
                    _task.allViewTask.ForEach((e, i) => UsesTask.Execute((i, e)));
            }

            Sub();
        }

        public void AddTask(TaskModel model)
        {
            var k = model.TaskEnd.paramKey;

            _task.activeTasks[k] = 0;
            Save();
            AddTaskCheck.Execute(k);
        }

        public bool Detect(TaskModel task, int idDetect)
        {
            if (_task.activeTasks.TryGetValue(task.TaskEnd.paramKey, out var v))
            {
                if (v + 1 <= task.TaskEnd.count)
                {
                    _task.activeTasks[task.TaskEnd.paramKey]++;
                    UsesTask.Execute((idDetect, true));
                    Save();
                    if (v + 1 >= task.TaskEnd.count)
                        EndTask.Execute(task.TaskEnd.paramKey);
                    return true;
                }
            }

            return false;
        }

        public int GetDialog(int id) => _task.dialogProgress[id];

        public void SetDialog(int id, int progress)
        {
            _task.dialogProgress[id] = progress;
            Save();
        }

        private void Sub()
        {
            UsesTask
                .Subscribe(e => _task.allViewTask[e.Item1] = e.Item2)
                .AddTo(_disposable);
        }

        private void Save()
        {
            d_task.sceneToTasks[_scene.ThisIdScene] = _task;
            _save.SaveData(d_task);
        }

        public void DeleteAll()
        {
            Delete();
            d_task = new();
            d_task.sceneToTasks[_scene.ThisIdScene] = _task;
            _save.SaveData(d_task);
        }

        private void Delete()
        {
            _task.activeTasks = new();
            _task.dialogProgress = LoadDialog.ToList();
            _task.allViewTask = Activators.ToList();
        }
    }
}
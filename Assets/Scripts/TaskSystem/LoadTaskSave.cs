using System;
using System.Collections.Generic;
using System.Linq;
using CodeScripts.Abstraction;
using CodeScripts.SaveLoadSystem;
using CodeScripts.Scene;
using ModestTree;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CodeScripts.TaskSystem
{
    [Serializable]
    public record TaskStatus
    {
        public int id;
        public bool status;
    }

    [Serializable]
    public record TaskSD : IData
    {
        public Dictionary<string, int> activeTasks;
        public List<TaskStatus> allViewTask;
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

        public readonly ReactiveCommand<TaskStatus> UsesTask = new();
        private readonly CompositeDisposable _disposable = new();

        private DictionaryTaskSD d_task;
        private TaskSD _task;

        public readonly ReactiveCommand<string> EndTask = new();

        public TaskSD GetTask() => _task ??= d_task.sceneToTasks[_scene.ThisIdScene];

        public void Load(TaskStatus[] status)
        {
            _save.SetSave("task");
            d_task = _save.LoadData();

            if (d_task is null || !d_task.sceneToTasks.ContainsKey(_scene.ThisIdScene))
            {
                d_task ??= new();
                _task = new();

                _task.activeTasks = new();
                _task.allViewTask = new(status);
                Save();
            }
            else
            {
                if (d_task.sceneToTasks.TryGetValue(_scene.ThisIdScene, out _task))
                    foreach (var view in _task.allViewTask)
                        UsesTask.Execute(view);
            }

            Sub();
        }

        public void AddTask(TaskModel model)
        {
            _task.activeTasks[model.TaskEnd.paramKey] = 0;
            Save();
        }

        public bool Detect(TaskModel task, int idDetect)
        {
            if (_task.activeTasks.TryGetValue(task.TaskEnd.paramKey, out var v))
            {
                if (v + 1 <= task.TaskEnd.count)
                {
                    _task.activeTasks[task.TaskEnd.paramKey]++;
                    UsesTask.Execute(new TaskStatus { id = idDetect, status = true });
                    Save();
                    Debug.Log(
                        $" ------- {task.TaskEnd.paramKey} ------- {d_task.sceneToTasks[_scene.ThisIdScene].activeTasks[task.TaskEnd.paramKey]} ------- {d_task.sceneToTasks[_scene.ThisIdScene].allViewTask[idDetect]} ------- ");
                    return true;
                }

                EndTask.Execute(task.TaskEnd.paramKey);
            }

            return false;
        }

        private void Sub()
        {
            UsesTask
                .Subscribe(e => _task.allViewTask[e.id] = e)
                .AddTo(_disposable);
        }

        private void Save()
        {
            d_task.sceneToTasks[_scene.ThisIdScene] = _task;
            _save.SaveData(d_task);
        }
    }
}
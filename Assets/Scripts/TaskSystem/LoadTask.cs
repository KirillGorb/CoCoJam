using System;
using System.Collections.Generic;
using System.Linq;
using CodeScripts.Abstraction;
using CodeScripts.SaveLoadSystem;
using CodeScripts.Scene;
using ModestTree;
using Sirenix.Utilities;
using UniRx;
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
    public class TaskSD : IData
    {
        public Dictionary<string, int> activeTasks;
        public List<TaskStatus> allViewTask;
    }

    [Serializable]
    public class DictionaryTaskSD : IData
    {
        public Dictionary<string, TaskSD> sceneToTasks;
    }

    public class LoadTask
    {
        [Inject] private readonly Save<DictionaryTaskSD> _save;
        [Inject] private readonly ContainerTask _container;
        [Inject] private readonly SceneController _scene;

        public readonly ReactiveCommand<TaskStatus> UsesTask = new();
        private readonly CompositeDisposable _disposable = new();

        private DictionaryTaskSD d_task;
        private TaskSD _task;

        public void Load(TaskStatus[] status)
        {
            d_task = _save.LoadData();

            _task = d_task.sceneToTasks[_scene.ThisNameScene];
            
            if (_task is null)
            {
                _task = new();

                _task.activeTasks = new();
                _task.allViewTask = new(status);
                Save();
            }
            else
            {
                foreach (var view in _task.allViewTask)
                    UsesTask.Execute(view);
            }

            Sub();
        }

        public void AddTask(TaskModel model)
        {
            if (_task.activeTasks.ContainsKey(model.TaskEnd.paramKey))
                _task.activeTasks.Add(model.TaskEnd.paramKey, model.TaskEnd.count);
        }

        public void Detect(TaskModel task, int idDetect)
        {
            if (_task.activeTasks.TryGetValue(task.TaskEnd.paramKey, out var v))
            {
                if (v + 1 >= task.TaskEnd.count)
                {
                    _task.activeTasks[task.TaskEnd.paramKey]++;
                    UsesTask.Execute(new TaskStatus { id = idDetect, status = true });
                }
                else
                {
                    _task.activeTasks.Remove(task.TaskEnd.paramKey);
                    // return _Status_End_;
                }
                Save();
            }
        }

        private void Sub()
        {
            UsesTask
                .WhereU(e => _task.activeTasks.Count > e.id && e.id >= 0)
                .Subscribe(e => _task.allViewTask[e.id] = e)
                .AddTo(_disposable);
        }

        private void Save()
        {
            d_task.sceneToTasks[_scene.ThisNameScene] = _task;
            _save.SaveData(d_task);
        }
    }
}
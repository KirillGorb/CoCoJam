using CodeScripts.Dialog.ViewDialog;
using CodeScripts.SaveLoadSystem;
using CodeScripts.TaskSystem;
using UnityEngine;
using Zenject;

namespace CodeScripts.Dialog
{
    public class DialogInstaller : MonoInstaller
    {
        [SerializeField] private DialogUIView viewDialog;
        [SerializeField] private ContainerDialog containerDialog;

        public override void InstallBindings()
        {
            Container.BindInstance(containerDialog).AsSingle();
            Container.BindInstance(viewDialog).AsSingle();

            Container.Bind<LoadTaskSave>().FromNew().AsSingle();
            Container.Bind<Save<DictionaryTaskSD>>().FromNew().AsSingle();
        }
    }
} 
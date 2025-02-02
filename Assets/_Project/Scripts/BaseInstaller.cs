using _Project.Scripts.Player;
using UnityEngine;
using Zenject;

namespace _Project.Scripts
{
    public class BaseInstaller : MonoInstaller
    {
        [SerializeField] private InputModel model;

        public override void InstallBindings()
        {
            Container.Bind<InputModel>().FromInstance(model).AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().FromNew().AsSingle();

            Container.Bind<PlayerMovement>().AsSingle();
        }
    }
}
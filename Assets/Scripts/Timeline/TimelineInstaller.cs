using CodeScripts.SaveLoadSystem;
using CodeScripts.Timeline.Model;
using UnityEngine;
using Zenject;

namespace CodeScripts.Timeline
{
    public class TimelineInstaller : MonoInstaller
    {
        [SerializeField] private GraphModel graphModel;

        public override void InstallBindings()
        {
            Container.BindInstance(graphModel).AsSingle();
            Container.Bind<Save<TimelineSD>>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadProgressTimeline>().FromNew().AsSingle();
        }
    }
}
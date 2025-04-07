using CodeScripts.SaveLoadSystem;
using CodeScripts.Scene;
using CodeScripts.Timeline.Model;
using UnityEngine;
using Zenject;

namespace CodeScripts.Timeline
{
    public class TimelineInstaller : MonoInstaller
    {
        [SerializeField] private TimelineData data;

        public override void InstallBindings()
        {
            Container.BindInstance(data).AsSingle();
            
            Container.Bind<SceneController>().FromNew().AsSingle();
            Container.Bind<Save<TimelineSD>>().FromNew().AsSingle();
            
            Container.BindInterfacesAndSelfTo<LoadProgressTimeline>().FromNew().AsSingle();
        }
    }
}
using Zenject;

namespace CodeScripts.Camera
{
    public class CameraGlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CameraZoomAndMoveController>().FromNew().AsSingle();
        }
    }
}
using Zenject;

namespace CodeScripts.UI.TooltipSystem
{
    public class UITooltipInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TooltipPointer>().FromNew().AsSingle();
        }
    }
}
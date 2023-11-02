using Infrastructure.AssetManagement;
using Infrastructure.SimpleStateMachine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AssetsLoadingService>().AsSingle();
        }
    }
}
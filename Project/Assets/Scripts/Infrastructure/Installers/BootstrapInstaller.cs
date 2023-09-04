using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TestService>().AsSingle().NonLazy();
        }
    }

    public class TestService
    {
        public TestService()
        {
            Debug.LogError("Kek");
        }
    }
}
using Infrastructure.AssetManagement;
using RogueSharp.Factories;
using RogueSharp.Random;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AssetsLoadingService>().AsSingle();
            
            Container.BindFactory<int, IRandom, RandomNumGeneratorFactory>().FromMethod((_, seed) => new DotNetRandom(seed));
        }
    }
}
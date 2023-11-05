using Infrastructure.AssetManagement;
using UI.LoadingScreen;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private LoadingScreen _loadingScreenPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<AssetsLoadingService>().AsSingle();
            Container.Bind<LoadingScreen>().FromComponentInNewPrefab(_loadingScreenPrefab).AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
        }
    }
}
using Infrastructure.AssetManagement;
using Infrastructure.SimpleStateMachine;
using UISystem;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private UIWindowsCollection _uiWindowsCollection;
        [SerializeField] private UIWindowsManager _windowsManagerPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<AssetsLoadingService>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            
            Container.BindInstance(_uiWindowsCollection).AsSingle();
            Container.Bind<UIWindowsFactory>().AsSingle();
            Container.Bind<UIWindowsManager>().FromComponentInNewPrefab(_windowsManagerPrefab).AsSingle();
            
            
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<StatesFactory>().AsSingle();
        }
    }
}
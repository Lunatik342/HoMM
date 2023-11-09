using Cysharp.Threading.Tasks;
using Infrastructure.SimpleStateMachine;
using UI.LoadingScreen;
using UI.MainMenu;
using UISystem;
using UnityEngine;

namespace Infrastructure
{
    public class MainMenuState: IState
    {
        private readonly UIWindowsManager _uiWindowsManager;
        private readonly SceneLoader _sceneLoader;

        public MainMenuState(UIWindowsManager uiWindowsManager, SceneLoader sceneLoader)
        {
            _uiWindowsManager = uiWindowsManager;
            _sceneLoader = sceneLoader;
        }
        
        public async void Enter()
        {
            var loadingScreen = await _uiWindowsManager.OpenWindow<LoadingScreen>();
            await _sceneLoader.LoadMainMenuScene();
            await _uiWindowsManager.OpenWindow<MainMenu>();
            await _uiWindowsManager.CloseWindow(loadingScreen);
        }

        public void Exit()
        {
            _uiWindowsManager.CloseWindow<MainMenu>().Forget(Debug.LogError);
        }
    }
}
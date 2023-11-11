using Cysharp.Threading.Tasks;
using Infrastructure.SimpleStateMachine;
using UI.LoadingScreen;
using UISystem;
using UnityEngine;

namespace Infrastructure
{
    public class BootstrapState: IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly UIWindowsFactory _uiWindowsFactory;
        private readonly UIWindowsManager _uiWindowsManager;

        public BootstrapState(GameStateMachine gameStateMachine, UIWindowsFactory uiWindowsFactory, UIWindowsManager uiWindowsManager)
        {
            _gameStateMachine = gameStateMachine;
            _uiWindowsFactory = uiWindowsFactory;
            _uiWindowsManager = uiWindowsManager;
        }
        
        public void Enter()
        {
            InitializeInitialServices().Forget(Debug.LogError);
        }

        private async UniTask InitializeInitialServices()
        {
            _uiWindowsFactory.Setup();
            await _uiWindowsManager.OpenWindow<LoadingWindow>();
            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
            
        }
    }
}
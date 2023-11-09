using Battle;
using Infrastructure.SimpleStateMachine;
using UI.LoadingScreen;
using UISystem;

namespace Infrastructure
{
    public class BattleState: IPaylodedState<BattleStartParameters>
    {
        private readonly UIWindowsManager _uiWindowsManager;
        private readonly SceneLoader _sceneLoader;

        public BattleState(UIWindowsManager uiWindowsManager, SceneLoader sceneLoader)
        {
            _uiWindowsManager = uiWindowsManager;
            _sceneLoader = sceneLoader;
        }
        
        public async void Enter(BattleStartParameters battleStartParameters)
        {
            var loadingScreen = await _uiWindowsManager.OpenWindow<LoadingScreen>();
            await _sceneLoader.LoadBattleScene(battleStartParameters);
            await _uiWindowsManager.CloseWindow(loadingScreen);
        }

        public void Exit()
        {
            
        }
    }
}
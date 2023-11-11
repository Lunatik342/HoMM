using Battle;
using Infrastructure.SimpleStateMachine;

namespace Infrastructure
{
    public class BattleState: IPaylodedState<BattleStartParameters>
    {
        private readonly SceneLoader _sceneLoader;

        public BattleState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        public async void Enter(BattleStartParameters battleStartParameters)
        {
            await _sceneLoader.LoadBattleScene(battleStartParameters);
        }

        public void Exit()
        {
            
        }
    }
}
using Infrastructure.SimpleStateMachine;
using UISystem;

namespace Infrastructure
{
    public class BootstrapState: IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly UIWindowsFactory _uiWindowsFactory;

        public BootstrapState(GameStateMachine gameStateMachine, UIWindowsFactory uiWindowsFactory)
        {
            _gameStateMachine = gameStateMachine;
            _uiWindowsFactory = uiWindowsFactory;
        }
        
        public void Enter()
        {
            _uiWindowsFactory.Setup();
            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
            
        }
    }
}
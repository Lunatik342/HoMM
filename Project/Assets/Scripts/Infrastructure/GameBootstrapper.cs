using Infrastructure;
using Infrastructure.SimpleStateMachine;
using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    private GameStateMachine _gameStateMachine;
    private StatesFactory _statesFactory;
    
    [Inject]
    public void Construct(GameStateMachine gameStateMachine, StatesFactory statesFactory)
    {
        _gameStateMachine = gameStateMachine;
        _statesFactory = statesFactory;
    }
    
    public void Start()
    {
        _gameStateMachine.RegisterState(_statesFactory.Create<BootstrapState>());
        _gameStateMachine.RegisterState(_statesFactory.Create<MainMenuState>());
        _gameStateMachine.RegisterState(_statesFactory.Create<BattleState>());
        
        _gameStateMachine.Enter<BootstrapState>();
    }
}

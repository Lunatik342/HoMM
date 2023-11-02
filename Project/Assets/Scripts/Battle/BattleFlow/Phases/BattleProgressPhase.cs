using Infrastructure.SimpleStateMachine;

namespace Battle.BattleFlow.Phases
{
    public class BattleProgressPhase: IState
    {
        private readonly BattleTurnsController _battleTurnsController;

        public BattleProgressPhase(BattleTurnsController battleTurnsController)
        {
            _battleTurnsController = battleTurnsController;
        }
        
        public void Enter()
        {
            _battleTurnsController.StartTurns();
        }

        public void Exit()
        {
            
        }
    }
}
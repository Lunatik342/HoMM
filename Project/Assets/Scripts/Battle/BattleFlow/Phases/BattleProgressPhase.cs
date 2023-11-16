using Infrastructure.SimpleStateMachine;

namespace Battle.BattleFlow.Phases
{
    public class BattleProgressPhase: IState
    {
        private readonly BattleFlowController _battleFlowController;

        public BattleProgressPhase(BattleFlowController battleFlowController)
        {
            _battleFlowController = battleFlowController;
        }
        
        public void Enter()
        {
            _battleFlowController.StartBattleFlow();
        }

        public void Exit()
        {
            
        }
    }
}
using Infrastructure.SimpleStateMachine;

namespace Battle.BattleFlow.StateMachine
{
    public class GridViewStateMachine: BasicStateMachine
    {
        public GridViewStateMachine(ControllingUnitViewState controllingUnitViewState, 
            WaitingForCommandProcessViewState waitingForCommandProcessViewState)
        {
            RegisterState(controllingUnitViewState);
            RegisterState(waitingForCommandProcessViewState);
        }
    }
}
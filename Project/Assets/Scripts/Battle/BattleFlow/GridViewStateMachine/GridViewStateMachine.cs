using Infrastructure.SimpleStateMachine;

namespace Battle.BattleFlow.StateMachine
{
    public class GridViewStateMachine: BasicStateMachine
    {
        public GridViewStateMachine(UnitControlViewState unitControlViewState, 
            WaitingForCommandProcessViewState waitingForCommandProcessViewState,
            WaitingForEnemyTurnViewState waitingForEnemyTurnViewState)
        {
            RegisterState(unitControlViewState);
            RegisterState(waitingForCommandProcessViewState);
            RegisterState(waitingForEnemyTurnViewState);
        }
    }
}
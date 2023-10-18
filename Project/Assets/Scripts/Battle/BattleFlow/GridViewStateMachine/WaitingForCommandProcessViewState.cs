using Battle.BattleArena.CellsViews;
using Infrastructure.SimpleStateMachine;

namespace Battle.BattleFlow.StateMachine
{
    public class WaitingForCommandProcessViewState: IState
    {
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;

        public WaitingForCommandProcessViewState(BattleArenaCellsDisplayService cellsDisplayService)
        {
            _cellsDisplayService = cellsDisplayService;
        }
        
        public void Enter()
        {
            _cellsDisplayService.DisplayAllCellsDefault();
        }

        public void Exit()
        {
            
        }
    }
}
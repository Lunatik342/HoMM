using Battle.CellViewsGrid.CellsViews;
using Infrastructure.SimpleStateMachine;

namespace Battle.CellViewsGrid.GridViewStateMachine.States
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
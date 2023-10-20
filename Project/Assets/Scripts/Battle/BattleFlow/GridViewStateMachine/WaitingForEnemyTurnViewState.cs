using Battle.BattleArena.CellsViews;
using Battle.BattleArena.Pathfinding;
using Infrastructure.SimpleStateMachine;
using RogueSharp;

namespace Battle.BattleFlow.StateMachine
{
    public class WaitingForEnemyTurnViewState: IState
    {
        private readonly BattleCellsInputService _cellsInputService;
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly PathfindingService _pathfindingService;
        
        public WaitingForEnemyTurnViewState(BattleCellsInputService cellsInputService, 
            BattleArenaCellsDisplayService cellsDisplayService,
            PathfindingService pathfindingService)
        {
            _cellsInputService = cellsInputService;
            _cellsDisplayService = cellsDisplayService;
            _pathfindingService = pathfindingService;
        }
        
        public void Enter()
        {
            _cellsInputService.SelectedCellChanged += MouseOverCellChanged;
            _cellsInputService.CellRightClicked += OnCellClicked;

            MouseOverCellChanged(_cellsInputService.MouseOverCell);
        }
        
        private void OnCellClicked(Cell clickedCell)
        {
            //Show UI for unit
        }

        private void MouseOverCellChanged(Cell cell)
        {
            _cellsDisplayService.DisplayAllCellsDefault();
            
            if (cell!= null && cell.PlacedUnit != null)
            {
                var reachableCells = _pathfindingService.GetReachableCells(cell.PlacedUnit);
                _cellsDisplayService.DisplayEnemyWalkableCells(reachableCells);
            }
        }

        public void Exit()
        {
            _cellsInputService.SelectedCellChanged -= MouseOverCellChanged;
            _cellsInputService.CellLeftClicked -= OnCellClicked;
        }
    }

}
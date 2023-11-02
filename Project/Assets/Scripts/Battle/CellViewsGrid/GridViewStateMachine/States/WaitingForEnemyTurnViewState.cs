using System.Collections.Generic;
using Algorithms.RogueSharp;
using Battle.CellViewsGrid.CellsViews;
using Battle.Input;
using Infrastructure.SimpleStateMachine;

namespace Battle.CellViewsGrid.GridViewStateMachine.States
{
    public class WaitingForEnemyTurnViewState: IState
    {
        private readonly BattleCellsInputService _cellsInputService;
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;

        public WaitingForEnemyTurnViewState(BattleCellsInputService cellsInputService, 
            BattleArenaCellsDisplayService cellsDisplayService)
        {
            _cellsInputService = cellsInputService;
            _cellsDisplayService = cellsDisplayService;
        }
        
        public void Enter()
        {
            _cellsInputService.MouseoverCellChanged += MouseoverCellChanged;
            MouseoverCellChanged(_cellsInputService.MouseOverCell);
        }

        private void MouseoverCellChanged(Cell cell)
        {
            _cellsDisplayService.DisplayAllCellsDefault();

            var mouseoverUnitReachableCells = cell?.PlacedUnit != null ? 
                cell.PlacedUnit.MovementController.GetReachableCells() : 
                new List<Cell>();
            
            _cellsDisplayService.DisplayReachableCells(mouseoverUnitReachableCells, mouseoverUnitReachableCells);
        }

        public void Exit()
        {
            _cellsInputService.MouseoverCellChanged -= MouseoverCellChanged;
        }
    }

}
using System.Collections.Generic;
using Battle.BattleArena.CellsViews;
using RogueSharp;

namespace Battle.BattleFlow.StateMachine
{
    public abstract class DisplayingCellsViewStateBase
    {
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;

        protected DisplayingCellsViewStateBase(BattleArenaCellsDisplayService cellsDisplayService)
        {
            _cellsDisplayService = cellsDisplayService;
        }

        protected void DisplayReachableCells(Cell mouseoverCell, List<Cell> controlledUnitReachableCells)
        {
            List<Cell> mouseoverUnitReachableCells = null;

            if (mouseoverCell != null && mouseoverCell.PlacedUnit != null)
            {
                mouseoverUnitReachableCells = mouseoverCell.PlacedUnit.MovementController.GetReachableCells();
            }
            else
            {
                mouseoverUnitReachableCells = new List<Cell>();
            }
            
            _cellsDisplayService.DisplayReachableCells(controlledUnitReachableCells, mouseoverUnitReachableCells);
        }
    }
}
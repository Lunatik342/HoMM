using System.Collections.Generic;
using RogueSharp;
using UnityEngine;

namespace Battle.BattleArena.CellsViews
{
    public class BattleArenaCellsDisplayService
    {
        private readonly IBattleArenaCellsViewsHolder _cellsViewsHolder;

        public BattleArenaCellsDisplayService(IBattleArenaCellsViewsHolder cellsViewsHolder)
        {
            _cellsViewsHolder = cellsViewsHolder;
        }

        public void DisplayAllCellsDefault()
        {
            foreach (var cellView in _cellsViewsHolder.CellsViews)
            {
                cellView.PaintCell(CellViewState.Default);
            }
        }

        public void DisplayReachableCells(List<Cell> reachableCells)
        {
            foreach (var reachableCell in reachableCells)
            {
                _cellsViewsHolder.CellsViews[reachableCell.X, reachableCell.Y].PaintCell(CellViewState.Walkable);
            }
        }

        public void DisplayMoveTargetCell(Cell cell)
        {
            _cellsViewsHolder.CellsViews[cell.X, cell.Y].PaintCell(CellViewState.MoveTarget);
        }

        public void DisplayCurrentlyControlledUnitCell(Cell cell)
        {
            _cellsViewsHolder.CellsViews[cell.X, cell.Y].PaintCell(CellViewState.CurrentUnit);
        }
    }
}
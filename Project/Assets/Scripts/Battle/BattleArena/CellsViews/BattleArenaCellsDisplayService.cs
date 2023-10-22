using System.Collections.Generic;
using System.Linq;
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

        public void DisplayReachableCells(List<Cell> reachableCells, List<Cell> enemyReachableCells)
        {
            var intersection = reachableCells.Intersect(enemyReachableCells);
            
            foreach (var cell in reachableCells)
            {
                _cellsViewsHolder.CellsViews[cell.X, cell.Y].PaintCell(CellViewState.Walkable);
            }
            
            foreach (var cell in enemyReachableCells)
            {
                _cellsViewsHolder.CellsViews[cell.X, cell.Y].PaintCell(CellViewState.EnemyWalkable);
            }

            foreach (var cell in intersection)
            {
                _cellsViewsHolder.CellsViews[cell.X, cell.Y].PaintCell(CellViewState.WalkableAndEnemyWalkableIntersection);
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
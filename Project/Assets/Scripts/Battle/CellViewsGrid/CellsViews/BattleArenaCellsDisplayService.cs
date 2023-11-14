using System.Collections.Generic;
using System.Linq;
using Algorithms.RogueSharp;
using Battle.Arena.Map;

namespace Battle.CellViewsGrid.CellsViews
{
    public class BattleArenaCellsDisplayService
    {
        private readonly IBattleArenaCellsViewsHolder _cellsViewsHolder;
        private readonly IMapHolder _mapHolder;

        public BattleArenaCellsDisplayService(IBattleArenaCellsViewsHolder cellsViewsHolder, IMapHolder mapHolder)
        {
            _cellsViewsHolder = cellsViewsHolder;
            _mapHolder = mapHolder;
        }

        public void DisplayAllCellsDefault()
        {
            for (int i = 0; i < _mapHolder.Map.Width; i++)
            {
                for (int j = 0; j < _mapHolder.Map.Height; j++)
                {
                    var cell = _mapHolder.Map[i, j];
                    var cellView = _cellsViewsHolder.CellsViews[i, j];

                    if (!cell.IsFunctioning || cell.IsOccupiedByObstacle)
                    {
                        cellView.PaintCell(CellViewState.Empty);
                    }
                    else
                    {
                        cellView.PaintCell(CellViewState.Default);
                    }
                }
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

        public void DisplayAttackTargetCell(Cell cell)
        {
            _cellsViewsHolder.CellsViews[cell.X, cell.Y].PaintCell(CellViewState.MeleeAttackTarget);
        }

        public void DisplayCurrentlyControlledUnitCell(Cell cell)
        {
            _cellsViewsHolder.CellsViews[cell.X, cell.Y].PaintCell(CellViewState.CurrentUnit);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Algorithms;
using Algorithms.RogueSharp;
using Battle.Arena.Misc;
using Battle.Units;
using UnityEngine;

namespace Battle.Arena.Map
{
    public class PathfindingService
    {
        private readonly IMapHolder _mapHolder;

        public PathfindingService(IMapHolder mapHolder)
        {
            _mapHolder = mapHolder;
        }

        public List<ICell> FindPath(Vector2Int targetPosition, Unit unit)
        {
            var pathfindingMap = _mapHolder.Map;
            var pathFinder = new DijkstraPathFinder(pathfindingMap, BattleArenaConstants.DiagonalMovementCost, unit);
            var path = pathFinder.TryFindShortestPath(unit.PositionProvider.OccupiedCell, pathfindingMap[targetPosition.x, targetPosition.y]);
            return path.Steps.ToList();
        }

        public List<ICell> FindPathForFlyingUnit(Vector2Int targetPosition, Unit unit)
        {
            return new List<ICell>()
            {
                unit.PositionProvider.OccupiedCell,
                _mapHolder.Map.GetCell(targetPosition),
            };
        }

        public List<Cell> GetReachableCells(Unit unit, int travelDistance)
        {
            var occupiedCell = unit.PositionProvider.OccupiedCell;
            var cellsPositions = ReachableCellsFinder<Cell>.GetReachableCells(occupiedCell, _mapHolder.Map, unit, travelDistance);

            List<Cell> reachableCells = new List<Cell>();

            for (int i = 0; i < cellsPositions.Length; i++)
            {
                if (cellsPositions[i])
                {
                    var cell = _mapHolder.Map.CellFor(i);
                    reachableCells.Add(cell);
                }
            }

            reachableCells.Remove(unit.PositionProvider.OccupiedCell);
            return reachableCells;
        }
        
        public List<Cell> GetReachableCellsForFlyingUnit(Unit unit, int travelDistance)
        {
            var occupiedCell = unit.PositionProvider.OccupiedCell;
            var reachableCells = FlyingUnitReachableCellsFinder<Cell>.GetReachableCells(occupiedCell, _mapHolder.Map, unit, travelDistance);
            reachableCells.Remove(unit.PositionProvider.OccupiedCell);
            return reachableCells;
        }
    }
}
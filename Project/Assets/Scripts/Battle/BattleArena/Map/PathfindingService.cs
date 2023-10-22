using System.Collections.Generic;
using RogueSharp;
using RogueSharp.Algorithms;
using UnityEngine;

namespace Battle.BattleArena.Pathfinding
{
    public class PathfindingService
    {
        private readonly IMapHolder _mapHolder;
        
        private const float DiagonalMovementCost = 1.41f;

        public PathfindingService(IMapHolder mapHolder)
        {
            _mapHolder = mapHolder;
        }

        public Path FindPath(Vector2Int targetPosition, Unit unit)
        {
            var pathfindingMap = _mapHolder.Map;
            var pathFinder = new DijkstraPathFinder(pathfindingMap, DiagonalMovementCost, unit);
            var path = pathFinder.TryFindShortestPath(unit.BattleMapPlaceable.OccupiedCell, pathfindingMap[targetPosition.x, targetPosition.y]);
            return path;
        }

        public ICell FindPathForFlyingUnit(Vector2Int targetPosition)
        {
            return _mapHolder.Map[targetPosition.x, targetPosition.y];
        }

        public List<Cell> GetReachableCells(Unit unit)
        {
            var occupiedCell = unit.BattleMapPlaceable.OccupiedCell;
            var reachableCellsFinder = new ReachableCellsFinder<Cell>(DiagonalMovementCost);
            var cellsPositions = reachableCellsFinder.GetReachableCells(occupiedCell, _mapHolder.Map, unit, unit.MovementController.TravelDistance);

            List<Cell> reachableCells = new List<Cell>();

            for (int i = 0; i < cellsPositions.Length; i++)
            {
                if (cellsPositions[i])
                {
                    var cell = _mapHolder.Map.CellFor(i);
                    reachableCells.Add(cell);
                }
            }

            reachableCells.Remove(unit.BattleMapPlaceable.OccupiedCell);
            return reachableCells;
        }
        
        public List<Cell> GetReachableCellsForFlyingUnit(Unit unit)
        {
            var occupiedCell = unit.BattleMapPlaceable.OccupiedCell;
            var reachableCellsFinder = new FlyingUnitReachableCellsFinder<Cell>(DiagonalMovementCost);
            var reachableCells = reachableCellsFinder.GetReachableCells(occupiedCell, _mapHolder.Map, unit, unit.MovementController.TravelDistance);
            reachableCells.Remove(unit.BattleMapPlaceable.OccupiedCell);
            return reachableCells;
        }
    }
}
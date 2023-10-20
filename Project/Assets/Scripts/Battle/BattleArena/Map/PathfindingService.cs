using System.Collections.Generic;
using RogueSharp;
using RogueSharp.Algorithms;
using UnityEngine;

namespace Battle.BattleArena.Pathfinding
{
    public class PathfindingService
    {
        private readonly IMapHolder _mapHolder;

        public PathfindingService(IMapHolder mapHolder)
        {
            _mapHolder = mapHolder;
        }

        public Path FindPath(Vector2Int targetPosition, Unit unit)
        {
            var pathfindingMap = _mapHolder.Map;
            DijkstraPathFinder pathFinder = new DijkstraPathFinder(pathfindingMap, BattleArenaConstants.DiagonalMovementCost, unit);
            var path = pathFinder.TryFindShortestPath(unit.BattleMapPlaceable.OccupiedCell, pathfindingMap[targetPosition.x, targetPosition.y]);

            return path;
        }

        public List<Cell> GetReachableCells(Unit unit)
        {
            var occupiedCell = unit.BattleMapPlaceable.OccupiedCell;
            var reachableCellsFinder = new ReachableCellsFinder<Cell>(BattleArenaConstants.DiagonalMovementCost);
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

            return reachableCells;
        }

        public ICell FindPathForFlyingUnit(Vector2Int targetPosition)
        {
            return _mapHolder.Map[targetPosition.x, targetPosition.y];
        }
    }
}
using Battle.BattleArena.CellsViews;
using RogueSharp;
using UnityEngine;

namespace Battle.BattleArena.Pathfinding
{
    public class PathfindingService
    {
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly IMapHolder _mapHolder;

        public PathfindingService(BattleArenaCellsDisplayService cellsDisplayService, IMapHolder mapHolder)
        {
            _cellsDisplayService = cellsDisplayService;
            _mapHolder = mapHolder;
        }

        public Path FindPath(Vector2Int targetPosition, BattleMapPlaceable targetEntity)
        {
            var pathfindingMap = _mapHolder.Map;
            DijkstraPathFinder pathFinder = new DijkstraPathFinder(pathfindingMap, BattleArenaConstants.DiagonalMovementCost, targetEntity);
            var path = pathFinder.TryFindShortestPath(targetEntity.OccupiedCells[0], pathfindingMap[targetPosition.x, targetPosition.y]);

            return path;
        }

        public ICell FindPathForFlyingUnit(Vector2Int targetPosition)
        {
            return _mapHolder.Map[targetPosition.x, targetPosition.y];
        }
    }
}
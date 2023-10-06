using Battle.BattleArena.CellsViews;
using RogueSharp;
using UnityEngine;

namespace Battle.BattleArena.Pathfinding
{
    public class PathfindingService
    {
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly Map _pathfindingMap;

        public PathfindingService(BattleArenaCellsDisplayService cellsDisplayService, Map pathfindingMap)
        {
            _cellsDisplayService = cellsDisplayService;
            _pathfindingMap = pathfindingMap;
        }

        public Path FindPath(Vector2Int targetPosition, BattleMapPlaceable targetEntity)
        {
            DijkstraPathFinder pathFinder = new DijkstraPathFinder(_pathfindingMap, 1.41f, targetEntity);
            _cellsDisplayService.DisplayBattleField(_pathfindingMap);

            var path = pathFinder.TryFindShortestPath(targetEntity.OccupiedCells[0], _pathfindingMap[targetPosition.x, targetPosition.y]);
            _cellsDisplayService.DisplayPath(path);

            return path;
        }
    }
}
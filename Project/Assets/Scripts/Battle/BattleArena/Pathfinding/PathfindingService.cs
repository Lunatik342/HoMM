using Battle.BattleArena.CellsViews;
using RogueSharp;

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

        public void Test()
        {
            DijkstraPathFinder pathFinder = new DijkstraPathFinder(_pathfindingMap, 1.41f, null);
            _cellsDisplayService.DisplayBattleField(_pathfindingMap);

            var path = pathFinder.TryFindShortestPath(_pathfindingMap[0, 0], _pathfindingMap[11, 9]);
            
            if (path != null)
            {
                _cellsDisplayService.DisplayPath(path);
            }
        }
    }
}
using Battle.BattleArena.Cells;
using RogueSharp;

namespace Battle.BattleArena.Pathfinding
{
    public class PathfindingService
    {
        private readonly BattleArenaCellsDisplayService _cellsDisplayService;
        private readonly PathfindingMapFactory _pathfindingMapFactory;

        public PathfindingService(PathfindingMapFactory pathfindingMapFactory,BattleArenaCellsDisplayService cellsDisplayService)
        {
            _pathfindingMapFactory = pathfindingMapFactory;
            _cellsDisplayService = cellsDisplayService;
        }

        public void Test()
        {
            DijkstraPathFinder pathFinder = new DijkstraPathFinder(_pathfindingMapFactory.PathfindingMap, 1.41f);
            _cellsDisplayService.DisplayBattleField(_pathfindingMapFactory.PathfindingMap);

            var path = pathFinder.TryFindShortestPath(_pathfindingMapFactory.PathfindingMap[0, 0], _pathfindingMapFactory.PathfindingMap[11, 0]);
            
            if (path != null)
            {
                _cellsDisplayService.DisplayPath(path);
            }
        }
    }
}
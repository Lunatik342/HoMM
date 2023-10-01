using Battle.BattleArena.StaticData;
using RogueSharp;

namespace Battle.BattleArena.Pathfinding
{
    public class PathfindingMapFactory
    {
        private readonly BattleArenaStaticDataProvider _battleArenaStaticDataProvider;
        
        private Map _pathfindingMap;

        public Map PathfindingMap => _pathfindingMap;

        public PathfindingMapFactory(BattleArenaStaticDataProvider battleArenaStaticDataProvider)
        {
            _battleArenaStaticDataProvider = battleArenaStaticDataProvider;
        }

        public void CreatePathfindingGrid(BattleArenaId battleArenaId)
        {
            var staticData = _battleArenaStaticDataProvider.ForBattleArena(battleArenaId);
            
            _pathfindingMap = new Map(staticData.Size.x, staticData.Size.y);

            foreach (var cell in _pathfindingMap.GetAllCells())
            {
                cell.IsWalkable = true;
            }
        }

        public void SetCellWalkable(bool isWalkable, int x, int y)
        {
            _pathfindingMap[x, y].IsWalkable = isWalkable;
        }
    }
}

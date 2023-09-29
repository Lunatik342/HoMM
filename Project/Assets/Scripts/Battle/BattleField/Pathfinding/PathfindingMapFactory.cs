using Battle.BattleField.Cells;
using RogueSharp;

namespace Battle.BattleField.Pathfinding
{
    public class PathfindingMapFactory
    {
        private readonly BattleFieldStaticDataService _battleFieldStaticDataService;
        
        private Map _pathfindingMap;

        public Map PathfindingMap => _pathfindingMap;

        public PathfindingMapFactory(BattleFieldStaticDataService battleFieldStaticDataService)
        {
            _battleFieldStaticDataService = battleFieldStaticDataService;
        }

        public void CreatePathfindingGrid(BattleFieldId battleFieldId)
        {
            var staticData = _battleFieldStaticDataService.GetStaticDataForId(battleFieldId);
            
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

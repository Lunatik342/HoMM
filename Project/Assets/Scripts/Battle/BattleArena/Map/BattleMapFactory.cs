using Battle.BattleArena.StaticData;
using RogueSharp;
using Zenject;

namespace Battle.BattleArena.Pathfinding
{
    public class BattleMapFactory: IFactory<BattleArenaId, Map>
    {
        private readonly BattleArenaStaticDataProvider _battleArenaStaticDataProvider;

        private Map _pathfindingMap;

        public BattleMapFactory(BattleArenaStaticDataProvider battleArenaStaticDataProvider)
        {
            _battleArenaStaticDataProvider = battleArenaStaticDataProvider;
        }

        public Map Create(BattleArenaId battleArenaId)
        {
            var staticData = _battleArenaStaticDataProvider.ForBattleArena(battleArenaId);
            
            _pathfindingMap = new Map(staticData.Size.x, staticData.Size.y);

            for (int i = 0; i < staticData.Size.x; i++)
            {
                for (int j = 0; j < staticData.Size.y; j++)
                {
                    var isCellFunctional = !staticData.Layout[i, j];
                    _pathfindingMap[i, j].IsFunctioning = isCellFunctional;
                }
            }

            return _pathfindingMap;
        }
    }
}

using Battle.BattleArena.StaticData;
using RogueSharp;
using Zenject;

namespace Battle.BattleArena.Pathfinding
{
    public class BattleMapFactory: IFactory<Map>
    {
        private readonly BattleArenaStaticDataProvider _battleArenaStaticDataProvider;
        private readonly BattleStartParameters _battleStartParameters;

        private Map _pathfindingMap;

        public BattleMapFactory(BattleArenaStaticDataProvider battleArenaStaticDataProvider, BattleStartParameters battleStartParameters)
        {
            _battleArenaStaticDataProvider = battleArenaStaticDataProvider;
            _battleStartParameters = battleStartParameters;
        }

        public Map Create()
        {
            var staticData = _battleArenaStaticDataProvider.ForBattleArena(_battleStartParameters.BattleArenaId);
            
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

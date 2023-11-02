using Battle.Arena.StaticData;
using Zenject;

namespace Battle.Arena.Map
{
    public class BattleMapFactory: IFactory<BattleArenaId, Algorithms.RogueSharp.Map>
    {
        private readonly BattleArenaStaticDataProvider _battleArenaStaticDataProvider;

        private Algorithms.RogueSharp.Map _pathfindingMap;

        public BattleMapFactory(BattleArenaStaticDataProvider battleArenaStaticDataProvider)
        {
            _battleArenaStaticDataProvider = battleArenaStaticDataProvider;
        }

        public Algorithms.RogueSharp.Map Create(BattleArenaId battleArenaId)
        {
            var staticData = _battleArenaStaticDataProvider.ForBattleArena(battleArenaId);
            
            _pathfindingMap = new Algorithms.RogueSharp.Map(staticData.Size.x, staticData.Size.y);

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

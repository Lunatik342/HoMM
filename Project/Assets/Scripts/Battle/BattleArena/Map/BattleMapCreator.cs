using Battle.BattleArena.StaticData;
using RogueSharp;

namespace Battle.BattleArena.Pathfinding
{
    public class BattleMapCreator: IMapHolder
    {
        private readonly BattleMapFactory _battleMapFactory;
        
        public Map Map { get; private set; }

        public BattleMapCreator(BattleMapFactory battleMapFactory)
        {
            _battleMapFactory = battleMapFactory;
        }

        public void Create(BattleArenaId battleArenaId)
        {
            Map = _battleMapFactory.Create(battleArenaId);
        }
    }
}
using Battle.Arena.StaticData;

namespace Battle.Arena.Map
{
    public class BattleMapCreator: IMapHolder
    {
        private readonly BattleMapFactory _battleMapFactory;
        
        public Algorithms.RogueSharp.Map Map { get; private set; }

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
using Battle.BattleArena.StaticData;

namespace Battle
{
    public class BattleStartParameters
    {
        public BattleArenaId BattleArenaId { get; private set; }

        public BattleStartParameters(BattleArenaId battleArenaId)
        {
            BattleArenaId = battleArenaId;
        }
    }
}
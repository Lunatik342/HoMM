using System.Collections.Generic;

namespace Battle.BattleFlow
{
    public class BattleResultData
    {
        public Team WonTeam { get; }
        public Dictionary<Team, List<UnitsCount>> Casualties { get; }

        public BattleResultData(Team wonTeam, Dictionary<Team, List<UnitsCount>> casualties)
        {
            WonTeam = wonTeam;
            Casualties = casualties;
        }
    }
}
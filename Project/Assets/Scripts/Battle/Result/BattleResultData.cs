using System.Collections.Generic;
using Utilities.UsefullClasses;

namespace Battle.Result
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
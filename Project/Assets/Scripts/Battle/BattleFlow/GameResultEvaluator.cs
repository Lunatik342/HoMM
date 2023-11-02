using System.Collections.Generic;
using System.Linq;
using Battle.Units;

namespace Battle.BattleFlow
{
    public class GameResultEvaluator
    {
        private readonly IUnitsHolder _unitsHolder;
        
        private List<Team> _participatingTeams  = new();

        public GameResultEvaluator(IUnitsHolder unitsHolder)
        {
            _unitsHolder = unitsHolder;
        }

        public void SetParticipatingTeams(Dictionary<Team, List<UnitCreationParameter>> startingUnits)
        {
            foreach (var teamStartingUnit in startingUnits)
            {
                _participatingTeams.Add(teamStartingUnit.Key);
            }
        }
        
        public bool IsGameOver(out Team winningTeam)
        {
            foreach (var participatingTeam in _participatingTeams)
            {
                var aliveUnitsCount = _unitsHolder.GetAllUnitsOfTeam(participatingTeam).Count(u => u.Health.IsAlive);

                if (aliveUnitsCount == 0)
                {
                    winningTeam = GetOppositeTeamTo(participatingTeam);
                    return true;
                }
            }

            winningTeam = Team.TeamLeft;
            return false;
        }

        private Team GetOppositeTeamTo(Team team)
        {
            return team == Team.TeamLeft ? Team.TeamRight : Team.TeamLeft;
        }
    }
}
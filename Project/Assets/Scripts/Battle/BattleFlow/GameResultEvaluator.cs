using System.Collections.Generic;
using System.Linq;
using Battle.Units.Creation;

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
                var allTeamUnitsDead = _unitsHolder.GetAllUnitsOfTeam(participatingTeam).All(u => !u.Health.IsAlive);

                if (allTeamUnitsDead)
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
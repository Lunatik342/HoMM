using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Units;
using Battle.Units.Creation;
using Battle.Units.StaticData;

namespace Battle.BattleFlow
{
    public class GameResultEvaluator
    {
        private readonly IUnitsHolder _unitsHolder;
        private readonly List<Team> _participatingTeams = new();

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
        
        public bool IsGameOver(out Team wonTeam)
        {
            foreach (var participatingTeam in _participatingTeams)
            {
                var allTeamUnitsDead = _unitsHolder.GetAllUnitsOfTeam(participatingTeam).All(u => !u.Health.IsAlive);

                if (allTeamUnitsDead)
                {
                    wonTeam = participatingTeam.GetOppositeTeam();
                    return true;
                }
            }

            wonTeam = Team.TeamLeft;
            return false;
        }

        public BattleResultData CalculateBattleResultData(BattleStartParameters battleStartParameters)
        {
            if (!IsGameOver(out var wonTeam))
            {
                throw new InvalidOperationException("Cannot calculate won team when game is not over");
            }
            
            var casualties = GetCasualties(battleStartParameters);
            var battleResultData = new BattleResultData(wonTeam, casualties);
            return battleResultData;
        }

        private Dictionary<Team, List<UnitsCount>> GetCasualties(BattleStartParameters battleStartParameters)
        {
            Dictionary<Team, List<UnitsCount>> casualties = new Dictionary<Team, List<UnitsCount>>();

            foreach (var startingUnitsOfTeam in battleStartParameters.StartingUnits)
            {
                var team = startingUnitsOfTeam.Key;
                var casualtiesOfTeam = new List<UnitsCount>();
                var unitsInBattle = _unitsHolder.GetAllUnitsOfTeam(team);

                FillTeamCasualties(unitsInBattle, startingUnitsOfTeam, casualtiesOfTeam);

                casualties[team] = casualtiesOfTeam;
            }

            return casualties;
        }

        private static void FillTeamCasualties(
            List<Unit> unitsInBattle,
            KeyValuePair<Team, List<UnitCreationParameter>> startingUnitsOfTeam,
            List<UnitsCount> casualtiesOfTeam)
        {
            var precessedUnitsId = new List<UnitId>();
            
            foreach (var unitInBattle in unitsInBattle)
            {
                var currentUnitId = unitInBattle.UnitId;

                if (precessedUnitsId.Contains(currentUnitId))
                {
                    continue;
                }

                var unitsDiedCount = CalculateCasualtiesForUnitId(startingUnitsOfTeam.Value, unitsInBattle, currentUnitId);

                if (unitsDiedCount != 0)
                {
                    casualtiesOfTeam.Add(new UnitsCount(currentUnitId, unitsDiedCount));
                }

                precessedUnitsId.Add(unitInBattle.UnitId);
            }
        }

        private static int CalculateCasualtiesForUnitId(
            List<UnitCreationParameter> startingUnits, 
            List<Unit> unitsAfterBattle,
            UnitId currentUnitId)
        {
            var startingUnitsCount = startingUnits.Where(u => u.UnitId == currentUnitId).Sum(u => u.Count);
            var aliveUnitsCount = unitsAfterBattle.Where(u => u.UnitId == currentUnitId).Sum(u => u.Health.AliveUnitsCount);
            var unitsDiesCount = startingUnitsCount - aliveUnitsCount;
            return unitsDiesCount;
        }
    }

    public class UnitsCount
    {
        public UnitId UnitId { get; }
        public int Count { get; }

        public UnitsCount(UnitId unitId, int count)
        {
            UnitId = unitId;
            Count = count;
        }
    }
}
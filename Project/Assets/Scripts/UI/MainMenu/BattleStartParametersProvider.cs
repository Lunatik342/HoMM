using System;
using System.Collections.Generic;
using Battle;
using Battle.Arena.StaticData;
using Battle.UnitCommands.Providers;
using Battle.Units.StaticData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.MainMenu
{
    public class BattleStartParametersProvider
    {
        private readonly Dictionary<UnitId, int> _weeklyGrowth = new()
        {
            { UnitId.Peasant, 22 },
            { UnitId.Archer, 12 },
            { UnitId.Footman, 10 },
            { UnitId.Swordsman, 5 },
            { UnitId.Priest, 3 },
            { UnitId.Cavalier, 2 },
            { UnitId.King, 1 },
        };

        private readonly int[] _startingYPositions = { 0, 2, 3, 5, 6, 8, 9 };

        public BattleStartParameters GetDefault(int weeksCount, float difficultyModifier)
        {
            var leftTeamUnits = GetUnits(0, weeksCount, 1);
            var rightTeamUnits = GetUnits(11, weeksCount, difficultyModifier);
            
            var unitsToSpawn = new Dictionary<Team, List<UnitCreationParameter>>()
            {
                { Team.TeamLeft, leftTeamUnits },
                { Team.TeamRight, rightTeamUnits }
            };

            var obstacleGenerationParameters = new ObstacleGenerationParameters
            {
                IsRandom = true,
                RandomSeed = Random.Range(0, Int32.MaxValue)
            };

            var controlParameters = new Dictionary<Team, CommandProviderType>
            {
                { Team.TeamLeft, CommandProviderType.PlayerControlled },
                { Team.TeamRight, CommandProviderType.AIControlled },
            };

            var testBattleStartParameters = new BattleStartParameters(BattleArenaId.Forge, obstacleGenerationParameters, unitsToSpawn,controlParameters);
            return testBattleStartParameters;
        }

        public BattleStartParameters GetEasyFunBattleParameters()
        {
            var leftTeamUnits = new List<UnitCreationParameter>()
            {
                new UnitCreationParameter(new Vector2Int(1, 4), UnitId.King, 1),
                new UnitCreationParameter(new Vector2Int(1, 7), UnitId.Cavalier, 1),
            };
            
            var rightTeamUnits = new List<UnitCreationParameter>()
            {
                new UnitCreationParameter(new Vector2Int(11, 0), UnitId.Peasant, 40),
                new UnitCreationParameter(new Vector2Int(11, 3), UnitId.Archer, 15),
                new UnitCreationParameter(new Vector2Int(11, 6), UnitId.Footman, 12),
                new UnitCreationParameter(new Vector2Int(11, 9), UnitId.Swordsman, 3),
            };
            
            var unitsToSpawn = new Dictionary<Team, List<UnitCreationParameter>>()
            {
                { Team.TeamLeft, leftTeamUnits },
                { Team.TeamRight, rightTeamUnits }
            };

            var obstacleGenerationParameters = new ObstacleGenerationParameters
            {
                IsRandom = true,
                RandomSeed = Random.Range(0, Int32.MaxValue)
            };

            var controlParameters = new Dictionary<Team, CommandProviderType>
            {
                { Team.TeamLeft, CommandProviderType.PlayerControlled },
                { Team.TeamRight, CommandProviderType.AIControlled },
            };

            var testBattleStartParameters = new BattleStartParameters(BattleArenaId.Forge, obstacleGenerationParameters, unitsToSpawn,controlParameters);
            return testBattleStartParameters;
        }

        private List<UnitCreationParameter> GetUnits(int xPosition, int weeksCount, float countMultiplier)
        {
            float randomSpread = 0.1f;
            var result = new List<UnitCreationParameter>();

            int i = 0;

            foreach (var weeklyGrowForUnit in _weeklyGrowth)
            {
                var unitId = weeklyGrowForUnit.Key;
                var unitsCount = weeklyGrowForUnit.Value * weeksCount * countMultiplier;
                var randomizedUnitsCount = (int) Math.Round( Random.Range(unitsCount * (1 - randomSpread), unitsCount * (1 + randomSpread)));

                result.Add(new UnitCreationParameter(new Vector2Int(xPosition, _startingYPositions[i]), 
                    unitId, randomizedUnitsCount));
                i++;
            }

            return result;
        }
    }
}
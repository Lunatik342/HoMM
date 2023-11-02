using System.Collections.Generic;
using Battle.Arena.Obstacles;
using Battle.Arena.StaticData;
using Battle.BattleFlow;
using Battle.UnitCommands.Providers;
using Battle.Units.StaticData;
using UnityEngine;

namespace Battle
{
    public class BattleStartParameters
    {
        public BattleArenaId BattleArenaId { get; private set; }
        public ObstacleGenerationParameters ObstacleGenerationParameters { get; private set; }
        
        public Dictionary<Team, List<UnitCreationParameter>> StartingUnits { get; private set; }
        public Dictionary<Team, CommandProviderType> CommandProvidersForTeams { get; private set; }

        public BattleStartParameters(BattleArenaId battleArenaId, 
            ObstacleGenerationParameters obstacleGenerationParameters,
            Dictionary<Team, List<UnitCreationParameter>> startingUnits, 
            Dictionary<Team, CommandProviderType> commandProvidersForTeams)
        {
            BattleArenaId = battleArenaId;
            ObstacleGenerationParameters = obstacleGenerationParameters;
            StartingUnits = startingUnits;
            CommandProvidersForTeams = commandProvidersForTeams;
        }
    }

    public class ObstacleGenerationParameters
    {
        public bool IsRandom { get; set; }
        public int RandomSeed { get; set; }
        public List<ObstacleOnGridParameters> DeterminedObstacleParameters { get; set; }
    }

    public class ObstacleOnGridParameters
    {
        public ObstacleId ObstacleId { get; private set; }
        public ObstaclesSpawner.ObstacleRotationAngle Rotation { get; private set; }
        public Vector2Int Position { get; private set; }

        public ObstacleOnGridParameters(ObstacleId obstacleId, ObstaclesSpawner.ObstacleRotationAngle rotation, Vector2Int position)
        {
            ObstacleId = obstacleId;
            Rotation = rotation;
            Position = position;
        }
    }

    public class UnitCreationParameter
    {
        public UnitId UnitId { get; private set; }
        public Vector2Int Position { get; private set; }
        public int Count { get; private set;}

        public UnitCreationParameter(Vector2Int position, UnitId unitId, int count)
        {
            Position = position;
            UnitId = unitId;
            Count = count;
        }
    }
}
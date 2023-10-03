using System.Collections.Generic;
using Battle.BattleArena;
using Battle.BattleArena.StaticData;
using UnityEngine;

namespace Battle
{
    public class BattleStartParameters
    {
        public BattleArenaId BattleArenaId { get; private set; }
        public ObstacleGenerationParameters ObstacleGenerationParameters { get; private set; }

        public BattleStartParameters(BattleArenaId battleArenaId, ObstacleGenerationParameters obstacleGenerationParameters)
        {
            BattleArenaId = battleArenaId;
            ObstacleGenerationParameters = obstacleGenerationParameters;
        }
    }

    public class ObstacleGenerationParameters
    {
        public bool IsRandom { get; set; }
        public int RandomSeed { get; set; }
        public List<DeterminedObstacleParameters> DeterminedObstacleParameters { get; set; }
    }

    public class DeterminedObstacleParameters
    {
        public ObstacleId ObstacleId { get; private set; }
        public ObstaclesSpawner.ObstacleRotationAngle Rotation { get; private set; }
        public Vector2Int Position { get; private set; }

        public DeterminedObstacleParameters(ObstacleId obstacleId, ObstaclesSpawner.ObstacleRotationAngle rotation, Vector2Int position)
        {
            ObstacleId = obstacleId;
            Rotation = rotation;
            Position = position;
        }
    }
}
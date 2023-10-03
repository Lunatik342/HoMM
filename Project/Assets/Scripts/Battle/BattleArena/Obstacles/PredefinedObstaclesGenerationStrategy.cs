using System.Collections.Generic;
using System.Linq;
using Battle.BattleArena.StaticData;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Obstacles
{
    public class PredefinedObstaclesGenerationStrategy: IObstaclesGenerationStrategy
    {
        private readonly BattleStartParameters _battleStartParameters;
        private readonly BattleArenaStaticDataProvider _staticDataProvider;

        public PredefinedObstaclesGenerationStrategy(BattleStartParameters battleStartParameters, BattleArenaStaticDataProvider staticDataProvider)
        {
            _battleStartParameters = battleStartParameters;
            _staticDataProvider = staticDataProvider;
        }
        
        public IEnumerable<(ObstacleStaticData, ObstaclesSpawner.ObstacleRotationAngle)> GetObstacles()
        {
            return _battleStartParameters.ObstacleGenerationParameters.DeterminedObstacleParameters
                .Select(t => (_staticDataProvider.ForObstacle(t.ObstacleId), t.Rotation));
        }

        public Vector2Int GetPositionForObstacle(int obstacleIndex, bool[,] layout)
        {
            return _battleStartParameters.ObstacleGenerationParameters.DeterminedObstacleParameters[obstacleIndex].Position;
        }
    }
}
using System.Collections.Generic;
using Battle.BattleArena.StaticData;
using UnityEngine;

namespace Battle.BattleArena.Obstacles
{
    public interface IObstaclesGenerationStrategy
    {
        public IEnumerable<(ObstacleStaticData, ObstaclesSpawner.ObstacleRotationAngle)> GetObstacles();
        public Vector2Int GetPositionForObstacle(int obstacleIndex, bool[,] layout);
    }
}
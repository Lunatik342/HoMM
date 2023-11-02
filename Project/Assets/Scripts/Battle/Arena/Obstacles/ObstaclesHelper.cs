using System;
using Battle.Arena.StaticData;
using Utilities;

namespace Battle.Arena.Obstacles
{
    public class ObstaclesHelper
    {
        public static bool[,] RotateObstacle(ObstaclesSpawner.ObstacleRotationAngle rotationAngle, ObstacleStaticData obstacleStaticData)
        {
            var sourceLayout = obstacleStaticData.GetLayout();
            bool[,] rotatedLayout;
            
            switch (rotationAngle)
            {
                case ObstaclesSpawner.ObstacleRotationAngle.Degrees0:
                    rotatedLayout = sourceLayout;
                    break;
                case ObstaclesSpawner.ObstacleRotationAngle.Degrees90:
                    rotatedLayout = sourceLayout.TurnBy90Degrees();
                    break;
                case ObstaclesSpawner.ObstacleRotationAngle.Degrees180:
                    rotatedLayout = sourceLayout.TurnBy180Degrees();
                    break;
                case ObstaclesSpawner.ObstacleRotationAngle.Degrees270:
                    rotatedLayout = sourceLayout.TurnBy270Degrees();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotationAngle), rotationAngle, null);
            }

            return rotatedLayout;
        }
    }
}
using System;
using System.Collections.Generic;
using Battle.BattleArena.Obstacles;
using Battle.BattleArena.StaticData;
using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using RogueSharp;
using UnityEngine;
using Utilities;

namespace Battle.BattleArena
{
    public class ObstaclesSpawner
    {
        private readonly Map _pathfindingMap;
        private readonly AssetsLoadingService _assetsLoadingService;
        private readonly IObstaclesGenerationStrategy _obstaclesGenerationStrategy;
        private readonly BattleArenaStaticDataProvider _staticDataProvider;

        private List<GameObject> _spawnedObstacles = new();

        public ObstaclesSpawner(
            Map pathfindingMap,
            AssetsLoadingService assetsLoadingService,
            IObstaclesGenerationStrategy obstaclesGenerationStrategy,
            BattleArenaStaticDataProvider staticDataProvider)
        {
            _pathfindingMap = pathfindingMap;
            _assetsLoadingService = assetsLoadingService;
            _obstaclesGenerationStrategy = obstaclesGenerationStrategy;
            _staticDataProvider = staticDataProvider;
        }

        public async UniTask SpawnObstacles()
        {
            var generatedObstaclesData = _obstaclesGenerationStrategy.GetObstacles();

            List<UniTask> obstaclesCreationTasks = new List<UniTask>();

            foreach ((ObstacleStaticData staticData, ObstacleRotationAngle angle) generatedObstacle in generatedObstaclesData)
            {
                obstaclesCreationTasks.Add(GenerateObstacle(generatedObstacle.staticData, generatedObstacle.angle));
            }

            await UniTask.WhenAll(obstaclesCreationTasks);
        }

        private async UniTask GenerateObstacle(ObstacleStaticData obstacleStaticData, ObstacleRotationAngle rotationAngle)
        {
            var rotatedObstacleLayout = RotateObstacle(rotationAngle, obstacleStaticData);

            //Tries to place an obstacle n times, gives up if not successful (good enough for random generation)
            for (int i = 0; i < _staticDataProvider.ObstaclesGenerationRules.MaxTriesToPlaceObstacleBeforeGivingUp; i++)
            {
                var gridPosition = _obstaclesGenerationStrategy.GetPositionForObstacle(i, rotatedObstacleLayout);
                var placed = TryPlacingObstacle(rotatedObstacleLayout, gridPosition);

                if (placed)
                {
                    _spawnedObstacles.Add((await InstantiateView(obstacleStaticData, rotationAngle, gridPosition, rotatedObstacleLayout)).gameObject);
                    break;
                }
            }

            await UniTask.CompletedTask;
        }

        private UniTask<Transform> InstantiateView(ObstacleStaticData obstacleStaticData, ObstacleRotationAngle rotationAngle,
            Vector2Int gridPosition, bool[,] rotatedObstacleLayout)
        {
            var obstacleCenter = new Vector2(
                (2 * gridPosition.x + rotatedObstacleLayout.GetLength(0) - 1) / 2f,
                (2 * gridPosition.y + rotatedObstacleLayout.GetLength(1) - 1) / 2f);

            return _assetsLoadingService.InstantiateAsync<Transform>(
                obstacleStaticData.ViewPrefabReference,
                obstacleCenter.ToBattleArenaWorldPosition(),
                Quaternion.Euler(0, (int)rotationAngle, 0), null);
        }

        private bool TryPlacingObstacle(bool[,] obstacleLayout, Vector2Int gridPosition)
        {
            bool hasEnoughSpaceOnPosition = true;
            
            for (int i = 0; i < obstacleLayout.GetLength(0); i++)
            {
                for (int j = 0; j < obstacleLayout.GetLength(1); j++)
                {
                    var occupiesCell = obstacleLayout[i, j];
                    
                    if (!occupiesCell)
                    {
                        continue;
                    }

                    var gridCell = _pathfindingMap[gridPosition.x + i, gridPosition.y + j];
                    hasEnoughSpaceOnPosition &= (gridCell.IsFunctioning && !gridCell.IsOccupiedByObstacle && !gridCell.IsOccupiedByEntity);
                }
            }

            if (hasEnoughSpaceOnPosition)
            {
                for (int i = 0; i < obstacleLayout.GetLength(0); i++)
                {
                    for (int j = 0; j < obstacleLayout.GetLength(1); j++)
                    {
                        var occupiesCell = obstacleLayout[i, j];
                    
                        if (!occupiesCell)
                        {
                            continue;
                        }

                        var gridCell = _pathfindingMap[gridPosition.x + i, gridPosition.y + j];
                        gridCell.IsOccupiedByObstacle = true;
                    }
                }
            }
            
            return hasEnoughSpaceOnPosition;
        }

        private bool[,] RotateObstacle(ObstacleRotationAngle rotationAngle, ObstacleStaticData obstacleStaticData)
        {
            var sourceLayout = obstacleStaticData.GetLayout();
            bool[,] rotatedLayout;
            
            switch (rotationAngle)
            {
                case ObstacleRotationAngle.Degrees0:
                    rotatedLayout = sourceLayout;
                    break;
                case ObstacleRotationAngle.Degrees90:
                    rotatedLayout = sourceLayout.TurnBy90Degrees();
                    break;
                case ObstacleRotationAngle.Degrees180:
                    rotatedLayout = sourceLayout.TurnBy180Degrees();
                    break;
                case ObstacleRotationAngle.Degrees270:
                    rotatedLayout = sourceLayout.TurnBy270Degrees();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotationAngle), rotationAngle, null);
            }

            return rotatedLayout;
        }

        public enum ObstacleRotationAngle
        {
            Degrees0 = 0,
            Degrees90 = 90,
            Degrees180 = 180,
            Degrees270 = 270,
        }
    }
}
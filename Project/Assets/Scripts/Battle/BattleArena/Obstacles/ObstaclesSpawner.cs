using System;
using System.Collections.Generic;
using Battle.BattleArena.Obstacles;
using Battle.BattleArena.Pathfinding;
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
        private readonly AssetsLoadingService _assetsLoadingService;
        private readonly BattleArenaStaticDataProvider _staticDataProvider;
        private readonly IMapHolder _mapHolder;

        private List<GameObject> _spawnedObstacles = new();

        public ObstaclesSpawner(AssetsLoadingService assetsLoadingService,
            BattleArenaStaticDataProvider staticDataProvider,
            IMapHolder mapHolder)
        {
            _assetsLoadingService = assetsLoadingService;
            _staticDataProvider = staticDataProvider;
            _mapHolder = mapHolder;
        }

        public async UniTask Spawn(IObstaclesGenerationStrategy generationStrategy)
        {
            var generatedObstaclesData = generationStrategy.GetObstacles();

            List<UniTask> obstaclesCreationTasks = new List<UniTask>();

            foreach (var obstacleParameters in generatedObstaclesData)
            {
                obstaclesCreationTasks.Add(GenerateObstacle(obstacleParameters));
            }

            await UniTask.WhenAll(obstaclesCreationTasks);
        }

        private async UniTask GenerateObstacle(ObstacleCreationParameters obstacleParameters)
        {
            var obstacleStaticData = _staticDataProvider.ForObstacle(obstacleParameters.ObstacleId);
            var rotatedObstacleLayout = ObstaclesHelper.RotateObstacle(obstacleParameters.Rotation, obstacleStaticData);
            
            PlaceObstacle(rotatedObstacleLayout, obstacleParameters.Position);
            
            _spawnedObstacles.Add((await InstantiateView(obstacleStaticData, 
                obstacleParameters.Rotation, obstacleParameters.Position, rotatedObstacleLayout)).gameObject);
        }

        private UniTask<Transform> InstantiateView(ObstacleStaticData obstacleStaticData, ObstacleRotationAngle rotationAngle,
            Vector2Int gridPosition, bool[,] rotatedObstacleLayout)
        {
            var obstacleCenterPosition = TwoDimensionalArrayUtilities.GetWorldPositionFor(gridPosition,
                rotatedObstacleLayout.GetLength(0), rotatedObstacleLayout.GetLength(1));

            return _assetsLoadingService.InstantiateAsync<Transform>(obstacleStaticData.ViewPrefabReference,
                obstacleCenterPosition, Quaternion.Euler(0, (int)rotationAngle, 0), null);
        }

        private void PlaceObstacle(bool[,] obstacleLayout, Vector2Int gridPosition)
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

                    var gridCell = _mapHolder.Map[gridPosition.x + i, gridPosition.y + j];
                    gridCell.IsOccupiedByObstacle = true;
                }
            }
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
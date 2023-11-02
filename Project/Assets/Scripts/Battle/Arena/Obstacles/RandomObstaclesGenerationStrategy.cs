using System.Collections.Generic;
using System.Linq;
using Algorithms.RogueSharp.Random;
using Battle.Arena.Map;
using Battle.Arena.Misc;
using Battle.Arena.StaticData;
using UnityEngine;
using Utilities;

namespace Battle.Arena.Obstacles
{
    public class RandomObstaclesGenerationStrategy: IObstaclesGenerationStrategy
    {
        private readonly IMapHolder _mapHolder;
        private readonly BattleArenaStaticDataProvider _staticDataProvider;
        private readonly BattleArenaId _battleArenaId;
        private readonly IRandom _randomGenerator;
        
        public RandomObstaclesGenerationStrategy(
            IMapHolder mapHolder,
            BattleArenaStaticDataProvider staticDataProvider,
            ObstacleGenerationParameters generationParameters,
            BattleArenaId battleArenaId)
        {
            _mapHolder = mapHolder;
            _staticDataProvider = staticDataProvider;
            _battleArenaId = battleArenaId;
            _randomGenerator = new DotNetRandom(generationParameters.RandomSeed);
        }
        
        public IEnumerable<ObstacleOnGridParameters> GetObstacles()
        {
            var obstacles = _staticDataProvider.GetObstaclesForBattleArena(_battleArenaId).ToList();

            if (obstacles.Count == 0)
            {
                yield break;
            }
            
            var generationRules = _staticDataProvider.ObstaclesGenerationRules;
            var obstaclesWeightPool = CreateWeightedPoolForObstacles(obstacles, _randomGenerator, generationRules);
            var countWeightPool = CreateWeightedPoolForCount(_randomGenerator, generationRules);
            var currentOccupiedCells = 0;
            var randomObstaclesCount = countWeightPool.Choose();

            for (int i = 0; i < randomObstaclesCount; i++)
            {
                var randomObstacle = GetRandomObstacle(obstaclesWeightPool, _randomGenerator);
                var occupiedCellsByObstacle = randomObstacle.GetOccupiedCellsCount();

                if (currentOccupiedCells + occupiedCellsByObstacle > generationRules.MaximumOccupiedSpaceByObstacles)
                {
                    yield break;
                }
                
                var randomRotation = RandomUtilities.RandomEnumValue<ObstaclesSpawner.ObstacleRotationAngle>(_randomGenerator);
                var rotatedObstacle = ObstaclesHelper.RotateObstacle(randomRotation, randomObstacle);

                //Tries to place an obstacle n times, gives up if not successful (good enough for random generation)
                for (int j = 0; j < generationRules.MaxTriesToPlaceObstacleBeforeGivingUp; j++)
                {
                    var randomPosition = GetPositionForObstacle(rotatedObstacle);
                    if (HasEnoughSpaceOnPosition(rotatedObstacle, randomPosition))
                    {
                        yield return new ObstacleOnGridParameters(randomObstacle.Id, randomRotation, randomPosition);
                        break;
                    }
                }
            }
        }

        private Vector2Int GetPositionForObstacle(bool[,] layout)
        {
            var minXPosition = 0 + BattleArenaConstants.TroopsArrangementFieldWidth;
            var maxXPosition = (_mapHolder.Map.Width - 1) - (layout.GetLength(0) - 1) - BattleArenaConstants.TroopsArrangementFieldWidth;

            var minYPosition = 0;
            var maxYPosition = (_mapHolder.Map.Height - 1) - (layout.GetLength(1) - 1);
            
            return new Vector2Int(_randomGenerator.Next(minXPosition, maxXPosition), _randomGenerator.Next(minYPosition, maxYPosition));
        }

        private static IWeightedPool<List<ObstacleStaticData>> CreateWeightedPoolForObstacles(
            List<ObstacleStaticData> obstacles, 
            IRandom randomGenerator,
            ObstaclesGenerationStaticData generationRules)
        {
            var sameSizeObstacles = new Dictionary<int, List<ObstacleStaticData>>();

            foreach (var obstacle in obstacles)
            {
                var occupiedSpace = obstacle.GetOccupiedCellsCount();

                if (sameSizeObstacles.TryGetValue(occupiedSpace, out var obstacleOfSameOccupiedSpace))
                {
                    obstacleOfSameOccupiedSpace.Add(obstacle);
                }
                else
                {
                    sameSizeObstacles[occupiedSpace] = new List<ObstacleStaticData> { obstacle };
                }
            }

            var obstaclesWeightPool = new WeightedPool<List<ObstacleStaticData>>(randomGenerator, l => l);

            foreach (var sameSizeObstaclePair in sameSizeObstacles)
            {
                if (generationRules.ObstaclesSizeWeights.TryGetValue(sameSizeObstaclePair.Key, out var weight))
                {
                    obstaclesWeightPool.Add(sameSizeObstaclePair.Value, weight);
                }
                else
                {
                    Debug.LogError($"No obstacle weight in static data for size: {sameSizeObstaclePair.Value}");
                    var fallbackWeight = 5;
                    obstaclesWeightPool.Add(sameSizeObstaclePair.Value, fallbackWeight);
                }
            }

            return obstaclesWeightPool;
        }

        private static IWeightedPool<int> CreateWeightedPoolForCount(IRandom randomGenerator, ObstaclesGenerationStaticData generationRules)
        {
            var countWeightPool = new WeightedPool<int>(randomGenerator, i => i);

            foreach (var countWeight in generationRules.ObstaclesCountWeights)
            {
                countWeightPool.Add(countWeight.Key, countWeight.Value);
            }

            return countWeightPool;
        }

        private static ObstacleStaticData GetRandomObstacle(IWeightedPool<List<ObstacleStaticData>> obstaclesWeightPool, IRandom randomGenerator)
        {
            return obstaclesWeightPool.Choose().GetRandomItem(randomGenerator);
        }
        
        private bool HasEnoughSpaceOnPosition(bool[,] obstacleLayout, Vector2Int gridPosition)
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

                    var gridCell = _mapHolder.Map[gridPosition.x + i, gridPosition.y + j];
                    hasEnoughSpaceOnPosition &= gridCell.IsFunctioning && !gridCell.IsOccupiedByObstacle && !gridCell.IsOccupiedByEntity;
                }
            }

            return hasEnoughSpaceOnPosition;
        }
    }
}
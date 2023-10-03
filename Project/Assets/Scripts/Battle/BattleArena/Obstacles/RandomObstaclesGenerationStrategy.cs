using System.Collections.Generic;
using System.Linq;
using Battle.BattleArena.StaticData;
using RogueSharp;
using RogueSharp.Factories;
using RogueSharp.Random;
using UnityEngine;
using Utilities;

namespace Battle.BattleArena.Obstacles
{
    public class RandomObstaclesGenerationStrategy: IObstaclesGenerationStrategy
    {
        private readonly Map _pathfindingMap;
        private readonly BattleStartParameters _battleStartParameters;
        private readonly BattleArenaStaticDataProvider _staticDataProvider;
        private readonly IRandom _randomGenerator;
        
        public RandomObstaclesGenerationStrategy(
            Map pathfindingMap, 
            BattleStartParameters battleStartParameters, 
            BattleArenaStaticDataProvider staticDataProvider,
            RandomNumGeneratorFactory randomNumGeneratorFactory)
        {
            _pathfindingMap = pathfindingMap;
            _battleStartParameters = battleStartParameters;
            _staticDataProvider = staticDataProvider;
            _randomGenerator = randomNumGeneratorFactory.Create(_battleStartParameters.ObstacleGenerationParameters.RandomSeed);
        }
        
        public IEnumerable<(ObstacleStaticData, ObstaclesSpawner.ObstacleRotationAngle)> GetObstacles()
        {
            var obstacles = _staticDataProvider.GetObstaclesForBattleArena(_battleStartParameters.BattleArenaId).ToList();

            if (obstacles.Count == 0)
            {
                yield break;
            }
            
            var generationRules = _staticDataProvider.ObstaclesGenerationRules;

            var obstaclesWeightPool = CreateWeightedPoolForObstacles(obstacles, _randomGenerator, generationRules);
            var countWeightPool = CreateWeightedPoolForCount(_randomGenerator, generationRules);

            var currentOccupiedCells = 0;

            var randomObstaclesCount = countWeightPool.Choose();
            var randomObstacles = GetRandomObstacles(randomObstaclesCount, obstaclesWeightPool, _randomGenerator);

            foreach (var randomObstacle in randomObstacles)
            {
                var occupiedCellsByObstacle = randomObstacle.GetOccupiedCellsCount();

                if (currentOccupiedCells + occupiedCellsByObstacle > generationRules.MaxTriesToPlaceObstacleBeforeGivingUp)
                {
                    break;
                }

                var randomTurn = RandomUtilities.RandomEnumValue<ObstaclesSpawner.ObstacleRotationAngle>(_randomGenerator);

                yield return (randomObstacle, randomTurn);
            }
        }

        public Vector2Int GetPositionForObstacle(int obstacleIndex, bool[,] layout)
        {
            var minXPosition = 0 + BattleArenaConstants.TroopsArrangementFieldWidth;
            var maxXPosition = (_pathfindingMap.Width - 1) - (layout.GetLength(0) - 1) - BattleArenaConstants.TroopsArrangementFieldWidth;

            var minYPosition = 0;
            var maxYPosition = (_pathfindingMap.Height - 1) - (layout.GetLength(1) - 1);
            
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
                    sameSizeObstacles[occupiedSpace] = new List<ObstacleStaticData>
                    {
                        obstacle
                    };
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
                    obstaclesWeightPool.Add(sameSizeObstaclePair.Value, 5);
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

        private static List<ObstacleStaticData> GetRandomObstacles(int randomObstaclesCount, IWeightedPool<List<ObstacleStaticData>> obstaclesWeightPool, IRandom randomGenerator)
        {
            var takenRandomObstacles = new List<ObstacleStaticData>();

            for (int i = 0; i < randomObstaclesCount; i++)
            {
                takenRandomObstacles.Add(obstaclesWeightPool.Choose().GetRandomItem(randomGenerator));
            }

            return takenRandomObstacles;
        }
    }
}
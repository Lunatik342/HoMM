using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Obstacles
{
    public class PredefinedObstaclesGenerationStrategy: IObstaclesGenerationStrategy
    {
        private readonly ObstacleGenerationParameters _generationParameters;

        public PredefinedObstaclesGenerationStrategy(ObstacleGenerationParameters generationParameters)
        {
            _generationParameters = generationParameters;
        }
        
        public IEnumerable<ObstacleCreationParameters> GetObstacles()
        {
            return _generationParameters.DeterminedObstacleParameters;
        }

    }
}
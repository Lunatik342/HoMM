using System.Collections.Generic;

namespace Battle.Arena.Obstacles
{
    public class PredefinedObstaclesGenerationStrategy: IObstaclesGenerationStrategy
    {
        private readonly ObstacleGenerationParameters _generationParameters;

        public PredefinedObstaclesGenerationStrategy(ObstacleGenerationParameters generationParameters)
        {
            _generationParameters = generationParameters;
        }
        
        public IEnumerable<ObstacleOnGridParameters> GetObstacles()
        {
            return _generationParameters.DeterminedObstacleParameters;
        }

    }
}
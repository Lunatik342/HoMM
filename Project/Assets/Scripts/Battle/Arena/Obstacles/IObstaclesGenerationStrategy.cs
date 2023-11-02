using System.Collections.Generic;
using Battle.Arena.StaticData;
using Zenject;

namespace Battle.Arena.Obstacles
{
    public interface IObstaclesGenerationStrategy
    {
        public IEnumerable<ObstacleOnGridParameters> GetObstacles();
        
        public class Factory: PlaceholderFactory<ObstacleGenerationParameters, BattleArenaId, IObstaclesGenerationStrategy>
        {
            
        }
    }
}
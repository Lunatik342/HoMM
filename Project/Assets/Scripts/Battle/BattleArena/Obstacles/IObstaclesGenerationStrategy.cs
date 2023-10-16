using System.Collections.Generic;
using Battle.BattleArena.StaticData;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Obstacles
{
    public interface IObstaclesGenerationStrategy
    {
        public IEnumerable<ObstacleOnGridParameters> GetObstacles();
        
        public class Factory: PlaceholderFactory<ObstacleGenerationParameters, BattleArenaId, IObstaclesGenerationStrategy>
        {
            
        }
    }
}
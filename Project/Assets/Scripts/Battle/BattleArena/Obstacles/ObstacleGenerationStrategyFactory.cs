using Battle.BattleArena.StaticData;
using Zenject;

namespace Battle.BattleArena.Obstacles
{
    public class ObstacleGenerationStrategyFactory: IFactory<ObstacleGenerationParameters, BattleArenaId, IObstaclesGenerationStrategy>
    {
        private readonly IInstantiator _instantiator;

        public ObstacleGenerationStrategyFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public IObstaclesGenerationStrategy Create(ObstacleGenerationParameters generationParameters, BattleArenaId battleArenaId)
        {
            if (generationParameters.IsRandom)
            {
                return _instantiator.Instantiate<RandomObstaclesGenerationStrategy>(new object[] { generationParameters, battleArenaId});
            }
            else
            {
                return _instantiator.Instantiate<PredefinedObstaclesGenerationStrategy>(new[] { generationParameters });
            }
        }
    }
}
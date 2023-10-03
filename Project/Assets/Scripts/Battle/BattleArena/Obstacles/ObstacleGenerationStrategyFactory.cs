using Zenject;

namespace Battle.BattleArena.Obstacles
{
    public class ObstacleGenerationStrategyFactory: IFactory<IObstaclesGenerationStrategy>
    {
        private readonly BattleStartParameters _battleStartParameters;
        private readonly IInstantiator _instantiator;

        public ObstacleGenerationStrategyFactory(BattleStartParameters battleStartParameters, IInstantiator instantiator)
        {
            _battleStartParameters = battleStartParameters;
            _instantiator = instantiator;
        }

        public IObstaclesGenerationStrategy Create()
        {
            if (_battleStartParameters.ObstacleGenerationParameters.IsRandom)
            {
                return _instantiator.Instantiate<RandomObstaclesGenerationStrategy>();
            }
            else
            {
                return _instantiator.Instantiate<PredefinedObstaclesGenerationStrategy>();
            }
        }
    }
}
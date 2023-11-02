using Battle.Arena.ArenaView;
using Battle.Arena.Map;
using Battle.Arena.Obstacles;
using Battle.Arena.StaticData;
using UnityEngine;
using Zenject;

namespace Battle.Arena.Installers
{
    public class BattleFieldInstaller : MonoInstaller
    {
        [SerializeField] private BattleArenaStaticData[] _battleArenasStaticData;
        [SerializeField] private ObstacleStaticData[] _obstaclesStaticData;
        [SerializeField] private ObstaclesGenerationStaticData _obstaclesGenerationStaticData;

        public override void InstallBindings()
        {
            InstallStaticData();
            InstallBattleMap();
            InstallBattleFieldViewSpawner();
            InstallObstaclesGeneration();
            InstallPathfinding();
        }

        private void InstallStaticData()
        {
            Container.Bind<BattleArenaStaticDataProvider>()
                .AsSingle()
                .WithArguments(_battleArenasStaticData, _obstaclesStaticData, _obstaclesGenerationStaticData);
        }

        private void InstallBattleMap()
        {
            Container.Bind<BattleMapFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleMapCreator>().AsSingle();
        }

        private void InstallBattleFieldViewSpawner()
        {
            Container.Bind<BattleFieldViewSpawner>().AsSingle();
        }

        private void InstallObstaclesGeneration()
        {
            Container
                .BindFactory<ObstacleGenerationParameters, BattleArenaId, IObstaclesGenerationStrategy, IObstaclesGenerationStrategy.Factory>()
                .FromFactory<ObstacleGenerationStrategyFactory>();
            Container.Bind<ObstaclesSpawner>().AsSingle();
        }

        private void InstallPathfinding()
        {
            Container.Bind<PathfindingService>().AsSingle();
        }
    }
}
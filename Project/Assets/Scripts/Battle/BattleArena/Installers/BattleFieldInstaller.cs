using Battle.BattleArena.CellsViews;
using Battle.BattleArena.Obstacles;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.StaticData;
using RogueSharp;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Installers
{
    public class BattleFieldInstaller : MonoInstaller
    {
        [SerializeField] private BattleArenaStaticData[] _battleArenasStaticData;
        [SerializeField] private ObstacleStaticData[] _obstaclesStaticData;
        [SerializeField] private ObstaclesGenerationStaticData _obstaclesGenerationStaticData;
        [SerializeField] private BattleArenaCellView _cellView;

        public override void InstallBindings()
        {
            InstallStaticData();
            InstallBattleMap();
            InstallBattleFieldViewSpawner();
            InstallCellsViews();
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

        private void InstallCellsViews()
        {
            Container.BindFactory<BattleArenaCellView, BattleArenaCellView.Factory>()
                .FromComponentInNewPrefab(_cellView)
                .UnderTransformGroup("CellViews");

            Container.BindFactory<BattleArenaId, BattleArenaCellView[,], CellsViewsArrayFactory>()
                .FromFactory<BattleArenaCellsViewsFactory>();

            Container.BindInterfacesAndSelfTo<BattleArenaCellsViewsSpawner>().AsSingle();
            Container.Bind<BattleArenaCellsDisplayService>().AsSingle();
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
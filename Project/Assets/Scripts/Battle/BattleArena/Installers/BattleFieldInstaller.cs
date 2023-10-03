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
            Container.Bind<BattleArenaStaticDataProvider>()
                .FromSubContainerResolve()
                .ByMethod(InstallBattleArenaStaticData)
                .AsSingle();

            Container.Bind<Map>().FromFactory<BattleMapFactory>().AsSingle();
            Container.Bind<BattleFieldViewSpawner>().AsSingle();
            
            Container.Bind<BattleArenaCellsDisplayService>()
                .FromSubContainerResolve()
                .ByMethod(InstallCellsViewService)
                .AsSingle();
            
            Container.Bind<IObstaclesGenerationStrategy>().FromFactory<ObstacleGenerationStrategyFactory>().AsSingle();
            Container.Bind<ObstaclesSpawner>().AsSingle();
            
            Container.Bind<PathfindingService>().AsSingle();
        }

        private void InstallBattleArenaStaticData(DiContainer container)
        {
            container.BindInstance(_battleArenasStaticData).AsSingle();
            container.BindInstance(_obstaclesStaticData).AsSingle();
            container.BindInstance(_obstaclesGenerationStaticData).AsSingle();
            container.Bind<BattleArenaStaticDataProvider>().AsSingle();
        }

        private void InstallCellsViewService(DiContainer container)
        {
            container.BindFactory<BattleArenaCellView, BattleArenaCellView.Factory>()
                .FromComponentInNewPrefab(_cellView)
                .UnderTransformGroup("CellViews");
            container.Bind<BattleArenaCellView[,]>().FromFactory<BattleArenaCellsViewsFactory>();
            container.Bind<BattleArenaCellsDisplayService>().AsSingle();
        }
    }
}
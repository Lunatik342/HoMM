using Battle.BattleArena.Cells;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.StaticData;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Installers
{
    public class BattleFieldInstaller : MonoInstaller
    {
        [SerializeField] private BattleArenaStaticData[] _battleArenasStaticData;
        [SerializeField] private ObstacleStaticData[] _obstaclesStaticData;
        [SerializeField] private BattleArenaCellView _cellView;

        public override void InstallBindings()
        {
            Container.Bind<BattleFieldViewSpawner>().AsSingle();

            Container.Bind<PathfindingMapFactory>().AsSingle();
            Container.Bind<RandomObstaclesFactory>().AsSingle();
            Container.Bind<PathfindingService>().AsSingle();

            Container.Bind<BattleArenaStaticDataProvider>()
                .FromSubContainerResolve()
                .ByMethod(InstallBattleArenaStaticData)
                .AsSingle();
            
            Container.Bind<BattleArenaCellsDisplayService>()
                .FromSubContainerResolve()
                .ByMethod(InstallCellsViewService)
                .AsSingle();
        }

        private void InstallBattleArenaStaticData(DiContainer container)
        {
            container.BindInstance(_battleArenasStaticData).AsSingle();
            container.BindInstance(_obstaclesStaticData).AsSingle();
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
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
        [SerializeField] private BattleFieldCellView _cellView;

        public override void InstallBindings()
        {
            Container.Bind<BattleFieldFactory>().AsSingle();
            Container.Bind<BattleFieldCellView>().FromInstance(_cellView).AsSingle();
            Container.Bind<BattleFieldCellsDisplayService>().AsSingle();
            
            Container.Bind<PathfindingMapFactory>().AsSingle();
            Container.Bind<RandomObstaclesFactory>().AsSingle();
            Container.Bind<PathfindingService>().AsSingle();
            
            BindStaticData();
        }

        private void BindStaticData()
        {
            Container.Bind<BattleArenaStaticDataService>()
                .FromSubContainerResolve()
                .ByMethod(InstallBattleArenaStaticData)
                .AsSingle();
        }

        private void InstallBattleArenaStaticData(DiContainer container)
        {
            container.Bind<BattleArenaStaticData[]>().FromInstance(_battleArenasStaticData).AsSingle();
            container.Bind<ObstacleStaticData[]>().FromInstance(_obstaclesStaticData).AsSingle();
            container.Bind<BattleArenaStaticDataService>().AsSingle();
        }
    }
}

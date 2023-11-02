using Battle.Arena.StaticData;
using Battle.BattleFlow.Installers;
using Battle.CellViewsGrid.CellsViews;
using Battle.CellViewsGrid.GridViewStateMachine.CellHoverHandler;
using Battle.CellViewsGrid.PathDisplay;
using UnityEngine;
using Zenject;

namespace Battle.CellViewsGrid.Installers
{
    public class CellViewsInstaller: MonoInstaller<BattleFlowInstaller>
    {
        [SerializeField] private BattleArenaCellView _cellViewPrefab;
        [SerializeField] private LineRenderer _pathLineRendererPrefab;
        
        public override void InstallBindings()
        {
            InstallCellsViews();
            InstallGridViewStateMachine();
            InstallPathDisplay();
        }

        private void InstallCellsViews()
        {
            Container.BindFactory<BattleArenaCellView, BattleArenaCellView.Factory>()
                .FromComponentInNewPrefab(_cellViewPrefab)
                .UnderTransformGroup("CellViews");

            Container.BindFactory<BattleArenaId, BattleArenaCellView[,], CellsViewsArrayFactory>()
                .FromFactory<BattleArenaCellsViewsFactory>();

            Container.BindInterfacesAndSelfTo<BattleArenaCellsViewsSpawner>().AsSingle();
            Container.Bind<BattleArenaCellsDisplayService>().AsSingle();
        }

        private void InstallGridViewStateMachine()
        {
            Container.Bind<GridViewStateMachine.GridViewStateMachine>().AsSingle();

            Container.Bind<ReachableCellHoverHandler>().AsSingle();
            Container.Bind<MeleeAttackCellHoverHandler>().AsSingle();
            Container.Bind<EmptyCellHoverHandler>().AsSingle();
        }

        private void InstallPathDisplay()
        {
            Container.Bind<PathDisplayService>().AsSingle();
            Container.Bind<LineRenderer>().FromComponentInNewPrefab(_pathLineRendererPrefab).AsSingle();
        }
    }
}
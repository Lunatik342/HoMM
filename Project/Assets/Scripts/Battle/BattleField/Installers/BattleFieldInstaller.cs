using Battle.BattleField.Cells;
using Battle.BattleField.Pathfinding;
using Zenject;

namespace Battle.BattleField.Installers
{
    public class BattleFieldInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.Bind<BattleFieldStaticDataService>().AsSingle();
            Container.Bind<BattleFieldFactory>().AsSingle();
            Container.Bind<BattleFieldCellsDisplayService>().AsSingle();
            
            Container.Bind<PathfindingMapFactory>().AsSingle();
            Container.Bind<RandomObstaclesFactory>().AsSingle();
            Container.Bind<PathfindingService>().AsSingle();
        }
    }
}

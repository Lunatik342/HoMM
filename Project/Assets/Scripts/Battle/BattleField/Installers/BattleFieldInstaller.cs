using Battle.BattleField.Cells;
using Zenject;

namespace Battle.BattleField.Installers
{
    public class BattleFieldInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.Bind<BattleFieldStaticDataService>().AsSingle();
            Container.Bind<BattleFieldFactory>().AsSingle();
        }
    }
}

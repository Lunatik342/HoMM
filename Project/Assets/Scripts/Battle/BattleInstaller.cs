using Battle.BattleArena.StaticData;
using Zenject;

namespace Battle
{
    public class BattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var testBattleStartParameters = new BattleStartParameters(BattleArenaId.Blank);
            
            Container.Bind<BattleStartParameters>().FromInstance(testBattleStartParameters).AsSingle();
            Container.BindInterfacesAndSelfTo<BattleStarter>().AsSingle().NonLazy();
        }
    }
}

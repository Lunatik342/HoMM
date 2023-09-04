using Zenject;

namespace Battle
{
    public class BattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BattleStarter>().AsSingle().NonLazy();
        }
    }
}

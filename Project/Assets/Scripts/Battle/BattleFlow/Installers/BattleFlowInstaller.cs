using Zenject;

namespace Battle.BattleFlow.Installers
{
    public class BattleFlowInstaller: MonoInstaller<BattleFlowInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BattleInputService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleCommandsDispatcher>().AsSingle();
        }
    }
}
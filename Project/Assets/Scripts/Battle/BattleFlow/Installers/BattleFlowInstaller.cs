using Battle.AI;
using Battle.DamageCalculation;
using Battle.Input;
using Battle.Result;
using Zenject;

namespace Battle.BattleFlow.Installers
{
    public class BattleFlowInstaller: MonoInstaller<BattleFlowInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BattleCellsInputService>().AsSingle();
            
            Container.Bind<UnitsQueueService>().AsSingle();
            Container.Bind<BattleFlowController>().AsSingle();
            Container.Bind<BattleResultEvaluator>().AsSingle();
            
            Container.Bind<DamageCalculator>().AsSingle();
            Container.Bind<DamagePredictionService>().AsSingle();
        }
    }
}
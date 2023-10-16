using Battle.Units.Movement;
using UnityEngine;
using Zenject;

namespace Battle.BattleFlow.Installers
{
    public class BattleFlowInstaller: MonoInstaller<BattleFlowInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<UnitsMoveCommandHandler>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<BattleCellsInputService>().AsSingle().NonLazy();
            Container.Bind<BattleTurnsProcessor>().AsSingle();
            Container.Bind<TurnsQueueService>().AsSingle();
            
            Container.Bind<TurnControllerLocal>().AsSingle();
            Container.Bind<UnitOnGridController>().AsSingle();

            Container.BindInstance(Camera.main).AsSingle();
        }
    }
}
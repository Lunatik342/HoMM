using Battle.BattleFlow.Commands;
using Battle.BattleFlow.StateMachine;
using Battle.Units.Movement;
using UnityEngine;
using Zenject;

namespace Battle.BattleFlow.Installers
{
    public class BattleFlowInstaller: MonoInstaller<BattleFlowInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<TurnsQueueService>().AsSingle();
            
            Container.Bind<BattleTurnsController>().AsSingle();
            
            Container.Bind<LocalPlayerCommandProvider>().AsSingle();
            Container.Bind<AICommandProvider>().AsSingle();
            
            Container.Bind<MoveCommandProcessor>().AsSingle();
            Container.Bind<CommandsProcessor>().AsSingle();

            Container.BindInterfacesAndSelfTo<BattleCellsInputService>().AsSingle().NonLazy();

            Container.BindInstance(Camera.main).AsSingle();

            Container.Bind<UnitControlViewState>().AsTransient();
            Container.Bind<WaitingForCommandProcessViewState>().AsTransient();
            Container.Bind<WaitingForEnemyTurnViewState>().AsTransient();
            Container.Bind<GridViewStateMachine>().AsSingle();
        }
    }
}
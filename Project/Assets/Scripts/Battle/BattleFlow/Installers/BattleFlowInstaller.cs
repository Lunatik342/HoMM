using Battle.BattleFlow.Commands;
using Battle.BattleFlow.StateMachine;
using Battle.Units.Movement;
using UnityEngine;
using Zenject;

namespace Battle.BattleFlow.Installers
{
    public class BattleFlowInstaller: MonoInstaller<BattleFlowInstaller>
    {
        [SerializeField] private Camera _mainCamera;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_mainCamera).AsSingle();
            Container.BindInterfacesAndSelfTo<BattleCellsInputService>().AsSingle();
            
            Container.Bind<TurnsQueueService>().AsSingle();
            Container.Bind<BattleTurnsController>().AsSingle();

            BindCommandsProcessing();
            BindGridViewStateMachine();
        }

        private void BindCommandsProcessing()
        {
            Container.BindFactory<LocalPlayerControlledCommandProvider, LocalPlayerControlledCommandProvider.Factory>().AsSingle();
            Container.BindFactory<AIControlledCommandProvider, AIControlledCommandProvider.Factory>().AsSingle();
            Container.Bind<MoveCommandProcessor>().AsSingle();
            Container.Bind<CommandsProcessor>().AsSingle();
        }

        private void BindGridViewStateMachine()
        {
            Container.Bind<UnitControlViewState>().AsTransient();
            Container.Bind<WaitingForCommandProcessViewState>().AsTransient();
            Container.Bind<WaitingForEnemyTurnViewState>().AsTransient();
            Container.Bind<GridViewStateMachine>().AsSingle();
        }
    }
}
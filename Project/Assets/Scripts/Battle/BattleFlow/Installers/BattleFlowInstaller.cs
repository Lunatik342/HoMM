using Battle.BattleFlow.Commands;
using Battle.BattleFlow.Commands.Processors;
using Battle.BattleFlow.StateMachine;
using Battle.BattleFlow.StateMachine.MouseOverCells;
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
            Container.Bind<GameResultEvaluator>().AsSingle();

            BindCommandsProcessing();
            BindGridViewStateMachine();
        }

        private void BindCommandsProcessing()
        {
            Container.BindFactory<LocalPlayerControlledCommandProvider, LocalPlayerControlledCommandProvider.Factory>().AsSingle();
            Container.BindFactory<AIControlledCommandProvider, AIControlledCommandProvider.Factory>().AsSingle();
            Container.Bind<MoveCommandProcessor>().AsSingle();
            Container.Bind<MeleeAttackCommandProcessor>().AsSingle();
            Container.Bind<CommandsProcessor>().AsSingle();
        }

        private void BindGridViewStateMachine()
        {
            Container.Bind<GridViewStateMachine>().AsSingle();
            
            Container.Bind<UnitControlViewState>().AsTransient();
            Container.Bind<WaitingForCommandProcessViewState>().AsTransient();
            Container.Bind<WaitingForEnemyTurnViewState>().AsTransient();

            Container.Bind<ReachableCellHoverHandler>().AsSingle();
            Container.Bind<MeleeAttackCellHoverHandler>().AsSingle();
            Container.Bind<EmptyCellHoverHandler>().AsSingle();
        }
    }
}
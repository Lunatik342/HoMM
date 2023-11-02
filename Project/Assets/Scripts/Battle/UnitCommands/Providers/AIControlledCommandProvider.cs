using Battle.CellViewsGrid.GridViewStateMachine;
using Battle.CellViewsGrid.GridViewStateMachine.States;
using Battle.UnitCommands.Commands;
using Battle.Units;
using Cysharp.Threading.Tasks;
using Utilities;
using Zenject;

namespace Battle.UnitCommands.Providers
{
    public class AIControlledCommandProvider: ICommandProvider
    {
        private readonly GridViewStateMachine _gridViewStateMachine;

        public AIControlledCommandProvider(GridViewStateMachine gridViewStateMachine)
        {
            _gridViewStateMachine = gridViewStateMachine;
        }

        public async UniTask<ICommand> WaitForCommand(Unit unit)
        {
            _gridViewStateMachine.Enter<WaitingForEnemyTurnViewState>();
            await UniTask.Delay(2000);
            var reachableCells = unit.MovementController.GetReachableCells();
            var randomCell = reachableCells.GetRandomItem();
            return new UnitMoveCommand(unit, randomCell.GridPosition);
        }
        
        public class Factory: PlaceholderFactory<AIControlledCommandProvider>
        {
            
        }
    }
}
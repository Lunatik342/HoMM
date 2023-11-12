using Battle.AI;
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
        private readonly ConsiderationsFactory _considerationsFactory;

        public AIControlledCommandProvider(GridViewStateMachine gridViewStateMachine, ConsiderationsFactory considerationsFactory)
        {
            _gridViewStateMachine = gridViewStateMachine;
            _considerationsFactory = considerationsFactory;
        }

        public async UniTask<ICommand> WaitForCommand(Unit unit)
        {
            _gridViewStateMachine.Enter<WaitingForEnemyTurnViewState>();
            await UniTask.Delay(500);
            var considerations = _considerationsFactory.Create(unit);

            IConsideration considerationWithMaxResult = null;

            foreach (var consideration in considerations)
            {
                if (!consideration.CalculationComplete)
                {
                    consideration.Consider();
                }

                if (considerationWithMaxResult == null || consideration.ConsiderationResult > considerationWithMaxResult.ConsiderationResult)
                {
                    considerationWithMaxResult = consideration;
                }
            }

            return considerationWithMaxResult.GetCommand();
        }
        
        public class Factory: PlaceholderFactory<AIControlledCommandProvider>
        {
            
        }
    }
}
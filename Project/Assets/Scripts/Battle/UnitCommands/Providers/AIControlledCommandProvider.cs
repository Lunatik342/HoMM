using Battle.AI;
using Battle.AI.Considerations;
using Battle.AI.ConsiderationsFactories;
using Battle.CellViewsGrid.GridViewStateMachine;
using Battle.CellViewsGrid.GridViewStateMachine.States;
using Battle.UnitCommands.Commands;
using Battle.Units;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Battle.UnitCommands.Providers
{
    public class AIControlledCommandProvider: ICommandProvider
    {
        private readonly GridViewStateMachine _gridViewStateMachine;
        private readonly ConsiderationsFactory _considerationsFactory;

        private int _aiThinkingFakeDelay = 500;

        public AIControlledCommandProvider(GridViewStateMachine gridViewStateMachine, ConsiderationsFactory considerationsFactory)
        {
            _gridViewStateMachine = gridViewStateMachine;
            _considerationsFactory = considerationsFactory;
        }

        public async UniTask<ICommand> WaitForCommand(Unit unit)
        {
            _gridViewStateMachine.Enter<WaitingForEnemyTurnViewState>();
            await UniTask.Delay(_aiThinkingFakeDelay);
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

            return considerationWithMaxResult == null ? new EmptyCommand() : considerationWithMaxResult.GetCommand();
        }
        
        public class Factory: PlaceholderFactory<AIControlledCommandProvider>
        {
            
        }
    }
}
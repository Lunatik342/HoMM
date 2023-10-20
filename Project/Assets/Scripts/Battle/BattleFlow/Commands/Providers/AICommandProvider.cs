using Battle.BattleArena.Pathfinding;
using Battle.BattleFlow.Commands;
using Battle.BattleFlow.StateMachine;
using Cysharp.Threading.Tasks;
using Utilities;

namespace Battle.BattleFlow
{
    public class AICommandProvider: ICommandProvider
    {
        private readonly PathfindingService _pathfindingService;
        private readonly GridViewStateMachine _gridViewStateMachine;

        public AICommandProvider(PathfindingService pathfindingService,
            GridViewStateMachine gridViewStateMachine)
        {
            _pathfindingService = pathfindingService;
            _gridViewStateMachine = gridViewStateMachine;
        }

        public async UniTask<ICommand> WaitForCommand(Unit unit)
        {
            _gridViewStateMachine.Enter<WaitingForEnemyTurnViewState>();
            await UniTask.Delay(2000);
            var reachableCells = _pathfindingService.GetReachableCells(unit);
            var randomCell = reachableCells.GetRandomItem();
            return new UnitMoveCommand(unit, randomCell.GridPosition);
        }
    }
}
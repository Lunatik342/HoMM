using System.Threading.Tasks;
using Battle.BattleArena.Pathfinding;
using Battle.BattleFlow.Commands;
using Battle.BattleFlow.StateMachine;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow
{
    public class LocalPlayerCommandProvider: ICommandProvider
    {
        private readonly GridViewStateMachine _gridViewStateMachine;

        public LocalPlayerCommandProvider(GridViewStateMachine gridViewStateMachine)
        {
            _gridViewStateMachine = gridViewStateMachine;
        }

        public async UniTask<ICommand> WaitForCommand(Unit unit)
        {
            var unitMoveCommandCompletionSource = new TaskCompletionSource<ICommand>();
            _gridViewStateMachine.Enter<ControllingUnitViewState, ControllingUnitStatePayload>(new ControllingUnitStatePayload(unitMoveCommandCompletionSource, unit));
            var command = await unitMoveCommandCompletionSource.Task;
            return command;
        }
    }
}
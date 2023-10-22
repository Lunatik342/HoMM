using System.Threading.Tasks;
using Battle.BattleArena.Pathfinding;
using Battle.BattleFlow.Commands;
using Battle.BattleFlow.StateMachine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Battle.BattleFlow
{
    public class LocalPlayerControlledCommandProvider: ICommandProvider
    {
        private readonly GridViewStateMachine _gridViewStateMachine;

        public LocalPlayerControlledCommandProvider(GridViewStateMachine gridViewStateMachine)
        {
            _gridViewStateMachine = gridViewStateMachine;
        }

        public async UniTask<ICommand> WaitForCommand(Unit unit)
        {
            var unitMoveCommandCompletionSource = new TaskCompletionSource<ICommand>();
            _gridViewStateMachine.Enter<UnitControlViewState, UnitControlStatePayload>(new UnitControlStatePayload(unitMoveCommandCompletionSource, unit));
            var command = await unitMoveCommandCompletionSource.Task;
            return command;
        }

        public class Factory : PlaceholderFactory<LocalPlayerControlledCommandProvider>
        {
            
        }
    }
}
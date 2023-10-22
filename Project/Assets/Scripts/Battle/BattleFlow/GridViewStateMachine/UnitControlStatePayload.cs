using System.Threading.Tasks;
using Battle.BattleArena.Pathfinding;
using Battle.BattleFlow.Commands;

namespace Battle.BattleFlow.StateMachine
{
    public class UnitControlStatePayload
    {
        public Unit Unit { get; }
        public TaskCompletionSource<ICommand> CommandAwaiter { get; }

        public UnitControlStatePayload(TaskCompletionSource<ICommand> commandAwaiter, Unit unit)
        {
            CommandAwaiter = commandAwaiter;
            Unit = unit;
        }
    }
}
using Battle.BattleArena.Pathfinding;
using Battle.BattleFlow.Commands;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow
{
    public interface ICommandProvider
    {
        UniTask<ICommand> WaitForCommand(Unit unit);
    }
}
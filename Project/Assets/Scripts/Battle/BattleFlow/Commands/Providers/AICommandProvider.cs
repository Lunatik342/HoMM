using Battle.BattleArena.Pathfinding;
using Battle.BattleFlow.Commands;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow
{
    public class AICommandProvider: ICommandProvider
    {
        public UniTask<ICommand> WaitForCommand(Unit unit)
        {
            throw new System.NotImplementedException();
        }
    }
}
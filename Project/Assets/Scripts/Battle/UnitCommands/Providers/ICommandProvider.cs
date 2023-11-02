using Battle.UnitCommands.Commands;
using Battle.Units;
using Cysharp.Threading.Tasks;

namespace Battle.UnitCommands.Providers
{
    public interface ICommandProvider
    {
        UniTask<ICommand> WaitForCommand(Unit unit);
    }
}
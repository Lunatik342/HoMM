using Battle.UnitCommands.Processors;
using Cysharp.Threading.Tasks;

namespace Battle.UnitCommands.Commands
{
    public interface ICommand
    {
        UniTask Process(CommandsProcessorFacade commandsProcessorFacade);
    }
}
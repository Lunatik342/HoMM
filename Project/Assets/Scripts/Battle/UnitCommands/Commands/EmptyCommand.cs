using Battle.UnitCommands.Processors;
using Cysharp.Threading.Tasks;

namespace Battle.UnitCommands.Commands
{
    public class EmptyCommand: ICommand
    {
        public UniTask Process(CommandsProcessorFacade commandsProcessorFacade)
        {
            return commandsProcessorFacade.ProcessEmptyCommand(this);
        }
    }
}
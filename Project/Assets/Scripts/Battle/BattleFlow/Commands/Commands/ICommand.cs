using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow.Commands
{
    public interface ICommand
    {
        //Accept of the visitor pattern
        UniTask Process(CommandsProcessor commandsProcessor);
    }
}
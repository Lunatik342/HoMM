using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow.Commands
{
    public interface ICommand
    {
        UniTask Process(CommandsProcessor commandsProcessor);
    }
}
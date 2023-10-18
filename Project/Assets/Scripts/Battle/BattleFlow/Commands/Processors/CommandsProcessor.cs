using Battle.Units.Movement;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow.Commands
{
    //Basically visitor of the visitor pattern
    public class CommandsProcessor
    {
        private readonly MoveCommandProcessor _moveCommandProcessor;

        public CommandsProcessor(MoveCommandProcessor moveCommandProcessor)
        {
            _moveCommandProcessor = moveCommandProcessor;
        }
        
        public async UniTask ProcessMoveCommand(UnitMoveCommand moveCommand)
        {
            await _moveCommandProcessor.Process(moveCommand.Unit, moveCommand.Position);
        }
    }
}
using Battle.UnitCommands.Commands;
using Cysharp.Threading.Tasks;

namespace Battle.UnitCommands.Processors
{
    //Basically visitor of the visitor pattern
    public class CommandsProcessorFacade
    {
        private readonly MoveCommandProcessor _moveCommandProcessor;
        private readonly MeleeAttackCommandProcessor _meleeAttackCommandProcessor;

        public CommandsProcessorFacade(MoveCommandProcessor moveCommandProcessor,
            MeleeAttackCommandProcessor meleeAttackCommandProcessor)
        {
            _moveCommandProcessor = moveCommandProcessor;
            _meleeAttackCommandProcessor = meleeAttackCommandProcessor;
        }
        
        public async UniTask ProcessMoveCommand(UnitMoveCommand moveCommand)
        {
            await _moveCommandProcessor.Process(moveCommand.Unit, moveCommand.Position);
        }
        
        public async UniTask ProcessMeleeAttackCommand(UnitMeleeAttackCommand attackCommand)
        {
            await _meleeAttackCommandProcessor.Process(attackCommand.Unit, attackCommand.TargetUnit, attackCommand.FromPosition);
        }

        public UniTask ProcessEmptyCommand(EmptyCommand doNothingCommand)
        {
            return UniTask.CompletedTask;
        }
    }
}
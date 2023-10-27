using Battle.BattleFlow.Commands.Processors;
using Battle.Units.Movement;
using Cysharp.Threading.Tasks;

namespace Battle.BattleFlow.Commands
{
    //Basically visitor of the visitor pattern
    public class CommandsProcessor
    {
        private readonly MoveCommandProcessor _moveCommandProcessor;
        private readonly MeleeAttackCommandProcessor _meleeAttackCommandProcessor;

        public CommandsProcessor(MoveCommandProcessor moveCommandProcessor,
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
    }
}
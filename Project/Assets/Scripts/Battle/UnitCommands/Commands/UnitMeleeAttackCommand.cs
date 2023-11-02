using Battle.UnitCommands.Processors;
using Battle.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.UnitCommands.Commands
{
    public class UnitMeleeAttackCommand: ICommand
    {
        public Unit Unit { get; private set; }
        public Vector2Int FromPosition { get; private set; }
        public Unit TargetUnit { get; private set; }

        public UnitMeleeAttackCommand(Unit unit, Vector2Int fromPosition, Unit targetUnit)
        {
            Unit = unit;
            FromPosition = fromPosition;
            TargetUnit = targetUnit;
        }
        
        public async UniTask Process(CommandsProcessorFacade commandsProcessorFacade)
        {
            await commandsProcessorFacade.ProcessMeleeAttackCommand(this);
        }
    }
}
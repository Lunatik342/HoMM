using Battle.BattleArena.Pathfinding;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.BattleFlow.Commands
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
        
        public UniTask Process(CommandsProcessor commandsProcessor)
        {
            throw new System.NotImplementedException();
        }
    }
}
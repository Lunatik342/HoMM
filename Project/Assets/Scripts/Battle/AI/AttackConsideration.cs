using Algorithms.RogueSharp;
using Battle.UnitCommands.Commands;
using Battle.Units;

namespace Battle.AI
{
    public class AttackConsideration: IConsideration
    {
        private readonly Unit _attackingUnit;
        private readonly Unit _targetUnit;
        private readonly Cell _attackFromCell;
        private readonly IConsideration _moveConsiderationForCell;

        public float ConsiderationResult { get; private set; }
        public bool CalculationComplete { get; private set; }

        public AttackConsideration(Unit attackingUnit, Unit targetUnit, Cell attackFromCell, IConsideration moveConsiderationForCell)
        {
            _attackingUnit = attackingUnit;
            _targetUnit = targetUnit;
            _attackFromCell = attackFromCell;
            _moveConsiderationForCell = moveConsiderationForCell;
        }

        public void Consider()
        {
            CalculationComplete = true;
            ConsiderationResult = 1000f + _moveConsiderationForCell.ConsiderationResult;
        }

        public ICommand GetCommand()
        {
            return new UnitMeleeAttackCommand(_attackingUnit, _attackFromCell.GridPosition, _targetUnit);
        }
    }
}
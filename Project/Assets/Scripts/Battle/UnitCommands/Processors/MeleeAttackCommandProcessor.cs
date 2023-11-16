using System.Threading.Tasks;
using Battle.Arena.Misc;
using Battle.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.UnitCommands.Processors
{
    public class MeleeAttackCommandProcessor
    {
        private readonly DamageCalculator _damageCalculator;

        public MeleeAttackCommandProcessor(DamageCalculator damageCalculator)
        {
            _damageCalculator = damageCalculator;
        }
        
        public async UniTask Process(Unit attackingUnit, Unit attackedUnit, Vector2Int attackPosition)
        {
            if (attackingUnit.PositionProvider.OccupiedCell.GridPosition != attackPosition)
            {
                await attackingUnit.MovementController.MoveToPosition(attackPosition);
            }

            await UniTask.WhenAll(
                attackingUnit.RotationController.SmoothLookAt(attackedUnit.PositionProvider.OccupiedCell.GridPosition.ToBattleArenaWorldPosition()),
                attackedUnit.RotationController.SmoothLookAt(attackingUnit.PositionProvider.OccupiedCell.GridPosition.ToBattleArenaWorldPosition()));

            await Attack(attackingUnit, attackedUnit);

            if (attackedUnit.Health.IsAlive)
            {
                if (attackedUnit.Retaliation.CanRetaliate)
                {
                    attackedUnit.Retaliation.OnRetaliation();
                    await Attack(attackedUnit, attackingUnit);
                }
            }

            var attackedUnitRotationTask = UniTask.CompletedTask;

            if (attackedUnit.Health.IsAlive)
            {
                attackedUnitRotationTask = attackedUnit.RotationController.SmoothLookAtEnemySide();
            }
            
            var attackingUnitRotationTask = UniTask.CompletedTask;

            if (attackingUnit.Health.IsAlive)
            {
                attackingUnitRotationTask = attackingUnit.RotationController.SmoothLookAtEnemySide();
            }

            await UniTask.WhenAll(attackedUnitRotationTask, attackingUnitRotationTask);
        }

        private async Task Attack(Unit attackingUnit, Unit attackedUnit)
        {
            var (damageDealtTask, fullAnimationTask) = attackingUnit.UnitSimpleActions.MakeAttack();
            await damageDealtTask;

            var damage = _damageCalculator.CalculateDamage(attackingUnit, attackedUnit);
            var takeDamageAnimationTask = attackedUnit.UnitSimpleActions.TakeDamage(damage);

            await UniTask.WhenAll(fullAnimationTask, takeDamageAnimationTask);
        }
    }
}
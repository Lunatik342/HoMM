using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.BattleFlow.Commands.Processors
{
    public class MeleeAttackCommandProcessor
    {
        public async UniTask Process(Unit attackingUnit, Unit attackedUnit, Vector2Int attackPosition)
        {
            await attackingUnit.MovementController.MoveToPosition(attackPosition);

            await UniTask.WhenAll(
                attackingUnit.RotationController.SmoothLookAt(attackedUnit.PositionProvider.OccupiedCell.GridPosition.ToBattleArenaWorldPosition()),
                attackedUnit.RotationController.SmoothLookAt(attackingUnit.PositionProvider.OccupiedCell.GridPosition.ToBattleArenaWorldPosition()));

            var damageCompletionSource = new UniTaskCompletionSource<int>();
            var attackAnimationTask = attackingUnit.UnitSimpleActions.MakeAttack(damageCompletionSource);
            var damage = await damageCompletionSource.Task;
            var takeDamageAnimationTask = attackedUnit.UnitSimpleActions.TakeDamage(damage);

            await UniTask.WhenAll(attackAnimationTask, takeDamageAnimationTask);

            var t = UniTask.CompletedTask;

            if (attackedUnit.Health.IsAlive)
            {
                t = attackedUnit.RotationController.SmoothLookAtEnemySide();
            }

            var t2 = attackingUnit.RotationController.SmoothLookAtEnemySide();
            await UniTask.WhenAll(t, t2);
        }
    }
}
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
                attackingUnit.RotationController.SmoothLookAt(attackedUnit.GameObject.transform.position),
                attackedUnit.RotationController.SmoothLookAt(attackingUnit.GameObject.transform.position));

            var damageCompletionSource = new UniTaskCompletionSource<int>();
            var attackAnimationTask = attackingUnit.UnitActions.MakeAttack(damageCompletionSource);
            var damage = await damageCompletionSource.Task;
            var takeDamageAnimationTask = attackedUnit.UnitActions.TakeDamage(damage);

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
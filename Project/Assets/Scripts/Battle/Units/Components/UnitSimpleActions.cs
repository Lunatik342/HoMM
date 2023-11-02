using Battle.Units.Animation;
using Cysharp.Threading.Tasks;

namespace Battle.Units.Components
{
    public class UnitSimpleActions
    {
        private readonly UnitAttack _unitAttack;
        private readonly UnitHealth _unitHealth;
        private readonly UnitAnimator _unitAnimator;

        public UnitSimpleActions(UnitAttack unitAttack, UnitHealth unitHealth, UnitAnimator unitAnimator)
        {
            _unitAttack = unitAttack;
            _unitHealth = unitHealth;
            _unitAnimator = unitAnimator;
        }

        public async UniTask MakeAttack(UniTaskCompletionSource<int> damageCalculationCompletionSource)
        {
            var animationTimings = _unitAnimator.PlayAttackAnimation();

            await animationTimings.damageDealTiming;
            damageCalculationCompletionSource.TrySetResult(_unitAttack.GetRawDamage());
            await animationTimings.fullAnimationDuration;
        }

        public async UniTask TakeDamage(int damage)
        {
            _unitHealth.TakeDamage(damage);
            await (_unitHealth.IsAlive ? _unitAnimator.PlayTakeDamageAnimation() : _unitAnimator.PlayDeathAnimation());
        }
    }
}
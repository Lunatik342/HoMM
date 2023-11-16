using Cysharp.Threading.Tasks;

namespace Battle.Units.Components
{
    public class UnitSimpleActions
    {
        private readonly UnitHealth _unitHealth;
        private readonly UnitAnimator _unitAnimator;

        public UnitSimpleActions(UnitHealth unitHealth, UnitAnimator unitAnimator)
        {
            _unitHealth = unitHealth;
            _unitAnimator = unitAnimator;
        }

        public (UniTask, UniTask) MakeAttack()
        {
            return _unitAnimator.PlayAttackAnimation();
        }

        public async UniTask TakeDamage(int damage)
        {
            _unitHealth.TakeDamage(damage);
            await (_unitHealth.IsAlive ? _unitAnimator.PlayTakeDamageAnimation() : _unitAnimator.PlayDeathAnimation());
        }
    }
}
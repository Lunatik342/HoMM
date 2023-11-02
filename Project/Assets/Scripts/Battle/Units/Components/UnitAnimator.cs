using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Units.Components
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
        
        public void SetMoving(bool isMoving)
        {
            _animator.SetBool(IsMoving, isMoving);
        }
        
        //TODO Remove hardcode
        public (UniTask damageDealTiming, UniTask fullAnimationDuration) PlayAttackAnimation()
        {
            _animator.SetTrigger(Attack);
            return (UniTask.Delay(100), UniTask.Delay(250));
        }

        public async UniTask PlayDeathAnimation()
        {
            _animator.SetTrigger(Death);
            await Task.Delay(1250);
        }

        public async UniTask PlayTakeDamageAnimation()
        {
            _animator.SetTrigger(TakeDamage);
            await Task.Delay(835);
        }
    }
}

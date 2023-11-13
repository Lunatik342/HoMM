using Animations;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Units.Components
{
    public class UnitAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private int _attackTiming;
        
        private readonly int _isMoving = Animator.StringToHash("IsMoving");
        private readonly int _attackTrigger = Animator.StringToHash("Attack");
        private readonly int _deathTrigger = Animator.StringToHash("Death");
        private readonly int _takeDamageTrigger = Animator.StringToHash("TakeDamage");

        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _attackStateHash = Animator.StringToHash("Attack");
        private readonly int _runStateHash = Animator.StringToHash("Running");
        private readonly int _takeDamageStateHash = Animator.StringToHash("TakeDamage");
        private readonly int _deathStateHash = Animator.StringToHash("Death");

        private AnimationStates _currentState;

        public void SetMoving(bool isMoving)
        {
            _animator.SetBool(_isMoving, isMoving);
        }
        
        public (UniTask damageDealTiming, UniTask fullAnimationDuration) PlayAttackAnimation()
        {
            _animator.SetTrigger(_attackTrigger);
            return (UniTask.Delay(_attackTiming), WaitWhileIn(AnimationStates.Attacking));
        }

        public async UniTask PlayDeathAnimation()
        {
            _animator.SetTrigger(_deathTrigger);
            await WaitWhileIn(AnimationStates.Dead);
            gameObject.SetActive(false);
        }

        public async UniTask PlayTakeDamageAnimation()
        {
            _animator.SetTrigger(_takeDamageTrigger);
            await WaitWhileIn(AnimationStates.TakingDamage);
        }

        void IAnimationStateReader.EnteredState(int stateHash)
        {
            _currentState = GetStateForHash(stateHash);
        }

        void IAnimationStateReader.ExitedState(int stateHash)
        {
            
        }

        private AnimationStates GetStateForHash(int stateHash)
        {
            if (stateHash == _idleStateHash)
            {
                return AnimationStates.Idle;
            }
            else if (stateHash == _attackStateHash)
            {
                return AnimationStates.Attacking;
            }
            else if (stateHash == _runStateHash)
            {
                return AnimationStates.Running;
            }
            else if (stateHash == _takeDamageStateHash)
            {
                return AnimationStates.TakingDamage;
            }
            else if (stateHash == _deathStateHash)
            {
                return AnimationStates.Dead;
            }

            return AnimationStates.Unknown;
        }

        private UniTask WaitWhileIn(AnimationStates state)
        {
            return UniTask.WaitWhile(() => _currentState == state);
        }
    }

    public enum AnimationStates
    {
        Unknown,
        Idle,
        Attacking,
        Running,
        TakingDamage,
        Dead
    }
}

using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class RotationController: IDeathEventReceiver
    {
        private readonly Transform _transform;
        private readonly float _rotationSpeed;
        private readonly Vector3 _enemySideOfFieldDirection;

        private Tween _rotationTween;

        public RotationController(Transform transform, float rotationSpeed, Vector3 enemySideOfFieldDirection)
        {
            _transform = transform;
            _rotationSpeed = rotationSpeed;
            _enemySideOfFieldDirection = enemySideOfFieldDirection;
        }

        public async UniTask SmoothLookAtEnemySide()
        {
            await SmoothLookAt(GetEnemySideLookAtPosition());
        }

        public async UniTask SmoothLookAt(Vector3 position)
        {
            StopRotation();
            _rotationTween = _transform.DOLookAt(position, _rotationSpeed, AxisConstraint.Y).SetSpeedBased(true);
            await _rotationTween.ToUniTask();
        }

        public void LookAtEnemySide()
        {
            LookAt(GetEnemySideLookAtPosition());
        }
        
        public void LookAt(Vector3 position)
        {
            _transform.LookAt(position);
        }

        public void OnDeath()
        {
            StopRotation();
        }

        private Vector3 GetEnemySideLookAtPosition()
        {
            return _transform.position + _enemySideOfFieldDirection;
        }

        private void StopRotation()
        {
            _rotationTween?.Kill();
        }
    }
}
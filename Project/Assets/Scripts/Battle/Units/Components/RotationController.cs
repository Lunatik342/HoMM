using Battle.Units.Components.Interfaces;
using Battle.Units.StaticData.Components;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Units.Components
{
    public class RotationController: IDeathEventReceiver
    {
        private readonly Transform _transform;
        private readonly UnitRotationStaticData _staticData;
        private readonly Vector3 _enemySideOfFieldDirection;

        private Tween _rotationTween;

        public RotationController(Transform transform, UnitRotationStaticData staticData, Team team)
        {
            _transform = transform;
            _staticData = staticData;
            _enemySideOfFieldDirection = team.GetRotationDirection();
        }

        public async UniTask SmoothLookAtEnemySide()
        {
            await SmoothLookAt(GetEnemySideLookAtPosition());
        }

        public async UniTask SmoothLookAt(Vector3 position)
        {
            StopRotation();
            _rotationTween = _transform.DOLookAt(position, _staticData.RotationSpeed, AxisConstraint.Y).SetSpeedBased(true);
            await _rotationTween.ToUniTask();
        }

        public void LookAtEnemySide()
        {
            _transform.rotation = Quaternion.LookRotation(_enemySideOfFieldDirection);
        }

        void IDeathEventReceiver.OnDeath()
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
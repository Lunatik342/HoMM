using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class RotationController
    {
        private readonly Transform _transform;
        private readonly float _speed;

        private Tween _rotationTween;

        public RotationController(Transform transform, float speed)
        {
            _transform = transform;
            _speed = speed;
        }

        public async UniTask SmoothLookAt(Vector3 position)
        {
            _rotationTween?.Kill();
            _rotationTween = _transform.DOLookAt(position, _speed, AxisConstraint.Y).SetSpeedBased(true);
            await _rotationTween.ToUniTask();
        }
        
        public void LookAt(Vector3 position)
        {
            _transform.LookAt(position);
        }
    }
}
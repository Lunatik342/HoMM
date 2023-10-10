using Battle.BattleArena.Pathfinding;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class FlyingUnitMovementController: IUnitMovementController, IDeathEventReceiver
    {
        private readonly Transform _transform;
        private readonly float _flySpeed;
        private readonly BattleMapPlaceable _mapPlaceable;
        private readonly RotationController _rotationController;
        private readonly PathfindingService _pathfindingService;

        private Sequence _flyTween;

        public FlyingUnitMovementController(Transform transform, 
            float flySpeed, 
            BattleMapPlaceable mapPlaceable, 
            RotationController rotationController,
            PathfindingService pathfindingService)
        {
            _transform = transform;
            _flySpeed = flySpeed;
            _mapPlaceable = mapPlaceable;
            _rotationController = rotationController;
            _pathfindingService = pathfindingService;
        }

        public async UniTask MoveToPosition(Vector2Int targetPosition)
        {
            var targetCell = _pathfindingService.FindPathForFlyingUnit(targetPosition);
            var targetWorldPosition = targetCell.GetWorldPosition();

            _flyTween = _transform.DOJump(targetWorldPosition, 2, 1, _flySpeed).SetSpeedBased();

            await _rotationController.SmoothLookAt(targetWorldPosition);
            await _flyTween.ToUniTask();
            
            _mapPlaceable.RelocateTo(targetCell.GetLogicalCells());
        }

        public void OnDeath()
        {
            Stop();
        }

        private void Stop()
        {
            _flyTween?.Kill();
        }
    }
}
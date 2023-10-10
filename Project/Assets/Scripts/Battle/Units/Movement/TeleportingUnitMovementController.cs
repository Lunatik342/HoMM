using Battle.BattleArena.Pathfinding;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class TeleportingUnitMovementController: IUnitMovementController, IDeathEventReceiver
    {
        private readonly Transform _transform;
        private readonly BattleMapPlaceable _mapPlaceable;
        private readonly RotationController _rotationController;
        private readonly PathfindingService _pathfindingService;

        private Sequence _teleportTween;

        public TeleportingUnitMovementController(Transform transform,
            BattleMapPlaceable mapPlaceable, 
            RotationController rotationController,
            PathfindingService pathfindingService)
        {
            _transform = transform;
            _mapPlaceable = mapPlaceable;
            _rotationController = rotationController;
            _pathfindingService = pathfindingService;
        }

        public async UniTask MoveToPosition(Vector2Int targetPosition)
        {
            var targetCell = _pathfindingService.FindPathForFlyingUnit(targetPosition);
            var targetWorldPosition = targetCell.GetWorldPosition();
            await _rotationController.SmoothLookAt(targetWorldPosition);

            _teleportTween = DOTween.Sequence();
            _teleportTween.AppendInterval(0.5f);
            _teleportTween.AppendCallback(() => { _transform.position = targetWorldPosition; });
            _teleportTween.AppendInterval(0.5f);

            await _teleportTween.ToUniTask();
            _mapPlaceable.RelocateTo(targetCell.GetLogicalCells());
        }

        public void OnDeath()
        {
            Stop();
        }

        private void Stop()
        {
            _teleportTween?.Kill();
        }
    }
}
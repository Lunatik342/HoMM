using Battle.BattleArena.Pathfinding;
using Battle.Units.Movement.StaticData;
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
        private readonly TeleportingUnitMovementStaticData _staticData;

        private Sequence _teleportTween;

        public TeleportingUnitMovementController(Transform transform,
            BattleMapPlaceable mapPlaceable, 
            RotationController rotationController,
            PathfindingService pathfindingService,
            TeleportingUnitMovementStaticData staticData)
        {
            _transform = transform;
            _mapPlaceable = mapPlaceable;
            _rotationController = rotationController;
            _pathfindingService = pathfindingService;
            _staticData = staticData;
        }

        public async UniTask MoveToPosition(Vector2Int targetPosition)
        {
            var targetCell = _pathfindingService.FindPathForFlyingUnit(targetPosition);
            var targetWorldPosition = targetCell.GetWorldPosition();
            await _rotationController.SmoothLookAt(targetWorldPosition);

            _teleportTween = DOTween.Sequence();
            _teleportTween.AppendInterval(_staticData.DelayBeforeTeleport);
            _teleportTween.AppendCallback(() => { _transform.position = targetWorldPosition; });
            _teleportTween.AppendInterval(_staticData.DelayAfterTeleport);

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
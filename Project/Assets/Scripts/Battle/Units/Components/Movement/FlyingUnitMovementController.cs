using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using Battle.Units.Movement.StaticData;
using Battle.Units.StatsSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class FlyingUnitMovementController: UnitMovementControllerBase, IDeathEventReceiver
    {
        private readonly Transform _transform;
        private readonly BattleMapPlaceable _mapPlaceable;
        private readonly RotationController _rotationController;
        private readonly PathfindingService _pathfindingService;
        private readonly FlyingUnitMovementStaticData _staticData;

        private Sequence _flyTween;

        public FlyingUnitMovementController(Transform transform,
            BattleMapPlaceable mapPlaceable, 
            RotationController rotationController,
            PathfindingService pathfindingService,
            FlyingUnitMovementStaticData staticData,
            UnitStatsProvider statsProvider) : base(staticData, statsProvider)
        {
            _transform = transform;
            _mapPlaceable = mapPlaceable;
            _rotationController = rotationController;
            _pathfindingService = pathfindingService;
            _staticData = staticData;
        }

        public override async UniTask MoveToPosition(Vector2Int targetPosition)
        {
            var currentCell = _mapPlaceable.OccupiedCell;
            var targetCell = _pathfindingService.FindPathForFlyingUnit(targetPosition);
            var travelDistance = Vector3.Distance(currentCell.ToBattleArenaWorldPosition(), targetCell.GetWorldPosition());
            var flyDuration = travelDistance / _staticData.FlySpeed;

            var targetWorldPosition = targetCell.GetWorldPosition();

            await _rotationController.SmoothLookAt(targetWorldPosition);
            
            _flyTween = _transform.DOJump(targetWorldPosition, _staticData.JumpPower, 1, flyDuration).SetSpeedBased();
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
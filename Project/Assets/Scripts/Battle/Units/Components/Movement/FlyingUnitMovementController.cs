using System.Collections.Generic;
using Algorithms.RogueSharp;
using Battle.Arena.Map;
using Battle.Arena.Misc;
using Battle.CellViewsGrid.PathDisplay;
using Battle.StatsSystem;
using Battle.Units.Components.Interfaces;
using Battle.Units.StaticData.Components.Movement;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Units.Components.Movement
{
    public class FlyingUnitMovementController: UnitMovementController, IDeathEventReceiver
    {
        private readonly Transform _transform;
        private readonly BattleMapPlaceable _mapPlaceable;
        private readonly RotationController _rotationController;
        private readonly PathfindingService _pathfindingService;
        private readonly FlyingUnitMovementStaticData _staticData;
        private readonly Unit _unit;

        private Sequence _flyTween;

        public FlyingUnitMovementController(Transform transform,
            BattleMapPlaceable mapPlaceable, 
            RotationController rotationController,
            PathfindingService pathfindingService,
            FlyingUnitMovementStaticData staticData,
            UnitStatsProvider statsProvider,
            Unit unit) : base(staticData, statsProvider)
        {
            _transform = transform;
            _mapPlaceable = mapPlaceable;
            _rotationController = rotationController;
            _pathfindingService = pathfindingService;
            _staticData = staticData;
            _unit = unit;
        }

        public override void DisplayPathToCell(PathDisplayService pathDisplayService, Vector2Int position)
        {
            var path = GetPath(position);
            pathDisplayService.DisplayArc(path);
        }

        public override async UniTask MoveToPosition(Vector2Int targetPosition)
        {
            var currentCell = _mapPlaceable.OccupiedCell;
            var path = GetPath(targetPosition);
            var lastCell = path[^1];
            var lastCellPosition = lastCell.GetWorldPosition();

            var travelDistance = Vector3.Distance(currentCell.ToBattleArenaWorldPosition(), lastCellPosition);
            var flyDuration = travelDistance / _staticData.FlySpeed;

            await _rotationController.SmoothLookAt(lastCellPosition);
            
            _flyTween = _transform.DOJump(lastCellPosition, _staticData.JumpPower, 1, flyDuration).SetSpeedBased();
            await _flyTween.ToUniTask();
            
            _mapPlaceable.RelocateTo(lastCell.GetLogicalCell());
        }

        public override List<Cell> GetReachableCells()
        {
            return _pathfindingService.GetReachableCellsForFlyingUnit(_unit, _travelDistanceStat.Value);
        }

        void IDeathEventReceiver.OnDeath()
        {
            Stop();
        }

        private List<ICell> GetPath(Vector2Int targetPosition)
        {
            return _pathfindingService.FindPathForFlyingUnit(targetPosition, _unit);
        }

        private void Stop()
        {
            _flyTween?.Kill();
        }
    }
}
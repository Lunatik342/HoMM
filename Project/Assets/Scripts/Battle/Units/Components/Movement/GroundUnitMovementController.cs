using System.Collections.Generic;
using System.Linq;
using Battle.BattleArena.Pathfinding;
using Battle.Units.Animation;
using Battle.Units.Movement.StaticData;
using Battle.Units.StatsSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueSharp;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class GroundUnitMovementController: UnitMovementControllerBase, IDeathEventReceiver
    {
        private readonly Transform _transform;
        private readonly BattleMapPlaceable _mapPlaceable;
        private readonly RotationController _rotationController;
        private readonly PathfindingService _pathfindingService;
        private readonly GroundUnitMovementStaticData _staticData;
        private readonly Unit _unit;
        private readonly UnitAnimator _animator;

        private Tweener _moveTween;

        public GroundUnitMovementController(Transform transform,
            BattleMapPlaceable mapPlaceable, 
            RotationController rotationController,
            PathfindingService pathfindingService,
            GroundUnitMovementStaticData staticData,
            Unit unit,
            UnitStatsProvider statsProvider, 
            UnitAnimator animator) : base(staticData, statsProvider)
        {
            _transform = transform;
            _mapPlaceable = mapPlaceable;
            _rotationController = rotationController;
            _pathfindingService = pathfindingService;
            _staticData = staticData;
            _unit = unit;
            _animator = animator;
        }

        public override async UniTask MoveToPosition(Vector2Int targetPosition)
        {
            var path = GetPath(targetPosition);
            await MoveToPosition(path);
        }
        
        public override List<Cell> GetReachableCells()
        {
            return _pathfindingService.GetReachableCells(_unit);
        }

        private List<ICell> GetPath(Vector2Int targetPosition)
        {
            var path = _pathfindingService.FindPath(targetPosition, _unit);
            path.RemoveAt(0);
            return path;
        }

        private async UniTask MoveToPosition(List<ICell> path)
        {
            var pathPositions = path.Select(p => p.GetWorldPosition()).ToArray();
            
            await _rotationController.SmoothLookAt(pathPositions[0]);

            _animator.SetMoving(true);
            var pathTween = _transform
                .DOPath(pathPositions, _staticData.MovementSpeed, PathType.CatmullRom).SetLookAt(.05f)
                .SetEase(Ease.Linear).SetSpeedBased(true);

            pathTween.onWaypointChange += index =>
            {
                if (path.Count <= index)
                {
                    return;
                }
                
                var cellToOccupy = path[index].GetLogicalCell();
                _mapPlaceable.RelocateTo(cellToOccupy);
            };

            _moveTween = pathTween;
            await pathTween.ToUniTask();
            
            _animator.SetMoving(false);
        }

        private void StopMovement()
        {
            _moveTween?.Kill();
        }

        void IDeathEventReceiver.OnDeath()
        {
            StopMovement();
        }
    }
}
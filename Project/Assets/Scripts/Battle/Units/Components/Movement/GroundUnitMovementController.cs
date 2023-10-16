using System.Collections.Generic;
using System.Linq;
using Battle.BattleArena.PathDisplay;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Movement.StaticData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueSharp;
using UnityEngine;

namespace Battle.Units.Movement
{
    public class GroundUnitMovementController: IUnitMovementController, IDeathEventReceiver
    {
        private readonly Transform _transform;
        private readonly BattleMapPlaceable _mapPlaceable;
        private readonly RotationController _rotationController;
        private readonly PathfindingService _pathfindingService;
        private readonly GroundUnitMovementStaticData _staticData;

        private Tweener _moveTween;

        public GroundUnitMovementController(Transform transform,
            BattleMapPlaceable mapPlaceable, 
            RotationController rotationController,
            PathfindingService pathfindingService,
            GroundUnitMovementStaticData staticData)
        {
            _transform = transform;
            _mapPlaceable = mapPlaceable;
            _rotationController = rotationController;
            _pathfindingService = pathfindingService;
            _staticData = staticData;
        }

        public async UniTask MoveToPosition(Vector2Int targetPosition)
        {
            var path = GetPath(targetPosition);
            await MoveToPosition(path);
        }

        private List<ICell> GetPath(Vector2Int targetPosition)
        {
            var path = _pathfindingService.FindPath(targetPosition, _mapPlaceable).Steps.ToList();
            path.RemoveAt(0);
            return path;
        }

        private async UniTask MoveToPosition(List<ICell> path)
        {
            var pathPositions = path.Select(p => p.GetWorldPosition()).ToArray();
            
            await _rotationController.SmoothLookAt(pathPositions[0]);

            var pathTween = _transform
                .DOPath(pathPositions, _staticData.MovementSpeed, PathType.CatmullRom).SetLookAt(.05f)
                .SetEase(Ease.Linear).SetSpeedBased(true);

            pathTween.onWaypointChange += index =>
            {
                var cellsToOccupy = path[index].GetLogicalCells();
                _mapPlaceable.RelocateTo(cellsToOccupy);
            };

            _moveTween = pathTween;
            await pathTween.ToUniTask();
        }

        public void OnDeath()
        {
            StopMovement();
        }

        private void StopMovement()
        {
            _moveTween?.Kill();
        }
    }
}
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RogueSharp;
using UnityEngine;

namespace Battle.BattleArena.Pathfinding.Movement
{
    public class OnGroundMapMovementHandler: IMapMovementHandler
    {
        private readonly Map _map;
        private readonly PathfindingService _pathfindingService;

        public OnGroundMapMovementHandler(Map map, PathfindingService pathfindingService)
        {
            _map = map;
            _pathfindingService = pathfindingService;
        }
        
        public async UniTask MoveToPosition(Vector2Int targetPosition, Unit unit)
        {
            var path = _pathfindingService.FindPath(targetPosition, unit.BattleMapPlaceable);
            
            var cell = path.TryStepForward();

            if (cell != null)
            {
                await unit.RotationController.SmoothLookAt((new Vector2Int(cell.X, cell.Y)).ToBattleArenaWorldPosition());
            }
            
            while (cell != null)
            {
                await unit.MovementController.MoveToPosition(new Vector2Int(cell.X, cell.Y));
                unit.BattleMapPlaceable.Relocate(new List<Cell> {_map[cell.X, cell.Y]});
                cell = path.TryStepForward();
            }
        }
    }
}
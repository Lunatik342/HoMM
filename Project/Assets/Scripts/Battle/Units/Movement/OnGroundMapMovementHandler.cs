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

            foreach (var step in path.Steps)
            {
                await unit.MovementController.MoveToPosition(new Vector2Int(step.X, step.Y));
                unit.BattleMapPlaceable.Relocate(new List<Cell> {_map[step.X, step.Y]});
            }
        }
    }
}
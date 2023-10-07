using Battle.BattleArena.Pathfinding.Movement;
using Battle.Units.Movement;
using UnityEngine;

namespace Battle.BattleArena.Pathfinding
{
    public class Unit
    {
        public MovementType MovementType { get; set; }
        public IUnitMovementController MovementController { get; set; }
        public RotationController RotationController { get; set; }
        public BattleMapPlaceable BattleMapPlaceable { get; set; }
        public GameObject GameObject { get; set; }
    }
}
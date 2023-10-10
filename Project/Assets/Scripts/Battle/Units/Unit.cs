using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Movement;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Pathfinding
{
    public class Unit
    {
        [Inject]
        public IUnitMovementController MovementController { get; set; }
        [Inject]
        public RotationController RotationController { get; set; }
        [Inject]
        public BattleMapPlaceable BattleMapPlaceable { get; set; }
        [Inject]
        public GameObject GameObject { get; set; }
        
        public class Factory : PlaceholderFactory<UnitId, GameObject, MovementType, Unit>
        {
            
        }
    }
}
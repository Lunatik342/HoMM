using System.Collections.Generic;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units;
using Battle.Units.Movement;
using Battle.Units.StatsSystem;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Pathfinding
{
    public class Unit
    {
        [Inject] public Team Team { get; set; }
        [Inject] public GameObject GameObject { get; set; }
        [Inject] public UnitMovementControllerBase MovementController { get; set; }
        [Inject] public RotationController RotationController { get; set; }
        [Inject] public BattleMapPlaceable BattleMapPlaceable { get; set; }
        [Inject] public UnitStatsProvider StatsProvider { get; set; }
        [Inject] public UnitHealth Health { get; set; }
        
        [Inject] private List<IStatsInitializer> _initializableComponents { get; set; }

        public void InitializeStats()
        {
            foreach (var initializableComponent in _initializableComponents)
            {
                initializableComponent.ConfigureStats();
            }
        }

        public class Factory : PlaceholderFactory<GameObject, Team, UnitStaticData, Unit>
        {
            
        }
    }
}
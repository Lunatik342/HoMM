using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units;
using Battle.Units.Components;
using Battle.Units.Movement;
using Battle.Units.StatsSystem;
using RogueSharp;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Pathfinding
{
    public class Unit
    {
        public UnitMovementController MovementController { get; private set; }
        public RotationController RotationController { get; private set; }
        public UnitSimpleActions UnitSimpleActions { get; private set; }
        
        public IUnitPositionProvider PositionProvider { get; private set; }
        public IHealthInfoProvider Health { get; private set; }

        public UnitStatsProvider StatsProvider { get; private set; }
        public Team Team { get; private set; }
        
        private UnitInitializer _unitInitializer;

        public Unit(UnitStatsProvider statsProvider, 
            IHealthInfoProvider healthInfoProvider, 
            RotationController rotationController, 
            UnitSimpleActions unitSimpleActions,
            Team team)
        {
            UnitSimpleActions = unitSimpleActions;
            Health = healthInfoProvider;
            StatsProvider = statsProvider;
            RotationController = rotationController;
            Team = team;
        }

        [Inject] //TODO remove circular dep's
        public void Construct(UnitMovementController unitMovementController, 
            IUnitPositionProvider unitPositionProvider, 
            UnitInitializer unitInitializer)
        {
            MovementController = unitMovementController;
            _unitInitializer = unitInitializer;
            PositionProvider = unitPositionProvider;
        }

        public void Initialize(Cell cell, int count)
        {
            _unitInitializer.Initialize(cell, count);
        }

        public class Factory : PlaceholderFactory<GameObject, Team, UnitStaticData, Unit>
        {
            
        }
    }
}
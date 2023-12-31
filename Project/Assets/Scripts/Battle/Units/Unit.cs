using Algorithms.RogueSharp;
using Battle.StatsSystem;
using Battle.Units.Components;
using Battle.Units.Components.Interfaces;
using Battle.Units.Components.Movement;
using Battle.Units.StaticData;
using UnityEngine;
using Zenject;

namespace Battle.Units
{
    public class Unit
    {
        public UnitMovementController MovementController { get; private set; }
        public RotationController RotationController { get; private set; }
        public UnitSimpleActions UnitSimpleActions { get; private set; }
        public UnitTurnsNotificationsReceiver TurnsNotificationsReceiver { get; private set; }
        public UnitRetaliation Retaliation { get; private set; }
        
        public IUnitPositionProvider PositionProvider { get; private set; }
        public IHealthInfoProvider Health { get; private set; }
        public UnitAttack Attack { get; private set; }

        public UnitStatsProvider StatsProvider { get; private set; }
        public Team Team { get; private set; }
        public UnitId UnitId { get; private set; }
        
        private UnitInitializer _unitInitializer;

        public Unit(UnitStatsProvider statsProvider, 
            RotationController rotationController,
            UnitTurnsNotificationsReceiver unitTurnsNotificationsReceiver,
            UnitRetaliation unitRetaliation,
            Team team,
            UnitId unitId)
        {
            StatsProvider = statsProvider;
            RotationController = rotationController;
            TurnsNotificationsReceiver = unitTurnsNotificationsReceiver;
            Team = team;
            UnitId = unitId;
            Retaliation = unitRetaliation;
        }

        [Inject] //TODO remove circular dependencies
        public void Construct(UnitMovementController unitMovementController, 
            IUnitPositionProvider unitPositionProvider,
            UnitInitializer unitInitializer,
            IHealthInfoProvider healthInfoProvider,
            UnitSimpleActions unitSimpleActions,
            UnitAttack unitAttack)
        {
            MovementController = unitMovementController;
            _unitInitializer = unitInitializer;
            PositionProvider = unitPositionProvider;
            Health = healthInfoProvider;
            UnitSimpleActions = unitSimpleActions;
            Attack = unitAttack;
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
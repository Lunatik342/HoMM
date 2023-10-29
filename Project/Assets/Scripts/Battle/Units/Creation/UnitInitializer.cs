using System.Collections.Generic;
using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using Battle.Units.Creation;
using Battle.Units.Movement;
using RogueSharp;
using UnityEngine;

namespace Battle.Units
{
    public class UnitInitializer
    {
        private readonly UnitHealth _unitHealth;
        private readonly List<IStatsInitializer> _statsInitializers;
        private readonly Transform _rootTransform;
        private readonly RotationController _rotationController;
        private readonly BattleMapPlaceable _battleMapPlaceable;
        private readonly UnitHealthView _unitHealthView;
        private readonly UnitDeathHandler _unitDeathHandler;

        public UnitInitializer(UnitHealth unitHealth, 
            List<IStatsInitializer> statsInitializers, 
            Transform rootTransform, 
            RotationController rotationController,
            BattleMapPlaceable battleMapPlaceable,
            UnitHealthView unitHealthView,
            UnitDeathHandler unitDeathHandler)
        {
            _unitHealth = unitHealth;
            _statsInitializers = statsInitializers;
            _rootTransform = rootTransform;
            _rotationController = rotationController;
            _battleMapPlaceable = battleMapPlaceable;
            _unitHealthView = unitHealthView;
            _unitDeathHandler = unitDeathHandler;
        }

        public void Initialize(Cell cell, int count)
        {
            _battleMapPlaceable.RelocateTo(cell);
            _rootTransform.position = cell.GridPosition.ToBattleArenaWorldPosition();
            _rotationController.LookAtEnemySide();
            
            _statsInitializers.ForEach(s => s.ConfigureStats());
            _unitHealth.SetUnitsCount(count);
            _unitHealthView.Initialize();
            _unitDeathHandler.Initialize();
        }
    }
}
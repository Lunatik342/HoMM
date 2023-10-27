using System.Collections.Generic;
using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Movement;
using Battle.Units.StatsSystem;
using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using RogueSharp;
using UnityEngine;
using Utilities;

namespace Battle.Units
{
    public class UnitSpawner
    {
        private readonly Unit.Factory _unitsFactory;
        private readonly UnitsStaticDataProvider _unitsStaticDataProvider;
        private readonly AssetsLoadingService _assetsLoadingService;
        private readonly IMapHolder _mapHolder;

        public UnitSpawner(Unit.Factory unitsFactory, 
            UnitsStaticDataProvider unitsStaticDataProvider, 
            AssetsLoadingService assetsLoadingService,
            IMapHolder mapHolder)
        {
            _unitsFactory = unitsFactory;
            _unitsStaticDataProvider = unitsStaticDataProvider;
            _assetsLoadingService = assetsLoadingService;
            _mapHolder = mapHolder;
        }
        
        public async UniTask<Unit> Create(UnitCreationParameter unitCreationParameter, Team team)
        {
            var unitStaticData = _unitsStaticDataProvider.ForUnit(unitCreationParameter.UnitId);
            var gridPosition = unitCreationParameter.Position;
            
            var gameObject = await _assetsLoadingService.InstantiateAsync(unitStaticData.GameObjectAssetReference, 
                gridPosition.ToBattleArenaWorldPosition(), Quaternion.identity, null);
            
            var createdUnit = _unitsFactory.Create(gameObject.gameObject, team, unitStaticData);
            
            var size = createdUnit.BattleMapPlaceable.Size;
            
            createdUnit.GameObject.transform.position = TwoDimensionalArrayUtilities.GetWorldPositionFor(gridPosition, size, size);
            createdUnit.RotationController.LookAtEnemySide();
            createdUnit.BattleMapPlaceable.RelocateTo(_mapHolder.Map.GetCell(gridPosition));
            
            createdUnit.StatsProvider.AddStat(StatType.Initiative, unitStaticData.ActingInTurnsQueueStaticData.Initiative);
            createdUnit.InitializeStats();
            
            createdUnit.Health.SetUnitsCount(unitCreationParameter.Count);
            createdUnit.HealthUI.Initialize();

            return createdUnit;
        }
    }
}
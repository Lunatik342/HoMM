using System.Collections.Generic;
using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Movement;
using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using RogueSharp;
using UnityEngine;

namespace Battle.Units
{
    public class UnitsFactory
    {
        private readonly Unit.Factory _factory;
        private readonly UnitsStaticDataProvider _unitsStaticDataProvider;
        private readonly AssetsLoadingService _assetsLoadingService;
        private readonly Map _map;

        public List<Unit> CreatedUnits = new();

        public UnitsFactory(Unit.Factory factory, 
            UnitsStaticDataProvider unitsStaticDataProvider, 
            AssetsLoadingService assetsLoadingService,
            Map map)
        {
            _factory = factory;
            _unitsStaticDataProvider = unitsStaticDataProvider;
            _assetsLoadingService = assetsLoadingService;
            _map = map;
        }
        
        public async UniTask Create(UnitId unitId, Vector2Int position)
        {
            var unitStaticData = _unitsStaticDataProvider.ForUnit(unitId);
            
            var gameObject = await _assetsLoadingService.InstantiateAsync(unitStaticData.GameObjectAssetReference, 
                position.ToBattleArenaWorldPosition(), Quaternion.identity, null);
            
            var createdUnit = _factory.Create(unitId, gameObject.gameObject, MovementType.OnGround);
            createdUnit.GameObject.transform.position = position.ToBattleArenaWorldPosition();
            createdUnit.RotationController.LookAt(createdUnit.GameObject.transform.position + Vector3.right);
            createdUnit.BattleMapPlaceable.RelocateTo(new []{_map[position.x, position.y]});
            
            CreatedUnits.Add(createdUnit);
        }
    }
}
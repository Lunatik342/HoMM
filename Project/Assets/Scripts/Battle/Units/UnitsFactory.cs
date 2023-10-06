using System.Collections.Generic;
using System.Threading.Tasks;
using Battle.BattleArena.Pathfinding.StaticData;
using Battle.Units.Movement;
using Infrastructure.AssetManagement;
using RogueSharp;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.Pathfinding
{
    public class UnitsFactory
    {
        private readonly UnitsStaticDataProvider _unitsStaticDataProvider;
        private readonly AssetsLoadingService _assetsLoadingService;
        private readonly IInstantiator _instantiator;
        private readonly Map _map;

        public UnitsFactory(UnitsStaticDataProvider unitsStaticDataProvider, 
            AssetsLoadingService assetsLoadingService,
            IInstantiator instantiator,
            Map map)
        {
            _unitsStaticDataProvider = unitsStaticDataProvider;
            _assetsLoadingService = assetsLoadingService;
            _instantiator = instantiator;
            _map = map;
        }

        public async Task<Unit> Create(UnitId unitId, Vector2Int position)
        {
            var unit = new Unit();
            
            var placeable = _instantiator.Instantiate<BattleMapPlaceable>(new object[] {1});
            unit.BattleMapPlaceable = placeable;

            unit.MovementType = MovementType.OnGround;
            
            var staticData = _unitsStaticDataProvider.ForUnit(unitId); 
            var view = await _assetsLoadingService.InstantiateAsync<Transform>(staticData.GameObjectAssetReference, 
                position.ToBattleArenaWorldPosition(), Quaternion.identity, null);

            unit.GameObject = view.gameObject;
            unit.MovementController = new GroundUnitMovementController(unit.GameObject);
            
            placeable.Relocate(new List<Cell>{_map[position.x, position.y]});
            
            return unit;
        }
    }
}
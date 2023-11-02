using Battle.Arena.Misc;
using Battle.Arena.StaticData;
using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using Unity.Mathematics;
using UnityEngine;

namespace Battle.Arena.ArenaView
{
    public class BattleFieldViewSpawner
    {
        private readonly BattleArenaStaticDataProvider _staticDataProvider;
        private readonly AssetsLoadingService _assetsLoadingService;

        private Transform _battleFieldView;

        public BattleFieldViewSpawner(
            BattleArenaStaticDataProvider staticDataProvider, 
            AssetsLoadingService assetsLoadingService)
        {
            _staticDataProvider = staticDataProvider;
            _assetsLoadingService = assetsLoadingService;
        }

        public async UniTask Spawn(BattleArenaId battleArenaId)
        {
            var battleFieldStaticData = _staticDataProvider.ForBattleArena(battleArenaId);
            await SpawnBattleFieldView(battleFieldStaticData);
        }

        private async UniTask SpawnBattleFieldView(BattleArenaStaticData arenaStaticData)
        {
            //Taking [0,0] cell's top left corner as starting position for the view
            var gridPosition = new Vector2(-0.5f, -0.5f);
            
            _battleFieldView = await _assetsLoadingService.InstantiateAsync<Transform>(arenaStaticData.ViewGameObjectReference,
                gridPosition.ToBattleArenaWorldPosition(), quaternion.identity, null);
        }
    }
}

using UnityEngine;

namespace Battle.BattleField
{
    public class BattleFieldSpawner
    {
        private readonly BattleFieldStaticDataService _staticDataService;

        public GameObject SpawnedBattleField { get; private set; }

        public BattleFieldSpawner(BattleFieldStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void SpawnBattleField(BattleFieldId battleFieldId)
        {
            var battleFieldStaticData = _staticDataService.GetStaticDataForId(battleFieldId);
            var battleFieldPrefab = battleFieldStaticData.Prefab;

            SpawnedBattleField = GameObject.Instantiate(battleFieldPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}

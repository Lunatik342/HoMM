using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle.Arena.StaticData
{
    [CreateAssetMenu(fileName = "BattleArena", menuName = "StaticData/BattleArena/BattleArena")]
    public class BattleArenaStaticData : ScriptableObject
    {
        [field: SerializeField] public BattleArenaId Id { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject ViewGameObjectReference { get; private set; }
        [field: SerializeField] public ObstacleId[] PossibleObstacles { get; private set; }
        [field: SerializeField] private BattleArenaLayout BattleArenaLayout { get; set; }

        public Vector2Int Size => BattleArenaLayout.Layout.GridSize;
        public bool[,] Layout => BattleArenaLayout.Layout.GetCells();
    }
}
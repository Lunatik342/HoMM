using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle.BattleArena.Pathfinding.StaticData
{
    [CreateAssetMenu(fileName = "Unit", menuName = "StaticData/Units/Unit")]
    public class UnitStaticData: ScriptableObject
    {
        [field: SerializeField] public UnitId UnitId { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject GameObjectAssetReference { get; private set; }
    }
}
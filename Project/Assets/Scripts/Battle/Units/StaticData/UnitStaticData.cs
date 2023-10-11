using Battle.Units.Movement;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle.BattleArena.Pathfinding.StaticData
{
    [CreateAssetMenu(fileName = "Unit", menuName = "StaticData/Units/Unit")]
    public class UnitStaticData: SerializedScriptableObject
    {
        [field: SerializeField] public UnitId UnitId { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject GameObjectAssetReference { get; private set; }
        [field: SerializeField] public UnitRotationStaticData UnitRotationStaticData { get; private set; }
        [field: SerializeField] public UnitGridPlaceableStaticData UnitGridPlaceableStaticData { get; private set; }
        [field: SerializeField] public IUnitComponentBinder MovementStaticData { get; private set; }
    }
}
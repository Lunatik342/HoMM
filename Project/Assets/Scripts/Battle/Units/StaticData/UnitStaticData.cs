using Battle.Units.StaticData.Components;
using Battle.Units.StaticData.Components.Movement;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle.Units.StaticData
{
    [CreateAssetMenu(fileName = "Unit", menuName = "StaticData/Units/Unit")]
    public class UnitStaticData: SerializedScriptableObject
    {
        [field: SerializeField] public UnitId UnitId { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject GameObjectAssetReference { get; private set; }
        [field: SerializeField] public UnitRotationStaticData UnitRotationStaticData { get; private set; }
        [field: SerializeField] public UnitGridPlaceableStaticData UnitGridPlaceableStaticData { get; private set; }
        [field: SerializeField] public MovementStaticData MovementStaticData { get; private set; }
        [field: SerializeField] public DamageReceiverStaticData DamageReceiverStaticData { get; private set; }
        [field: SerializeField] public ActingInTurnsQueueStaticData ActingInTurnsQueueStaticData { get; private set; }
        [field: SerializeField] public AttackDamageDealerStaticData AttackDamageStaticData { get; private set; }
    }
}
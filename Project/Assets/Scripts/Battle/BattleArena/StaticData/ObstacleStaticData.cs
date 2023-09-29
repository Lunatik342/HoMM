using Array2DEditor;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle.BattleArena.StaticData
{
    [CreateAssetMenu(fileName = "Obstacle", menuName = "StaticData/BattleArena/Obstacle")]
    public class ObstacleStaticData : SerializedScriptableObject
    {
        [field: SerializeField] public ObstacleId Id { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject ViewPrefabReference { get; private set; }
        [field: Space(20), InfoBox("Checked cells are occupied by obstacle")] 
        [field: SerializeField] public Array2DBool Figure { get; private set; }
    }
}

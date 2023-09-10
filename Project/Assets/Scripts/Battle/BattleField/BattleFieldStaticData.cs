using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle.BattleField
{
    [CreateAssetMenu(fileName = "BattleFieldStaticData", menuName = "StaticData/BattleFieldView")]
    public class BattleFieldStaticData : ScriptableObject
    {
        [SerializeField] private BattleFieldId _id;
        [SerializeField] private AssetReferenceGameObject _viewGameObjectReference;
        [SerializeField] private BattleFieldSize _size;

        public BattleFieldId Id => _id;
        public AssetReferenceGameObject ViewGameObjectReference => _viewGameObjectReference;
        public Vector2Int Size => BattleFieldSizeSettings.GetSize(_size);
    }

    public static class BattleFieldSizeSettings
    {
        private static readonly Dictionary<BattleFieldSize, Vector2Int> _sizeSettings = new()
        {
            { BattleFieldSize.Standard, new Vector2Int(12, 10) },
            { BattleFieldSize.Siege, new Vector2Int(14, 12) }
        };

        public static Vector2Int GetSize(BattleFieldSize fieldSize)
        {
            return _sizeSettings[fieldSize];
        }
    }

    public enum BattleFieldSize
    {
        Standard = 0,
        Siege = 1
    }
}
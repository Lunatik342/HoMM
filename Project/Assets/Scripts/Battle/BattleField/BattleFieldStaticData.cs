using UnityEngine;

namespace Battle.BattleField
{
    [CreateAssetMenu(fileName = "BattleFieldStaticData", menuName = "StaticData/BattleField")]
    public class BattleFieldStaticData : ScriptableObject
    {
        public BattleFieldId Id;
        public GameObject Prefab;
    }
}
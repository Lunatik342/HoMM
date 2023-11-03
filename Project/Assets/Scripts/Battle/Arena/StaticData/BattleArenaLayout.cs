using Array2DEditor;
using UnityEngine;

namespace Battle.Arena.StaticData
{
    [CreateAssetMenu(fileName = "BattleArenaLayout", menuName = "StaticData/BattleArena/BattleArenaLayout")]
    public class BattleArenaLayout: ScriptableObject
    {
        [field: Tooltip("Checked cells are unwalkable")]
        [field: SerializeField] public Array2DBool Layout { get; private set; }
    }
}
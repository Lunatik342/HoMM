using UnityEngine;

namespace Battle.BattleArena
{
    public static class BattleArenaUtilities
    {
        public static Vector3 ToBattleArenaWorldPosition(this Vector2Int battleArenaGridPosition)
        {
            return ToBattleArenaWorldPosition((Vector2) battleArenaGridPosition);
        }
        
        public static Vector3 ToBattleArenaWorldPosition(this Vector2 battleArenaGridPosition)
        {
            return new Vector3(battleArenaGridPosition.x * BattleArenaConstants.CellSizeInUnits, 0, -1 * battleArenaGridPosition.y * BattleArenaConstants.CellSizeInUnits);
        }
    }
}
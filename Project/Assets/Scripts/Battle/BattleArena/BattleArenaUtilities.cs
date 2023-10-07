using System;
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

        public static Vector2Int ToMapCellCoordinates(this Vector3 worldPosition)
        {
            return new Vector2Int((int)Math.Round(worldPosition.x), (int)-Math.Round(worldPosition.z));
        }
    }
}
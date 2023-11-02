using System;
using Algorithms.RogueSharp;
using UnityEngine;

namespace Battle.Arena.Misc
{
    public static class BattleArenaUtilities
    {
        public static Vector3 ToBattleArenaWorldPosition(this Cell cell)
        {
            return GetWorldPositionFromMapCoordinates(cell.X, cell.Y);
        }
        
        public static Vector3 ToBattleArenaWorldPosition(this Vector2Int battleArenaGridPosition)
        {
            return GetWorldPositionFromMapCoordinates(battleArenaGridPosition.x, battleArenaGridPosition.y);
        }

        public static Vector3 ToBattleArenaWorldPosition(this Vector2 battleArenaGridPosition)
        {
            return GetWorldPositionFromMapCoordinates(battleArenaGridPosition.x, battleArenaGridPosition.y);
        }

        public static Vector2Int ToMapCellCoordinates(this Vector3 worldPosition)
        {
            return new Vector2Int((int)Math.Round(worldPosition.x), (int)-Math.Round(worldPosition.z));
        }

        private static Vector3 GetWorldPositionFromMapCoordinates(float x, float y)
        {
            return new Vector3(x * BattleArenaConstants.CellSizeInUnits, 0, -1 * y * BattleArenaConstants.CellSizeInUnits);
        }
    }
}
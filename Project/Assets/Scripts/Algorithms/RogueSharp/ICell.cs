using Battle.Units;
using UnityEngine;

namespace Algorithms.RogueSharp
{
   /// <summary>
   /// A interface ONLY for pathfinding, solves the task of finding the path for units of different sizes
   /// </summary>
   public interface ICell
   {
      int X { get; set; }
      int Y { get; set; }
      public Vector2Int GridPosition { get; }

      bool IsWalkableByUnit(Unit placeableEntity);
      public bool CanPlaceUnit(Unit placeableEntity);

      Vector3 GetWorldPosition();
      Cell GetLogicalCell();
   }
}
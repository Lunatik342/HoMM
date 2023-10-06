using System;
using Battle.BattleArena.Pathfinding;

namespace RogueSharp
{
   /// <summary>
   /// A class that defines a square on a Map with all of its associated properties
   /// </summary>
   public interface ICell
   {
      /// <summary>
      /// Gets the X location of the Cell starting with 0 as the farthest left
      /// </summary>
      int X { get; set; }

      /// <summary>
      /// Y location of the Cell starting with 0 as the top
      /// </summary>
      int Y { get; set; }

      bool IsWalkableByEntity(BattleMapPlaceable placeableEntity);
   }
}
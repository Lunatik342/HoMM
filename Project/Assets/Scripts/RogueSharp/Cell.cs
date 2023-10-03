using System;
using Battle.BattleArena.Pathfinding;

namespace RogueSharp
{
   public class Cell : ICell
   {
      public int X { get; set; }
      public int Y { get; set; }
      public bool IsFunctioning { get; set; }
      public bool IsOccupied => PlacedEntity != null;
      public IBattleGridPlaceable PlacedEntity { get; private set; }

      public bool IsWalkableByEntity(IBattleGridPlaceable placeableEntity)
      {
         if (!IsFunctioning)
         {
            return false;
         }

         if (PlacedEntity != null && PlacedEntity != placeableEntity)
         {
            return false;
         }
         
         return true;
      }

      public bool CanPlaceEntity(IBattleGridPlaceable placeableEntity)
      {
         return IsWalkableByEntity(placeableEntity);
      }

      public void PlaceEntity(IBattleGridPlaceable placeableEntity)
      {
         if (!CanPlaceEntity(placeableEntity))
         {
            throw new InvalidOperationException("Cannot place entity on this cell");
         }

         PlacedEntity = placeableEntity;
      }

      public override string ToString()
      {
         if (!IsFunctioning)
         {
            return " ";
         }

         if (IsOccupied)
         {
            return "X";
         }
         
         return "O";
      }
   }
}
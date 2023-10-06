using System;
using Battle.BattleArena.Pathfinding;

namespace RogueSharp
{
   public class Cell : ICell
   {
      public int X { get; set; }
      public int Y { get; set; }
      public bool IsFunctioning { get; set; }
      public bool IsOccupiedByObstacle { get; set; }
      public bool IsOccupiedByEntity => PlacedEntity != null;
      public BattleMapPlaceable PlacedEntity { get; private set; }

      public bool IsWalkableByEntity(BattleMapPlaceable placeableEntity)
      {
         if (!IsFunctioning || IsOccupiedByObstacle)
         {
            return false;
         }

         if (PlacedEntity != null && PlacedEntity != placeableEntity)
         {
            return false;
         }
         
         return true;
      }

      public bool CanPlaceEntity(BattleMapPlaceable placeableEntity)
      {
         return IsWalkableByEntity(placeableEntity);
      }

      public void PlaceEntity(BattleMapPlaceable placeableEntity)
      {
         if (!CanPlaceEntity(placeableEntity))
         {
            throw new InvalidOperationException("Cannot place entity on this cell");
         }

         PlacedEntity = placeableEntity;
      }

      public void RemoveEntity(BattleMapPlaceable placeableEntity)
      {
         if (PlacedEntity == placeableEntity)
         {
            PlacedEntity = null;
         }
      }

      public override string ToString()
      {
         if (!IsFunctioning)
         {
            return " ";
         }
         
         if (!IsOccupiedByObstacle)
         {
            return "■";
         }

         if (IsOccupiedByEntity)
         {
            return "▣";
         }
         
         return "□";
      }
   }
}
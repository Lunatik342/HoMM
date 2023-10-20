using System;
using System.Collections.Generic;
using Battle.BattleArena;
using Battle.BattleArena.Pathfinding;
using UnityEngine;

namespace RogueSharp
{
   public class Cell : ICell
   {
      public int X { get; set; }
      public int Y { get; set; }
      public bool IsFunctioning { get; set; }
      public bool IsOccupiedByObstacle { get; set; }
      public bool IsOccupiedByEntity => PlacedUnit != null;
      public Vector2Int GridPosition => new(X, Y);
      public Unit PlacedUnit { get; private set; }
      
      public bool IsWalkableByUnit(Unit placeableEntity)
      {
         if (!IsFunctioning || IsOccupiedByObstacle)
         {
            return false;
         }

         if (PlacedUnit != null && PlacedUnit != placeableEntity)
         {
            return false;
         }
         
         return true;
      }

      public bool CanPlaceUnit(Unit placeableEntity)
      {
         return IsWalkableByUnit(placeableEntity);
      }

      public void PlaceEntity(Unit unit)
      {
         if (!CanPlaceUnit(unit))
         {
            throw new InvalidOperationException("Cannot place entity on this cell");
         }

         PlacedUnit = unit;
      }

      public void RemoveEntity(Unit unit)
      {
         if (PlacedUnit == unit)
         {
            PlacedUnit = null;
         }
      }
      
      Vector3 ICell.GetWorldPosition()
      {
         return this.ToBattleArenaWorldPosition();
      }

      List<Cell> ICell.GetLogicalCells()
      {
         return new List<Cell> { this };
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
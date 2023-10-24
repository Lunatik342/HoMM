﻿using System.Collections.Generic;
using Battle.BattleArena.Pathfinding;
using UnityEngine;

namespace RogueSharp
{
   /// <summary>
   /// A interface ONLY for pathfinding, solves the task of finding the path for units of different sizes
   /// </summary>
   public interface ICell
   {
      int X { get; set; }
      int Y { get; set; }

      bool IsWalkableByUnit(Unit placeableEntity);
      public bool CanPlaceUnit(Unit placeableEntity);

      Vector3 GetWorldPosition();
      Cell GetLogicalCell();
   }
}
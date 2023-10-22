using System;
using Battle.BattleArena.Pathfinding;

namespace RogueSharp.Algorithms
{
   
   //Based on AStar (I just removed some code and added some code), very lazy implementation but it works
   public class ReachableCellsFinder<TCell> where TCell : ICell
   {
      private readonly double _diagonalCost;

      public ReachableCellsFinder( double diagonalCost )
      {
         _diagonalCost = diagonalCost;
      }

      public bool[] GetReachableCells( TCell source, IMap<TCell> map , Unit pathingAgent, int maxDistanceFormStart)
      {
         IndexMinPriorityQueue<PathNode> openNodes = new IndexMinPriorityQueue<PathNode>( map.Height * map.Width );
         bool[] isNodeClosed = new bool[map.Height * map.Width];
         
         openNodes.Insert( map.IndexFor( source ), new PathNode
         {
            DistanceFromStart = 0,
            X = source.X,
            Y = source.Y,
            Parent = null
         } );

         while ( true )
         {
            if ( openNodes.Size < 1 )
            {
               return isNodeClosed;
            }
            
            var currentNode = openNodes.MinKey();
            int currentIndex = openNodes.DeleteMin();
            isNodeClosed[currentIndex] = true;

            ICell currentCell = map.CellFor( currentIndex );
            
            foreach ( TCell neighbor in map.GetAdjacentCells( currentCell.X, currentCell.Y, true ) )
            {
               int neighborIndex = map.IndexFor( neighbor );
               if ( neighbor.IsWalkableByUnit(pathingAgent) == false || isNodeClosed[neighborIndex] )
               {
                  continue;
               }

               bool isNeighborInOpen = openNodes.Contains( neighborIndex );

               if ( isNeighborInOpen )
               {
                  PathNode neighborNode = openNodes.KeyAt( neighborIndex );
                  double newDistance = currentNode.DistanceFromStart + GetTravelCost(neighborNode.X, neighborNode.Y, currentNode.X, currentNode.Y);
                  if ( newDistance < neighborNode.DistanceFromStart )
                  {
                     neighborNode.DistanceFromStart = newDistance;
                     neighborNode.Parent = currentNode;
                  }
               }
               else
               {
                  var travelCost = GetTravelCost(neighbor.X, neighbor.Y, currentNode.X, currentNode.Y);
                  
                  if (currentNode.DistanceFromStart + travelCost > maxDistanceFormStart)
                  {
                     continue;
                  }
                  
                  PathNode neighborNode = new PathNode
                  {
                     DistanceFromStart = currentNode.DistanceFromStart + travelCost,
                     X = neighbor.X,
                     Y = neighbor.Y,
                     Parent = currentNode
                  };
                  
                  openNodes.Insert( neighborIndex, neighborNode );
               }
            }
         }
      }

      private double GetTravelCost(int x1, int y1, int x2, int y2)
      {
         //If they are diagonal
         if (x1 != x2 && y1 != y2)
         {
            return _diagonalCost;
         }

         return 1d;
      }

      private class PathNode : IComparable<PathNode>
      {
         public int X { get; set; }
         public int Y { get; set; }
         public double DistanceFromStart { get; set; }
         public PathNode Parent { get; set; }

         public int CompareTo( PathNode other )
         {
            if ( ReferenceEquals( this, other ) )
            {
               return 0;
            }

            if ( ReferenceEquals( null, other ) )
            {
               return 1;
            }

            return DistanceFromStart.CompareTo( other.DistanceFromStart );
         }
      }
   }
}

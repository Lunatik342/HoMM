using System;
using System.Collections.Generic;
using Battle.BattleArena.Pathfinding;

namespace RogueSharp.Algorithms
{
   /// <summary>
   /// The AStarShortestPath class represents a data type for finding the shortest path between two Cells on a Map
   /// </summary>
   public class KekPath<TCell> where TCell : ICell
   {
      private readonly double? _diagonalCost;

      /// <summary>
      /// Construct a new class for computing the shortest path between two Cells on a Map using the A* algorithm
      /// Using this constructor will not allow diagonal movement. Use the overloaded constructor with diagonalCost if diagonal movement is allowed.
      /// </summary>
      public KekPath()
      {
      }
      
      /// <summary>
      /// Construct a new class for computing the shortest path between two Cells on a Map using the A* algorithm
      /// </summary>
      /// <param name="diagonalCost">
      /// The cost of diagonal movement compared to horizontal or vertical movement.
      /// Use 1.0 if you want the same cost for all movements.
      /// On a standard cartesian map, it should be sqrt(2) (1.41)
      /// </param>
      public KekPath( double diagonalCost )
      {
         _diagonalCost = diagonalCost;
      }

      /// <summary>
      /// Returns an List of Cells representing a shortest path from the specified source to the specified destination
      /// </summary>
      /// <param name="source">The source Cell to find a shortest path from</param>
      /// <param name="destination">The destination Cell to find a shortest path to</param>
      /// <param name="map">The Map on which to find the shortest path between Cells</param>
      /// <param name="pathingAgent">Agent that searches a path</param>
      /// <returns>List of Cells representing a shortest path from the specified source to the specified destination. If no path is found null will be returned</returns>
      public bool[] FindPath( TCell source, IMap<TCell> map , BattleMapPlaceable pathingAgent, int maxKek)
      {
         IndexMinPriorityQueue<PathNode> openNodes = new IndexMinPriorityQueue<PathNode>( map.Height * map.Width );
         bool[] isNodeClosed = new bool[map.Height * map.Width];
         bool[] result = new bool[map.Height * map.Width];
         
         openNodes.Insert( map.IndexFor( source ), new PathNode
         {
            DistanceFromStart = 0,
            X = source.X,
            Y = source.Y,
            Parent = null
         } );

         PathNode currentNode;
         
         while ( true )
         {
            if ( openNodes.Size < 1 )
            {
               return result;
            }
            currentNode = openNodes.MinKey();
            int currentIndex = openNodes.DeleteMin();
            isNodeClosed[currentIndex] = true;
            result[currentIndex] = currentNode.DistanceFromStart <= maxKek;

            ICell currentCell = map.CellFor( currentIndex );

            bool includeDiagonals = _diagonalCost.HasValue;
            
            foreach ( TCell neighbor in map.GetAdjacentCells( currentCell.X, currentCell.Y, includeDiagonals ) )
            {
               int neighborIndex = map.IndexFor( neighbor );
               if ( neighbor.IsWalkableByEntity(pathingAgent) == false || isNodeClosed[neighborIndex] )
               {
                  continue;
               }

               bool isNeighborInOpen = openNodes.Contains( neighborIndex );

               if ( isNeighborInOpen )
               {
                  PathNode neighborNode = openNodes.KeyAt( neighborIndex );
                  double newDistance = currentNode.DistanceFromStart + GetDist(neighborNode.X, neighborNode.Y, currentNode);
                  if ( newDistance < neighborNode.DistanceFromStart )
                  {
                     neighborNode.DistanceFromStart = newDistance;
                     neighborNode.Parent = currentNode;
                     
                     result[map.IndexFor(neighborNode.X, neighborNode.Y)] = neighborNode.DistanceFromStart <= maxKek;
                  }
               }
               else
               {
                  if (currentNode.DistanceFromStart + GetDist(neighbor.X, neighbor.Y, currentNode) > maxKek)
                  {
                     continue;
                  }
                  
                  PathNode neighborNode = new PathNode
                  {
                     DistanceFromStart = currentNode.DistanceFromStart + GetDist(neighbor.X, neighbor.Y, currentNode),
                     X = neighbor.X,
                     Y = neighbor.Y,
                     Parent = currentNode
                  };
                  
                  result[map.IndexFor(neighborNode.X, neighborNode.Y)] = neighborNode.DistanceFromStart <= maxKek;
                  openNodes.Insert( neighborIndex, neighborNode );
               }
            }
         }
      }

      private float GetDist(int x, int y, PathNode p2)
      {
         if (x != p2.X && y != p2.Y)
         {
            return 1.41f;
         }

         return 1f;
      }
      
      
      

      private static double CalculateDistance( TCell source, TCell destination )
      {
         int dx = Math.Abs( source.X - destination.X );
         int dy = Math.Abs( source.Y - destination.Y );

         return dx + dy;
      }

      private static double CalculateDistance( TCell source, TCell destination, double? diagonalCost )
      {
         if ( !diagonalCost.HasValue )
         {
            return CalculateDistance( source, destination );
         }
         int dx = Math.Abs( source.X - destination.X );
         int dy = Math.Abs( source.Y - destination.Y );

         int dMin = Math.Min( dx, dy );
         int dMax = Math.Max( dx, dy );

         return ( dMin * diagonalCost.Value ) + ( dMax - dMin );
      }

      private class PathNode : IComparable<PathNode>
      {
         public int X
         {
            get;
            set;
         }

         public int Y
         {
            get;
            set;
         }

         // G cost = distance from starting node
         public double DistanceFromStart
         {
            get;
            set;
         }

         public PathNode Parent
         {
            get;
            set;
         }

         // F cost = G cost + H cost
         private double Cost => DistanceFromStart;

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

            return Cost.CompareTo( other.Cost );
         }
      }
   }
}

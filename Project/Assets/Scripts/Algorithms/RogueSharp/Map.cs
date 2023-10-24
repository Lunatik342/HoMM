using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueSharp
{
   /// <summary>
   /// A Map represents a rectangular grid of Cells, each of which has a number of properties for determining walkability and so on
   /// The upper left corner of the Map is Cell (0,0) and the X value increases to the right, as the Y value increases downward
   /// </summary>
   public class Map : Map<Cell>, IMap
   {
      /// <summary>
      /// Constructor creates a new uninitialized Map
      /// </summary>
      public Map()
      {
      }

      /// <summary>
      /// Constructor creates a new Map and immediately initializes it
      /// </summary>
      /// <param name="width">How many Cells wide the Map will be</param>
      /// <param name="height">How many Cells tall the Map will be</param>
      public Map( int width, int height )
      : base( width, height )
      {
      }
   }

   /// <summary>
   /// A Map represents a rectangular grid of Cells, each of which has a number of properties for determining walkability, field-of-view and so on
   /// The upper left corner of the Map is Cell (0,0) and the X value increases to the right, as the Y value increases downward
   /// </summary>
   public class Map<TCell> : IMap<TCell> where TCell : ICell
   {
      private TCell[,] _cells;

      /// <summary>
      /// Constructor creates a new uninitialized Map
      /// </summary>
      public Map()
      {
      }

      /// <summary>
      /// Constructor creates a new Map and immediately initializes it
      /// </summary>
      /// <param name="width">How many Cells wide the Map will be</param>
      /// <param name="height">How many Cells tall the Map will be</param>
      public Map( int width, int height )
      {
         Init( width, height );
      }

      /// <summary>
      /// This Indexer allows direct access to Cells given x and y index
      /// </summary>
      /// <param name="x">X index of the Cell to get</param>
      /// <param name="y">Y index of the Cell to get</param>
      /// <returns>Cell at the specified index</returns>
      public TCell this[int x, int y]
      {
         get => _cells[x, y];
         set => _cells[x, y] = value;
      }

      public TCell GetCell(Vector2Int position)
      {
         if (position.x < 0 || position.y < 0 || position.x >= _cells.GetLength(0) || position.y >= _cells.GetLength(1))
         {
            return default;
         }
         
         return this[position.x, position.y];
      }

      /// <summary>
      /// How many Cells wide the Map is
      /// </summary>
      /// <remarks>
      /// A Map with a Width of 10 will have Cells with X locations of 0 through 9
      /// Cells with an X value of 0 will be the leftmost Cells
      /// </remarks>
      public int Width
      {
         get; private set;
      }

      /// <summary>
      /// How many Cells tall the Map is
      /// </summary>
      /// <remarks>
      /// A Map with a Height of 20 will have Cells with Y locations of 0 through 19
      /// Cells with an Y value of 0 will be the topmost Cells
      /// </remarks>
      public int Height
      {
         get; private set;
      }

      /// <summary>
      /// Create a new map with the properties of all Cells set to false
      /// </summary>
      /// <remarks>
      /// This is basically a solid stone map that would then need to be modified to have interesting features. Override to initialize other internal state
      /// </remarks>
      /// <param name="width">How many Cells wide the Map will be</param>
      /// <param name="height">How many Cells tall the Map will be</param>
      public virtual void Initialize( int width, int height )
      {
         Init( width, height );
      }

      private void Init( int width, int height )
      {
         Width = width;
         Height = height;
         _cells = new TCell[width, height];
         for ( int x = 0; x < width; x++ )
         {
            for ( int y = 0; y < height; y++ )
            {
               _cells[x, y] = Activator.CreateInstance<TCell>();
               _cells[x, y].X = x;
               _cells[x, y].Y = y;
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of all Cells in the Map
      /// </summary>
      /// <returns>IEnumerable of all Cells in the Map</returns>
      public IEnumerable<TCell> GetAllCells()
      {
         for ( int y = 0; y < Height; y++ )
         {
            for ( int x = 0; x < Width; x++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of Cells in a line from the Origin Cell to the Destination Cell
      /// The resulting IEnumerable includes the Origin and Destination Cells
      /// Uses Bresenham's line algorithm to determine which Cells are in the closest approximation to a straight line between the two Cells
      /// </summary>
      /// <param name="xOrigin">X location of the Origin Cell at the start of the line with 0 as the farthest left</param>
      /// <param name="yOrigin">Y location of the Origin Cell at the start of the line with 0 as the top</param>
      /// <param name="xDestination">X location of the Destination Cell at the end of the line with 0 as the farthest left</param>
      /// <param name="yDestination">Y location of the Destination Cell at the end of the line with 0 as the top</param>
      /// <returns>IEnumerable of Cells in a line from the Origin Cell to the Destination Cell which includes the Origin and Destination Cells</returns>
      public IEnumerable<TCell> GetCellsAlongLine( int xOrigin, int yOrigin, int xDestination, int yDestination )
      {
         xOrigin = ClampX( xOrigin );
         yOrigin = ClampY( yOrigin );
         xDestination = ClampX( xDestination );
         yDestination = ClampY( yDestination );

         int dx = Math.Abs( xDestination - xOrigin );
         int dy = Math.Abs( yDestination - yOrigin );

         int sx = xOrigin < xDestination ? 1 : -1;
         int sy = yOrigin < yDestination ? 1 : -1;
         int err = dx - dy;

         while ( true )
         {
            yield return GetCell( xOrigin, yOrigin );
            if ( xOrigin == xDestination && yOrigin == yDestination )
            {
               break;
            }
            int e2 = 2 * err;
            if ( e2 > -dy )
            {
               err = err - dy;
               xOrigin = xOrigin + sx;
            }
            if ( e2 < dx )
            {
               err = err + dx;
               yOrigin = yOrigin + sy;
            }
         }
      }

      private int ClampX( int x )
      {
         return ( x < 0 ) ? 0 : ( x > Width - 1 ) ? Width - 1 : x;
      }

      private int ClampY( int y )
      {
         return ( y < 0 ) ? 0 : ( y > Height - 1 ) ? Height - 1 : y;
      }

      /// <summary>
      /// Get an IEnumerable of Cells in a circle around the center Cell up to the specified radius using Bresenham's midpoint circle algorithm
      /// </summary>
      /// <seealso href="https://en.wikipedia.org/wiki/Midpoint_circle_algorithm">Based on Bresenham's midpoint circle algorithm</seealso>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="radius">The number of Cells to get in a radius from the center Cell</param>
      /// <returns>IEnumerable of Cells in a circle around the center Cell</returns>
      public IEnumerable<TCell> GetCellsInCircle( int xCenter, int yCenter, int radius )
      {
         var discovered = new HashSet<int>();

         int d = ( 5 - ( radius * 4 ) ) / 4;
         int x = 0;
         int y = radius;

         do
         {
            foreach ( TCell cell in GetCellsAlongLine( xCenter + x, yCenter + y, xCenter - x, yCenter + y ) )
            {
               if ( AddToHashSet( discovered, cell ) )
               {
                  yield return cell;
               }
            }
            foreach ( TCell cell in GetCellsAlongLine( xCenter - x, yCenter - y, xCenter + x, yCenter - y ) )
            {
               if ( AddToHashSet( discovered, cell ) )
               {
                  yield return cell;
               }
            }
            foreach ( TCell cell in GetCellsAlongLine( xCenter + y, yCenter + x, xCenter - y, yCenter + x ) )
            {
               if ( AddToHashSet( discovered, cell ) )
               {
                  yield return cell;
               }
            }
            foreach ( TCell cell in GetCellsAlongLine( xCenter + y, yCenter - x, xCenter - y, yCenter - x ) )
            {
               if ( AddToHashSet( discovered, cell ) )
               {
                  yield return cell;
               }
            }

            if ( d < 0 )
            {
               d += ( 2 * x ) + 1;
            }
            else
            {
               d += ( 2 * ( x - y ) ) + 1;
               y--;
            }
            x++;
         } while ( x <= y );
      }

      /// <summary>
      /// Get an IEnumerable of Cells in a diamond (Rhombus) shape around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in a distance from the center Cell</param>
      /// <returns>IEnumerable of Cells in a diamond (Rhombus) shape around the center Cell</returns>
      public IEnumerable<TCell> GetCellsInDiamond( int xCenter, int yCenter, int distance )
      {
         var discovered = new HashSet<int>();

         int xMin = Math.Max( 0, xCenter - distance );
         int xMax = Math.Min( Width - 1, xCenter + distance );
         int yMin = Math.Max( 0, yCenter - distance );
         int yMax = Math.Min( Height - 1, yCenter + distance );

         for ( int i = 0; i <= distance; i++ )
         {
            for ( int j = distance; j >= 0 + i; j-- )
            {
               if ( AddToHashSet( discovered, Math.Max( xMin, xCenter - i ), Math.Min( yMax, yCenter + distance - j ), out TCell cell ) )
               {
                  yield return cell;
               }
               if ( AddToHashSet( discovered, Math.Max( xMin, xCenter - i ), Math.Max( yMin, yCenter - distance + j ), out cell ) )
               {
                  yield return cell;
               }
               if ( AddToHashSet( discovered, Math.Min( xMax, xCenter + i ), Math.Min( yMax, yCenter + distance - j ), out cell ) )
               {
                  yield return cell;
               }
               if ( AddToHashSet( discovered, Math.Min( xMax, xCenter + i ), Math.Max( yMin, yCenter - distance + j ), out cell ) )
               {
                  yield return cell;
               }
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of Cells in a square area around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in each direction from the center Cell</param>
      /// <returns>IEnumerable of Cells in a square area around the center Cell</returns>
      public IEnumerable<TCell> GetCellsInSquare( int xCenter, int yCenter, int distance )
      {
         int xMin = Math.Max( 0, xCenter - distance );
         int xMax = Math.Min( Width - 1, xCenter + distance );
         int yMin = Math.Max( 0, yCenter - distance );
         int yMax = Math.Min( Height - 1, yCenter + distance );

         for ( int y = yMin; y <= yMax; y++ )
         {
            for ( int x = xMin; x <= xMax; x++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of Cells in a rectangle area
      /// </summary>
      /// <param name="top">The top row of the rectangle </param>
      /// <param name="left">The left column of the rectangle</param>
      /// <param name="width">The width of the rectangle</param>
      /// <param name="height">The height of the rectangle</param>
      /// <returns>IEnumerable of Cells in a rectangle area</returns>
      public IEnumerable<TCell> GetCellsInRectangle( int top, int left, int width, int height )
      {
         int xMin = Math.Max( 0, left );
         int xMax = Math.Min( Width, left + width );
         int yMin = Math.Max( 0, top );
         int yMax = Math.Min( Height, top + height );

         for ( int y = yMin; y < yMax; y++ )
         {
            for ( int x = xMin; x < xMax; x++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of outermost border Cells in a circle around the center Cell up to the specified radius using Bresenham's midpoint circle algorithm
      /// </summary>
      /// <seealso href="https://en.wikipedia.org/wiki/Midpoint_circle_algorithm">Based on Bresenham's midpoint circle algorithm</seealso>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="radius">The number of Cells to get in a radius from the center Cell</param>
      /// <returns>IEnumerable of outermost border Cells in a circle around the center Cell</returns>
      public IEnumerable<TCell> GetBorderCellsInCircle( int xCenter, int yCenter, int radius )
      {
         var discovered = new HashSet<int>();

         int d = ( 5 - ( radius * 4 ) ) / 4;
         int x = 0;
         int y = radius;

         TCell centerCell = GetCell( xCenter, yCenter );

         do
         {
            if ( AddToHashSet( discovered, ClampX( xCenter + x ), ClampY( yCenter + y ), centerCell, out TCell cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter + x ), ClampY( yCenter - y ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter - x ), ClampY( yCenter + y ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter - x ), ClampY( yCenter - y ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter + y ), ClampY( yCenter + x ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter + y ), ClampY( yCenter - x ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter - y ), ClampY( yCenter + x ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, ClampX( xCenter - y ), ClampY( yCenter - x ), centerCell, out cell ) )
            {
               yield return cell;
            }

            if ( d < 0 )
            {
               d += ( 2 * x ) + 1;
            }
            else
            {
               d += ( 2 * ( x - y ) ) + 1;
               y--;
            }
            x++;
         } while ( x <= y );
      }

      /// <summary>
      /// Get an IEnumerable of outermost border Cells in a diamond (Rhombus) shape around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in a distance from the center Cell</param>
      /// <returns>IEnumerable of outermost border Cells in a diamond (Rhombus) shape around the center Cell</returns>
      public IEnumerable<TCell> GetBorderCellsInDiamond( int xCenter, int yCenter, int distance )
      {
         var discovered = new HashSet<int>();

         int xMin = Math.Max( 0, xCenter - distance );
         int xMax = Math.Min( Width - 1, xCenter + distance );
         int yMin = Math.Max( 0, yCenter - distance );
         int yMax = Math.Min( Height - 1, yCenter + distance );

         TCell centerCell = GetCell( xCenter, yCenter );
         if ( AddToHashSet( discovered, xCenter, yMin, centerCell, out TCell cell ) )
         {
            yield return cell;
         }
         if ( AddToHashSet( discovered, xCenter, yMax, centerCell, out cell ) )
         {
            yield return cell;
         }
         for ( int i = 1; i <= distance; i++ )
         {
            if ( AddToHashSet( discovered, Math.Max( xMin, xCenter - i ), Math.Min( yMax, yCenter + distance - i ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, Math.Max( xMin, xCenter - i ), Math.Max( yMin, yCenter - distance + i ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, Math.Min( xMax, xCenter + i ), Math.Min( yMax, yCenter + distance - i ), centerCell, out cell ) )
            {
               yield return cell;
            }
            if ( AddToHashSet( discovered, Math.Min( xMax, xCenter + i ), Math.Max( yMin, yCenter - distance + i ), centerCell, out cell ) )
            {
               yield return cell;
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of outermost border Cells in a square area around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in each direction from the center Cell</param>
      /// <returns>IEnumerable of outermost border Cells in a square area around the center Cell</returns>
      public IEnumerable<TCell> GetBorderCellsInSquare( int xCenter, int yCenter, int distance )
      {
         int xMin = Math.Max( 0, xCenter - distance );
         int xMax = Math.Min( Width - 1, xCenter + distance );
         int yMin = Math.Max( 0, yCenter - distance );
         int yMax = Math.Min( Height - 1, yCenter + distance );
         List<TCell> borderCells = new List<TCell>();

         for ( int x = xMin; x <= xMax; x++ )
         {
            borderCells.Add( GetCell( x, yMin ) );
            borderCells.Add( GetCell( x, yMax ) );
         }
         for ( int y = yMin + 1; y <= yMax - 1; y++ )
         {
            borderCells.Add( GetCell( xMin, y ) );
            borderCells.Add( GetCell( xMax, y ) );
         }

         TCell centerCell = GetCell( xCenter, yCenter );
         borderCells.Remove( centerCell );

         return borderCells;
      }

      /// <summary>
      /// Get an IEnumerable of all the Cells in the specified row numbers
      /// </summary>
      /// <param name="rowNumbers">Integer array of row numbers with 0 as the top</param>
      /// <returns>IEnumerable of all the Cells in the specified row numbers</returns>
      public IEnumerable<TCell> GetCellsInRows( params int[] rowNumbers )
      {
         foreach ( int y in rowNumbers )
         {
            for ( int x = 0; x < Width; x++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of all the Cells in the specified column numbers
      /// </summary>
      /// <param name="columnNumbers">Integer array of column numbers with 0 as the farthest left</param>
      /// <returns>IEnumerable of all the Cells in the specified column numbers</returns>
      public IEnumerable<TCell> GetCellsInColumns( params int[] columnNumbers )
      {
         foreach ( int x in columnNumbers )
         {
            for ( int y = 0; y < Height; y++ )
            {
               yield return GetCell( x, y );
            }
         }
      }

      /// <summary>
      /// Get an IEnumerable of adjacent Cells which touch the center Cell. Diagonal cells do not count as adjacent.
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <returns>IEnumerable of adjacent Cells which touch the center Cell</returns>
      public IEnumerable<TCell> GetAdjacentCells( int xCenter, int yCenter )
      {
         return GetAdjacentCells( xCenter, yCenter, false );
      }

      /// <summary>
      /// Get an IEnumerable of adjacent Cells which touch the center Cell. Diagonal cells may optionally be included.
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="includeDiagonals">Should diagonal Cells count as being adjacent cells?</param>
      /// <returns>IEnumerable of adjacent Cells which touch the center Cell</returns>
      public IEnumerable<TCell> GetAdjacentCells( int xCenter, int yCenter, bool includeDiagonals )
      {
         int topY = yCenter - 1;
         int bottomY = yCenter + 1;
         int leftX = xCenter - 1;
         int rightX = xCenter + 1;

         if ( topY >= 0 )
         {
            yield return GetCell( xCenter, topY );
         }

         if ( leftX >= 0 )
         {
            yield return GetCell( leftX, yCenter );
         }

         if ( bottomY < Height )
         {
            yield return GetCell( xCenter, bottomY );
         }

         if ( rightX < Width )
         {
            yield return GetCell( rightX, yCenter );
         }

         if ( includeDiagonals )
         {
            if ( rightX < Width && topY >= 0 )
            {
               yield return GetCell( rightX, topY );
            }

            if ( rightX < Width && bottomY < Height )
            {
               yield return GetCell( rightX, bottomY );
            }

            if ( leftX >= 0 && topY >= 0 )
            {
               yield return GetCell( leftX, topY );
            }

            if ( leftX >= 0 && bottomY < Height )
            {
               yield return GetCell( leftX, bottomY );
            }
         }
      }

      /// <summary>
      /// Get a Cell at the specified location
      /// </summary>
      /// <param name="x">X location of the Cell to get starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to get, starting with 0 as the top</param>
      /// <returns>Cell at the specified location</returns>
      public TCell GetCell( int x, int y )
      {
         return _cells[x, y];
      }

      /// <summary>
      /// Provides a simple visual representation of the map using the following symbols:
      /// - `s`: `Cell` is walkable
      /// - `#`: `Cell` is not walkable
      /// </summary>
      /// <returns>A string representation of the map using special symbols to denote Cell properties</returns>
      public override string ToString()
      {
         var mapRepresentation = new StringBuilder();
         int lastY = 0;
         foreach ( ICell iCell in GetAllCells() )
         {
            Cell cell = (Cell) iCell;
            if ( cell.Y != lastY )
            {
               lastY = cell.Y;
               mapRepresentation.Append( Environment.NewLine );
            }
            mapRepresentation.Append( cell.ToString() );
         }
         return mapRepresentation.ToString().TrimEnd( '\r', '\n' );
      }

      /// <summary>
      /// Get the Cell at the specified single dimensional array index using the formulas: x = index % Width; y = index / Width;
      /// </summary>
      /// <param name="index">The single dimensional array index for the Cell that we want to get</param>
      /// <returns>Cell at the specified single dimensional array index</returns>
      public TCell CellFor( int index )
      {
         int x = index % Width;
         int y = index / Width;

         return GetCell( x, y );
      }

      /// <summary>
      /// Get the single dimensional array index for a Cell at the specified location using the formula: index = ( y * Width ) + x
      /// </summary>
      /// <param name="x">X location of the Cell index to get starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell index to get, starting with 0 as the top</param>
      /// <returns>An index for the Cell at the specified location useful if storing Cells in a single dimensional array</returns>
      public int IndexFor( int x, int y )
      {
         return ( y * Width ) + x;
      }

      /// <summary>
      /// Get the single dimensional array index for the specified Cell
      /// </summary>
      /// <param name="cell">The Cell to get the index for</param>
      /// <returns>An index for the Cell which is useful if storing Cells in a single dimensional array</returns>
      /// <exception cref="ArgumentNullException">Thrown on null cell</exception>
      public int IndexFor( TCell cell )
      {
         if ( cell == null )
         {
            throw new ArgumentNullException( nameof( cell ), "Cell cannot be null" );
         }

         return ( cell.Y * Width ) + cell.X;
      }

      private bool AddToHashSet( HashSet<int> hashSet, int x, int y, out TCell cell )
      {
         cell = GetCell( x, y );
         return hashSet.Add( IndexFor( cell ) );
      }

      private bool AddToHashSet( HashSet<int> hashSet, int x, int y, TCell centerCell, out TCell cell )
      {
         cell = GetCell( x, y );
         if ( cell.Equals( centerCell ) )
         {
            return false;
         }

         return hashSet.Add( IndexFor( cell ) );
      }

      private bool AddToHashSet( HashSet<int> hashSet, TCell cell )
      {
         return hashSet.Add( IndexFor( cell ) );
      }
   }
}
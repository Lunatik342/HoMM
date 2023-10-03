using System;
using System.Collections.Generic;

namespace RogueSharp
{
   /// <summary>
   /// A Map represents a rectangular grid of Cells, each of which has a number of properties for determining walkability and so on
   /// The upper left corner of the Map is Cell (0,0) and the X value increases to the right, as the Y value increases downward
   /// </summary>
   public interface IMap : IMap<Cell>
   {
   }

   /// <summary>
   /// A Map represents a rectangular grid of Cells, each of which has a number of properties for determining walkability and so on
   /// The upper left corner of the Map is Cell (0,0) and the X value increases to the right, as the Y value increases downward
   /// </summary>
   public interface IMap<TCell> where TCell : ICell
   {
      /// <summary>
      /// This Indexer allows direct access to Cells given x and y index
      /// </summary>
      /// <param name="x">X index of the Cell to get</param>
      /// <param name="y">Y index of the Cell to get</param>
      /// <returns>Cell at the specified index</returns>
      TCell this[int x, int y] { get; set; }

      /// <summary>
      /// How many Cells wide the Map is
      /// </summary>
      /// <remarks>
      /// A Map with a Width of 10 will have Cells with X locations of 0 through 9
      /// Cells with an X value of 0 will be the leftmost Cells
      /// </remarks>
      int Width
      {
         get;
      }

      /// <summary>
      /// How many Cells tall the Map is
      /// </summary>
      /// <remarks>
      /// A Map with a Height of 20 will have Cells with Y locations of 0 through 19
      /// Cells with an Y value of 0 will be the topmost Cells
      /// </remarks>
      int Height
      {
         get;
      }

      /// <summary>
      /// Create a new map with the properties of all Cells set to false
      /// </summary>
      /// <remarks>
      /// This is basically a solid stone map that would then need to be modified to have interesting features
      /// </remarks>
      /// <param name="width">How many Cells wide the Map will be</param>
      /// <param name="height">How many Cells tall the Map will be</param>
      void Initialize( int width, int height );

      /// <summary>
      /// Get an IEnumerable of all Cells in the Map
      /// </summary>
      /// <returns>IEnumerable of all Cells in the Map</returns>
      IEnumerable<TCell> GetAllCells();

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
      IEnumerable<TCell> GetCellsAlongLine( int xOrigin, int yOrigin, int xDestination, int yDestination );

      /// <summary>
      /// Get an IEnumerable of Cells in a circle around the center Cell up to the specified radius using Bresenham's midpoint circle algorithm
      /// </summary>
      /// <seealso href="https://en.wikipedia.org/wiki/Midpoint_circle_algorithm">Based on Bresenham's midpoint circle algorithm</seealso>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="radius">The number of Cells to get in a radius from the center Cell</param>
      /// <returns>IEnumerable of Cells in a circle around the center Cell</returns>
      IEnumerable<TCell> GetCellsInCircle( int xCenter, int yCenter, int radius );

      /// <summary>
      /// Get an IEnumerable of Cells in a diamond (Rhombus) shape around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in a distance from the center Cell</param>
      /// <returns>IEnumerable of Cells in a diamond (Rhombus) shape around the center Cell</returns>
      IEnumerable<TCell> GetCellsInDiamond( int xCenter, int yCenter, int distance );

      /// <summary>
      /// Get an IEnumerable of Cells in a square area around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in each direction from the center Cell</param>
      /// <returns>IEnumerable of Cells in a square area around the center Cell</returns>
      IEnumerable<TCell> GetCellsInSquare( int xCenter, int yCenter, int distance );

      /// <summary>
      /// Get an IEnumerable of Cells in a rectangle area
      /// </summary>
      /// <param name="top">The top row of the rectangle </param>
      /// <param name="left">The left column of the rectangle</param>
      /// <param name="width">The width of the rectangle</param>
      /// <param name="height">The height of the rectangle</param>
      /// <returns>IEnumerable of Cells in a rectangle area</returns>
      IEnumerable<TCell> GetCellsInRectangle( int top, int left, int width, int height );

      /// <summary>
      /// Get an IEnumerable of outermost border Cells in a circle around the center Cell up to the specified radius using Bresenham's midpoint circle algorithm
      /// </summary>
      /// <seealso href="https://en.wikipedia.org/wiki/Midpoint_circle_algorithm">Based on Bresenham's midpoint circle algorithm</seealso>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="radius">The number of Cells to get in a radius from the center Cell</param>
      /// <returns>IEnumerable of outermost border Cells in a circle around the center Cell</returns>
      IEnumerable<TCell> GetBorderCellsInCircle( int xCenter, int yCenter, int radius );

      /// <summary>
      /// Get an IEnumerable of outermost border Cells in a diamond (Rhombus) shape around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in a distance from the center Cell</param>
      /// <returns>IEnumerable of outermost border Cells in a diamond (Rhombus) shape around the center Cell</returns>
      IEnumerable<TCell> GetBorderCellsInDiamond( int xCenter, int yCenter, int distance );

      /// <summary>
      /// Get an IEnumerable of outermost border Cells in a square area around the center Cell up to the specified distance
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="distance">The number of Cells to get in each direction from the center Cell</param>
      /// <returns>IEnumerable of outermost border Cells in a square area around the center Cell</returns>
      IEnumerable<TCell> GetBorderCellsInSquare( int xCenter, int yCenter, int distance );

      /// <summary>
      /// Get an IEnumerable of all the Cells in the specified row numbers
      /// </summary>
      /// <param name="rowNumbers">Integer array of row numbers with 0 as the top</param>
      /// <returns>IEnumerable of all the Cells in the specified row numbers</returns>
      IEnumerable<TCell> GetCellsInRows( params int[] rowNumbers );

      /// <summary>
      /// Get an IEnumerable of all the Cells in the specified column numbers
      /// </summary>
      /// <param name="columnNumbers">Integer array of column numbers with 0 as the farthest left</param>
      /// <returns>IEnumerable of all the Cells in the specified column numbers</returns>
      IEnumerable<TCell> GetCellsInColumns( params int[] columnNumbers );

      /// <summary>
      /// Get an IEnumerable of adjacent Cells which touch the center Cell. Diagonal cells do not count as adjacent.
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <returns>IEnumerable of adjacent Cells which touch the center Cell</returns>
      IEnumerable<TCell> GetAdjacentCells( int xCenter, int yCenter );
      
      /// <summary>
      /// Get an IEnumerable of adjacent Cells which touch the center Cell. Diagonal cells may optionally be included.
      /// </summary>
      /// <param name="xCenter">X location of the center Cell with 0 as the farthest left</param>
      /// <param name="yCenter">Y location of the center Cell with 0 as the top</param>
      /// <param name="includeDiagonals">Should diagonal Cells count as being adjacent cells?</param>
      /// <returns>IEnumerable of adjacent Cells which touch the center Cell</returns>
      IEnumerable<TCell> GetAdjacentCells( int xCenter, int yCenter, bool includeDiagonals );

      /// <summary>
      /// Get a Cell at the specified location
      /// </summary>
      /// <param name="x">X location of the Cell to get starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell to get, starting with 0 as the top</param>
      /// <returns>Cell at the specified location</returns>
      TCell GetCell( int x, int y );

      /// <summary>
      /// Provides a simple visual representation of the map using the following symbols:
      /// - `s`: `Cell` is walkable
      /// - `#`: `Cell` is not walkable
      /// </summary>
      /// <returns>A string representation of the map using special symbols to denote Cell properties</returns>
      string ToString();

      /// <summary>
      /// Get the Cell at the specified single dimensional array index using the formulas: x = index % Width; y = index / Width;
      /// </summary>
      /// <param name="index">The single dimensional array index for the Cell that we want to get</param>
      /// <returns>Cell at the specified single dimensional array index</returns>
      TCell CellFor( int index );

      /// <summary>
      /// Get the single dimensional array index for a Cell at the specified location using the formula: index = ( y * Width ) + x
      /// </summary>
      /// <param name="x">X location of the Cell index to get starting with 0 as the farthest left</param>
      /// <param name="y">Y location of the Cell index to get, starting with 0 as the top</param>
      /// <returns>An index for the Cell at the specified location useful if storing Cells in a single dimensional array</returns>
      int IndexFor( int x, int y );

      /// <summary>
      /// Get the single dimensional array index for the specified Cell
      /// </summary>
      /// <param name="cell">The Cell to get the index for</param>
      /// <returns>An index for the Cell which is useful if storing Cells in a single dimensional array</returns>
      int IndexFor( TCell cell );
   }

   /// <summary>
   /// A class representing the state of a Map which can be used to Restore a Map to a previously Saved state
   /// This POCO (Plain Old C# Object) can be easily serialized and deserialized
   /// </summary>
   public class MapState
   {
      /// <summary>
      /// Flags Enumeration of the possible properties for any Cell in the Map
      /// </summary>
      [Flags]
      public enum CellProperties
      {
         /// <summary>
         /// Not set
         /// </summary>
         None = 0,
         /// <summary>
         /// A character could normally walk across the Cell without difficulty
         /// </summary>
         Walkable = 1
      }

      /// <summary>
      /// How many Cells wide the Map is
      /// </summary>
      public int Width
      {
         get; set;
      }

      /// <summary>
      /// How many Cells tall the Map is
      /// </summary>
      public int Height
      {
         get; set;
      }

      /// <summary>
      /// An array of the Flags Enumeration of CellProperties for each Cell in the Map.
      /// The index of the array corresponds to the location of the Cell within the Map using the formula: index = ( y * Width ) + x
      /// </summary>
      public CellProperties[] Cells
      {
         get; set;
      }
   }
}
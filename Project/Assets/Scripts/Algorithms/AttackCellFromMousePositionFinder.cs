using Algorithms.RogueSharp;
using Battle.Arena.Misc;
using UnityEngine;
using Utilities;

namespace Algorithms
{
    public class AttackCellFromMousePositionFinder
    {
        public static Vector2Int FindPosition(Cell cellToAttack, Vector3 mouseWorldPosition)
        {
            //Size that guarantees so they would always intersect (1.41 - diagonal cost - should be enough, but taking more just in case)
            int cellToMouseLineSize = 5;
            
            var cellWorldPosition = cellToAttack.ToBattleArenaWorldPosition();
            var directionFromMouse = mouseWorldPosition - cellWorldPosition;

            var p1 = cellWorldPosition;
            var p2 = cellWorldPosition + directionFromMouse.normalized * cellToMouseLineSize;

            var cellCenterToMouseLine = new Line(p1.ToVector2XZ(), p2.ToVector2XZ());
            Vector2 result;

            var line = GetLine(cellToAttack, -1, +1, +1, +1);
            if (LinesIntersectionFinder.TryGetIntersectionPoint(cellCenterToMouseLine, line, out result))
            {
                return ConvertToCellCoordinates(result);
            }

            line = GetLine(cellToAttack, -1, -1, +1, -1);
            if (LinesIntersectionFinder.TryGetIntersectionPoint(cellCenterToMouseLine, line, out result))
            {
                return ConvertToCellCoordinates(result);
            }
            
            line = GetLine(cellToAttack, -1, -1, -1, +1);
            if (LinesIntersectionFinder.TryGetIntersectionPoint(cellCenterToMouseLine, line, out result))
            {
                return ConvertToCellCoordinates(result);
            }
            
            line = GetLine(cellToAttack, +1, -1, +1, +1);
            if (LinesIntersectionFinder.TryGetIntersectionPoint(cellCenterToMouseLine, line, out result))
            {
                return ConvertToCellCoordinates(result);
            }

            Debug.LogError("Error in finding intersection");
            return ConvertToCellCoordinates(result);
        }

        private static Vector2Int ConvertToCellCoordinates(Vector2 worldPosition)
        {
            return worldPosition.ToVector3X0Y().ToMapCellCoordinates();
        }

        private static Line GetLine(Cell cell, int p1XOffset, int p1YOffset, int p2XOffset, int p2YOffset)
        {
            var p1 = new Vector2Int(cell.X + p1XOffset, cell.Y + p1YOffset).ToBattleArenaWorldPosition().ToVector2XZ();
            var p2 = new Vector2Int(cell.X + p2XOffset, cell.Y + p2YOffset).ToBattleArenaWorldPosition().ToVector2XZ();

            return new Line(p1, p2);
        }
    }

    public class Line
    {
        public Vector2 Point1 { get; }
        public Vector2 Point2 { get; }

        public Line(Vector2 point1, Vector2 point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
    }
}
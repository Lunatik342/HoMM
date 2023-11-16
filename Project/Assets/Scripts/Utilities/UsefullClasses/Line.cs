using UnityEngine;

namespace Utilities.UsefullClasses
{
    public struct Line
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
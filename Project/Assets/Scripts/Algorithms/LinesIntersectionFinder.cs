using UnityEngine;

namespace Algorithms
{
    public class LinesIntersectionFinder
    {
        //https://forum.unity.com/threads/line-intersection.17384/
        public static bool TryGetIntersectionPoint(Line l1, Line l2, out Vector2 intersection)
        {
            var p1 = l1.Point1;
            var p2 = l1.Point2;
            var p3 = l2.Point1;
            var p4 = l2.Point2;
            
            intersection = Vector2.zero;
            
            float Ax, Bx, Cx, Ay, By, Cy, d, e, f, num;
            float x1lo, x1hi, y1lo, y1hi;

            Ax = p2.x - p1.x;
            Bx = p3.x - p4.x;

            if (Ax < 0)
            {
                x1lo = p2.x;
                x1hi = p1.x;
            }
            else
            {
                x1hi = p2.x;
                x1lo = p1.x;
            }

            if (Bx > 0)
            {
                if (x1hi < p4.x || p3.x < x1lo)
                    return false;
            }
            else
            {
                if (x1hi < p3.x || p4.x < x1lo)
                    return false;
            }

            Ay = p2.y - p1.y;
            By = p3.y - p4.y;

            if (Ay < 0)
            {
                y1lo = p2.y;
                y1hi = p1.y;
            }
            else
            {
                y1hi = p2.y;
                y1lo = p1.y;
            }

            if (By > 0)
            {
                if (y1hi < p4.y || p3.y < y1lo)
                    return false;
            }
            else
            {
                if (y1hi < p3.y || p4.y < y1lo)
                    return false;
            }


            Cx = p1.x - p3.x;
            Cy = p1.y - p3.y;
            d = By * Cx - Bx * Cy;
            f = Ay * Bx - Ax * By;

            if (f > 0)
            {
                if (d < 0 || d > f)
                    return false;
            }
            else
            {
                if (d > 0 || d < f)
                    return false;
            }

            e = Ax * Cy - Ay * Cx;

            if (f > 0)
            {
                if (e < 0 || e > f)
                    return false;
            }
            else
            {
                if (e > 0 || e < f)
                    return false;
            }

            if (f == 0)
                return false;

            num = d * Ax;
            intersection.x = p1.x + num / f;
            num = d * Ay;
            intersection.y = p1.y + num / f;

            return true;
        }
    }
}
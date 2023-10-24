using UnityEngine;

namespace Utilities
{
    public static class VectorUtilities
    {
        public static Vector2 ToVector2XZ(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }
        
        public static Vector3 ToVector3X0Y(this Vector2 vector2)
        {
            return new Vector3(vector2.x,0, vector2.y);
        }
    }
}
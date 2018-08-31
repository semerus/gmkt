using UnityEngine;

namespace GiraffeStar
{
    public static class VectorEx
    {
        //==============================================================================================
        // Vector2
        //==============================================================================================
        public static Vector2 OverrideX(this Vector2 vec, float x)
        {
            return new Vector2(x, vec.y);
        }

        public static Vector2 OverrideY(this Vector2 vec, float y)
        {
            return new Vector2(vec.x, y);
        }
        //==============================================================================================
        // Vector3
        //==============================================================================================
        public static Vector3 OverrideX(this Vector3 vec, float x)
        {
            return new Vector3(x, vec.y, vec.z);
        }

        public static Vector3 OverrideY(this Vector3 vec, float y)
        {
            return new Vector3(vec.x, y, vec.z);
        }

        public static Vector3 OverrideZ(this Vector3 vec, float z)
        {
            return new Vector3(vec.x, vec.y, z);
        }
        //==============================================================================================
    }
}

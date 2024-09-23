using UnityEngine;

namespace Utils
{
    public static class VectorUtils
    {
        public static Vector3 WithZ(this Vector3 vec, float z)
        {
            return new Vector3(vec.x, vec.y, z);
        }

        public static Vector3 WithX(this Vector3 vec, float x)
        {
            return new Vector3(x, vec.y, vec.z);
        }

        public static Vector3 WithY(this Vector3 vec, float y)
        {
            return new Vector3(vec.x, y, vec.z);
        }

        public static Vector2 WithX(this Vector2 vec, float x)
        {
            return new Vector2(x, vec.y);
        }

        public static Vector2 WithY(this Vector2 vec, float y)
        {
            return new Vector2(vec.x, y);
        }
        
        public static Vector3 AsVector3(this Vector2 vector2, bool swapYZ = false, float AddedFieldValue = 1f)
        {
            return new Vector3(vector2.x, 
                swapYZ ? AddedFieldValue : vector2.y,
                swapYZ ? vector2.y : AddedFieldValue);
        }
        

        public static Vector2Int AsVector2Int(this Vector2 vector2)
        {
            return new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
        }
        
        public static Vector2Int AsVector2IntNormalized(this Vector2 vector2)
        {
            return new Vector2Int(Mathf.Clamp(Mathf.RoundToInt(vector2.x), -1, 1), Mathf.Clamp(Mathf.RoundToInt(vector2.y), -1, 1));
        }
    }
}
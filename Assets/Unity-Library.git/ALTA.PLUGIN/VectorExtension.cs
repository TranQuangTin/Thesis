using UnityEngine;
using System.Collections;
namespace Alta.Plugin
{
    public static class VectorExtension
    {
        public static float AngleBetween(this Vector2 p1, Vector2 p2)
        {
            Vector2 p1Rotated90 = new Vector2(-p1.y, p1.x);
            float sign = (Vector2.Dot(p1Rotated90, p2) < 0) ? -1.0f : 1.0f;
            return Vector2.Angle(p1, p2) * sign;
        }

        public static void setZ(this Vector3 vec, float z)
        {
            vec = new Vector3(vec.x, vec.y, z);
        }
        public static void setY(this Vector3 vec, float y)
        {
            vec = new Vector3(vec.x, y, vec.z);
        }
        public static void setX(this Vector2 v, float x)
        {
            v = new Vector2(x, v.y);
        }
        public static void setY(this Vector2 v, float y)
        {
            v = new Vector2(v.x, y);
        }
        public static Vector3 to3(this Vector2 vec, float z)
        {
            return new Vector3(vec.x, vec.y, z);
        }

        public static Alta.Plugin.Data.Vector3 toSerializable(this Vector3 v)
        {
            return new Data.Vector3(v);
        }
        public static Data.Vector2 toSerializable(this Vector2 v)
        {
            return new Data.Vector2(v);
        }

        public static Data.Vector4 toSerializable(this Color v)
        {
            return new Data.Vector4(v);
        }
        public static Data.Vector4 toSerializable(this Vector4 v)
        {
            return new Data.Vector4(v);
        }

        public static Vector3 to3(this Alta.Plugin.Data.Vector3 v)
        {
            if (v == null)
                return Vector3.zero;
            return new Vector3(v.x, v.y, v.z);
        }
        public static Vector2 to2(this Data.Vector2 v)
        {
            if (v == null)
                return Vector2.zero;
            return new Vector2(v.x, v.y);
        }

        public static Color toColor(this Data.Vector4 v)
        {
            if (v == null)
                return Color.black;
            return new Color(v.x, v.y, v.z, v.a);
        }

        public static Vector4 to4(this Data.Vector4 v)
        {
            if (v == null)
                return Vector4.zero;
            return new Vector4(v.x, v.y, v.z, v.a);
        }
    }
}

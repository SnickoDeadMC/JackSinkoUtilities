using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
    
    /// <summary>
    /// Multiplies each element in Vector3 v by the corresponding element of w.
    /// </summary>
    public static Vector3 Multiply(this Vector3 a, Vector3 b)
    {
        a.x *= b.x;
        a.y *= b.y;
        a.z *= b.z;

        return a;
    }

    public static Vector3 ClampValues(this Vector3 a, float min, float max)
    {
        float x = Mathf.Clamp(a.x, min, max);
        float y = Mathf.Clamp(a.y, min, max);
        float z = Mathf.Clamp(a.z, min, max);

        return new Vector3(x, y, z);
    }
    
    public static bool ApproxEquals(Vector2 a, Vector2 b) {
        return Vector2.SqrMagnitude(a - b) < 0.0001;
    }

}

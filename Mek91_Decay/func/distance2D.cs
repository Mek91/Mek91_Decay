using UnityEngine;

namespace Mek91_Decay.func
{
    public class distance2D
    {
        public static float calc(Vector2 pointA, Vector2 pointB)
        {
            return Vector2.Distance(pointA, pointB);
        }
    }
}
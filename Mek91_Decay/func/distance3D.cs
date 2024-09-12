using UnityEngine;

namespace Mek91_Decay.func
{
    public class distance3D
    {
        public static float calc(Vector3 pointA, Vector3 pointB)
        {
            return Vector3.Distance(pointA, pointB);
        }
    }
}
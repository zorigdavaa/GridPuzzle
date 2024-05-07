using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

namespace Name
{
    public class Utility : MonoBehaviour
    {
        public static void DebugCube(Vector3 center, Vector3 halfExtents, Quaternion rotation, Color color, float duration = 0.0f)
        {
            Vector3[] btm = new Vector3[4];
            Vector3[] top = new Vector3[4];

            btm[0] = center - halfExtents;
            btm[1] = btm[0] + Vector3.forward * halfExtents.z * 2f;
            btm[2] = btm[1] + Vector3.right * halfExtents.x * 2f;
            btm[3] = btm[2] - Vector3.forward * halfExtents.z * 2f;

            for (int i = 0; i < 4; i++)
            {
                top[i] = btm[i].ChangeY(center.y + halfExtents.y);
                btm[i] = RotatePointAroundPivot(btm[i], center, rotation);
                top[i] = RotatePointAroundPivot(top[i], center, rotation);

                Debug.DrawLine(btm[i], top[i], color, duration);
                if (i > 0)
                {
                    Debug.DrawLine(btm[i], btm[i - 1], color, duration);
                    Debug.DrawLine(top[i], top[i - 1], color, duration);
                }
            }

            Debug.DrawLine(top[3], top[0], color, duration);
            Debug.DrawLine(btm[3], btm[0], color, duration);
        }

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            Vector3 v = point - pivot;
            v = rotation * v;
            return pivot + v;
        }
        public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
        {
            return 0.5f *
                   ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i +
                    (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
        }
    }
}


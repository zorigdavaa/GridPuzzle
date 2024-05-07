using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ZPackage
{
    public static class Vector3Helper
    {
        ///<Summary>
        ///1 is front -1 is back
        ///</Summary>
        public static bool IsFront(this Transform transform, Transform other, float dotValue)
        {
            Vector3 toTarget = (other.position - transform.position).normalized;

            if (Vector3.Dot(toTarget, transform.forward) > dotValue)//1 is fron -1 is back
            {
                // Debug.Log("Target is in front of this game object.");
                return true;
            }
            else
            {
                // Debug.Log("Target is not in front of this game object.");
                return false;
            }
        }
        ///<Summary>
        ///1 is front -1 is back
        ///</Summary>
        public static bool IsFront(this Vector3 position, Vector3 other, float dotValue)
        {
            Vector3 toTarget = (other - position).normalized;

            if (Vector3.Dot(toTarget, position) > dotValue)//1 is fron -1 is back
            {
                // Debug.Log("Target is in front of this game object.");
                return true;
            }
            else
            {
                // Debug.Log("Target is not in front of this game object.");
                return false;
            }
        }
        ///<Summary> Coroutine <Summary/>
        public static IEnumerator LookAtSmoothly(Transform Source, Transform lookTarget, float duration)
        {
            float time = 0;
            Vector3 direction = lookTarget.position - Source.position;
            direction.y = 0; // 
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            while (time < duration)
            {
                time += Time.deltaTime;
                Source.rotation = Quaternion.Lerp(Source.rotation, targetRotation, time / duration);
                yield return null;
            }
        }
        ///<Summary>This method uses Task.Delay async use it carefully<Summary/>
        public async static void LookAtSmoothlyAsync(this Transform transform, Vector3 other, float duration)
        {
            float time = 0;
            Vector3 direction = other - transform.position;
            direction.y = 0; // 
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            while (time < duration)
            {
                if (transform.rotation == targetRotation)
                {
                    break;
                }
                time += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, time / duration);
                await Task.Yield();
            }
        }
        public static bool isGreaterThanOrEqual(this Vector3 local, Vector3 other)
        {
            bool xCond = local.x > other.x || Mathf.Approximately(local.x, other.x);
            bool yCond = local.y > other.y || Mathf.Approximately(local.y, other.y);
            bool zCond = local.z > other.z || Mathf.Approximately(local.z, other.z);

            if (xCond && yCond && zCond)
                return true;

            return false;
        }

        public static bool isGreaterThan(this Vector3 local, Vector3 other)
        {
            bool xCond = local.x > other.x;
            bool yCond = local.y > other.y;
            bool zCond = local.z > other.z;

            if (xCond && yCond && zCond)
                return true;

            return false;
        }
        public static bool isOneSideLesserThan(this Vector3 local, Vector3 other)
        {
            bool xCond = local.x < other.x;
            bool yCond = local.y < other.y;
            bool zCond = local.z < other.z;

            if (xCond || yCond || zCond)
                return true;

            return false;
        }
        public static Vector3 ChangeX(this Vector3 vector, float x)
        {
            Vector3 tmp = vector;
            tmp.x = x;
            return tmp;
        }

        public static Vector3 ChangeY(this Vector3 vector, float y)
        {
            Vector3 tmp = vector;
            tmp.y = y;
            return tmp;
        }

        public static Vector3 ChangeZ(this Vector3 vector, float z)
        {
            Vector3 tmp = vector;
            tmp.z = z;
            return tmp;
        }
        public static Vector3 Round(this Vector3 v)
        {
            v.x = Mathf.Round(v.x);
            v.y = Mathf.Round(v.y);
            v.z = Mathf.Round(v.z);
            return v;
        }
        public static Vector3 Round(this Vector3 v, float size)
        {
            return (v / size).Round() * size;
        }
        public static Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 p0 = Vector3.Lerp(a, b, t);
            Vector3 p1 = Vector3.Lerp(b, c, t);
            return Vector3.Lerp(p0, p1, t);
        }
        public static Vector3 CubicCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            Vector3 p0 = QuadraticCurve(a, b, c, t);
            Vector3 p1 = QuadraticCurve(b, c, d, t);
            return Vector3.Lerp(p0, p1, t);
        }
        public static Vector3 RoundToNearest90Degree(Vector3 eulerAngles)
        {
            for (int i = 0; i < 3; i++)
            {
                eulerAngles[i] = Mathf.Round(eulerAngles[i] / 90f) * 90f;
            }
            return eulerAngles;
        }
        public static Vector3 RoundToNearestDegree(Vector3 eulerAngles, float degree)
        {
            for (int i = 0; i < 3; i++)
            {
                eulerAngles[i] = Mathf.Round(eulerAngles[i] / degree) * degree;
            }
            return eulerAngles;
        }

        public static Vector3 SetSelectedAxis(Vector3 original, Vector3 desired, Vector3 selectedAxis)
        {
            for (int i = 0; i < 3; i++)
            {
                if (selectedAxis[i] != 0)
                {
                    selectedAxis[i] = desired[i];
                }
                else
                {
                    selectedAxis[i] = original[i];
                }
            }

            return selectedAxis;
        }
    }
}


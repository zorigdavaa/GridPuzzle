using UnityEngine;

namespace ZPackage.Helper
{
    public static class PhysicsHelper
    {
        ///<Summary> targetluu shideh velocity <Summary/>
        public static Vector3 CalcBallisticVelocityVector(Vector3 source, Vector3 target, float angle)
        {
            Vector3 direction = target - source;
            float h = direction.y;
            direction.y = 0;
            float distance = direction.magnitude;
            float a = angle * Mathf.Deg2Rad;
            direction.y = distance * Mathf.Tan(a);
            distance += h / Mathf.Tan(a);
            Vector3 directionNorm = direction.normalized;
            // calculate velocity
            float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            return velocity * directionNorm;
            // return velocity * direction.normalized;
        }
        ///<Summary> targetluu shidehed hureh velocity gehdee targetluu bish uragshaa shidne <Summary/>
        public static Vector3 CalcBallisticVelocityVector(Vector3 source, Vector3 target, float angle, Vector3 forward)
        {
            Vector3 direction = target - source;
            float h = direction.y;
            direction.y = 0;
            float distance = direction.magnitude;
            float a = angle * Mathf.Deg2Rad;
            direction.y = distance * Mathf.Tan(a);
            distance += h / Mathf.Tan(a);
            Vector3 directionNorm = direction.normalized;
            directionNorm.x = forward.x;  // Ene zowhon uragshaa shiduulne
                                          // calculate velocity
            float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            return velocity * directionNorm;
            // return velocity * direction.normalized;
        }
        public static Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
        {
            //https://www.youtube.com/watch?v=03GHtGyEHas
            //define the distance x and y first
            Vector3 distance = target - origin;
            Vector3 distanceXZ = distance;
            distanceXZ.y = 0;

            //create a float the represent our distance
            float Sy = distance.y;
            float Sxz = distanceXZ.magnitude;
            float Vxz = Sxz / time;
            float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;
            Vector3 result = distanceXZ.normalized;
            result *= Vxz;
            result.y = Vy;
            return result;
        }
        public static VelocityData CalculateVelocityH(Vector3 current, Vector3 target, float h = 5)
        {
            //https://www.youtube.com/watch?v=IvT8hjy6q4o&list=RDCMUCmtyQOKKmrMVaKuRXz02jbQ&index=1
            float disPlacementY = target.y - current.y;
            float H = disPlacementY + h;
            Vector3 displacementXZ = new Vector3(target.x - current.x, 0, target.z - current.z);

            float time = (Mathf.Sqrt(-2 * H / Physics.gravity.y) + Mathf.Sqrt(2 * (disPlacementY - H) / Physics.gravity.y));

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * H);
            Vector3 velocityXZ = displacementXZ / time;
            return new VelocityData(velocityXZ + velocityY, time); ;
        }
        public static Vector3 CalculatePositionWithVelocity(Vector3 currentPosition, Vector3 velocity, float time)
        {
            // s= ut + att/2
            Vector3 atPosition = currentPosition + velocity * time + (Vector3.up * Physics.gravity.y * time * time) * 0.5f;
            return atPosition;
        }
        public static void DrawPath(VelocityData launchData, Vector3 from)
        {
            Vector3 previousDrawPoint = from; // ball.position;
            int resolution = 30;
            for (int i = 0; i < resolution; i++)
            {
                float simulationTime = i / (float)resolution * launchData.time;
                Vector3 displacement = launchData.velocity * simulationTime + Vector3.up * Physics.gravity.y * simulationTime * simulationTime / 2;
                Vector3 drawPoint = from + displacement;
                Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
                previousDrawPoint = drawPoint;
            }
        }
        // Use this onCollisionEnter
        public static Vector3 ComputeIncidentVelocity(Rigidbody body, Collision collision, out Vector3 otherVelocity)
        {
            Vector3 impulse = collision.impulse;
            // Both participants of a collision see the same impulse, so we need to flip it for one of them.
            if (Vector3.Dot(collision.GetContact(0).normal, impulse) < 0f)
                impulse *= -1f;
            otherVelocity = Vector3.zero;
            // Static or kinematic colliders won't be affected by impulses.
            var otherBody = collision.rigidbody;
            if (otherBody != null)
            {
                otherVelocity = otherBody.velocity;
                if (!otherBody.isKinematic)
                    otherVelocity += impulse / otherBody.mass;
            }
            return body.velocity - impulse / body.mass;
        }
        public struct VelocityData
        {
            public Vector3 velocity;
            public float time;
            public VelocityData(Vector3 velocity, float time)
            {
                this.time = time;
                this.velocity = velocity;
            }
        }

        public static void ApplyForceToReachVelocity(Rigidbody rigidbody, Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
        {
            if (force == 0 || velocity.magnitude == 0)
                return;

            velocity = velocity + velocity.normalized * 0.2f * rigidbody.drag;

            //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
            force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);

            //dot product is a projection from rhs to lhs with a length of result / lhs.magnitude https://www.youtube.com/watch?v=h0NJK4mEIJU
            if (rigidbody.velocity.magnitude == 0)
            {
                rigidbody.AddForce(velocity * force, mode);
            }
            else
            {
                var velocityProjectedToTarget = (velocity.normalized * Vector3.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
                rigidbody.AddForce((velocity - velocityProjectedToTarget) * force, mode);
            }
        }

        public static void ApplyTorqueToReachRPS(Rigidbody rigidbody, Quaternion rotation, float rps, float force = 1)
        {
            var radPerSecond = rps * 2 * Mathf.PI + rigidbody.angularDrag * 20;

            float angleInDegrees;
            Vector3 rotationAxis;
            rotation.ToAngleAxis(out angleInDegrees, out rotationAxis);

            if (force == 0 || rotationAxis == Vector3.zero)
                return;

            rigidbody.maxAngularVelocity = Mathf.Max(rigidbody.maxAngularVelocity, radPerSecond);

            force = Mathf.Clamp(force, -rigidbody.mass * 2 * Mathf.PI / Time.fixedDeltaTime, rigidbody.mass * 2 * Mathf.PI / Time.fixedDeltaTime);

            var currentSpeed = Vector3.Project(rigidbody.angularVelocity, rotationAxis).magnitude;

            rigidbody.AddTorque(rotationAxis * (radPerSecond - currentSpeed) * force);
        }


        public static Vector3 QuaternionToAngularVelocity(Quaternion rotation)
        {
            float angleInDegrees;
            Vector3 rotationAxis;
            rotation.ToAngleAxis(out angleInDegrees, out rotationAxis);

            return rotationAxis * angleInDegrees * Mathf.Deg2Rad;
        }

        public static Quaternion AngularVelocityToQuaternion(Vector3 angularVelocity)
        {
            var rotationAxis = (angularVelocity * Mathf.Rad2Deg).normalized;
            float angleInDegrees = (angularVelocity * Mathf.Rad2Deg).magnitude;

            return Quaternion.AngleAxis(angleInDegrees, rotationAxis);
        }

        public static Vector3 GetNormal(Vector3[] points)
        {
            //https://www.ilikebigbits.com/2015_03_04_plane_from_points.html
            if (points.Length < 3)
                return Vector3.up;

            var center = GetCenter(points);

            float xx = 0f, xy = 0f, xz = 0f, yy = 0f, yz = 0f, zz = 0f;

            for (int i = 0; i < points.Length; i++)
            {
                var r = points[i] - center;
                xx += r.x * r.x;
                xy += r.x * r.y;
                xz += r.x * r.z;
                yy += r.y * r.y;
                yz += r.y * r.z;
                zz += r.z * r.z;
            }

            var det_x = yy * zz - yz * yz;
            var det_y = xx * zz - xz * xz;
            var det_z = xx * yy - xy * xy;

            if (det_x > det_y && det_x > det_z)
                return new Vector3(det_x, xz * yz - xy * zz, xy * yz - xz * yy).normalized;
            if (det_y > det_z)
                return new Vector3(xz * yz - xy * zz, det_y, xy * xz - yz * xx).normalized;
            else
                return new Vector3(xy * yz - xz * yy, xy * xz - yz * xx, det_z).normalized;

        }

        public static Vector3 GetCenter(Vector3[] points)
        {
            var center = Vector3.zero;
            for (int i = 0; i < points.Length; i++)
                center += points[i] / points.Length;
            return center;
        }
    }
}

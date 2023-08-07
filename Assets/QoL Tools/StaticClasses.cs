using UnityEngine;

namespace MonoWaves.QoL
{
    public static class ZMath
    {
        public static readonly Vector3 XZ = new(1, 0, 1);
        public static readonly Vector3 XY = new(1, 1, 0);
        public static readonly Vector3 YZ = new(0, 1, 1);

        public static readonly Vector3 mXZ = new(-1, 0, -1);
        public static readonly Vector3 mXY = new(-1, -1, 0);
        public static readonly Vector3 mYZ = new(0, -1, -1);

        public static readonly Vector3 mXYmZ = new(-1, 1, -1);

        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }

        public static float AngleToMouse(Vector2 position)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        public static Vector3 Round(this Vector3 target, float roundTo = 1f)
        {
            return new Vector3
            (
                Mathf.Round(target.x / roundTo) * roundTo,
                Mathf.Round(target.y / roundTo) * roundTo,
                Mathf.Round(target.z / roundTo) * roundTo
            );
        }

        public static Vector3 Clamp(this Vector3 target, Vector3 min, Vector3 max)
        {
            return new Vector3
            (
                Mathf.Clamp(target.x, min.x, max.x),
                Mathf.Clamp(target.y, min.y, max.y),
                Mathf.Clamp(target.z, min.z, max.z)
            );
        }

        public static Vector3 Abs(this Vector3 target)
        {
            return new Vector3
            (
                Mathf.Abs(target.x),
                Mathf.Abs(target.y),
                Mathf.Abs(target.z)
            );
        }

        public static Vector3 Multiply(this Vector3 target, Vector3 multiplier)
        {
            return new Vector3
            (
                target.x * multiplier.x,
                target.y * multiplier.y,
                target.z * multiplier.z
            );
        }

        public static Vector3 Multiply(this Vector3 target, float x, float y, float z)
        {
            return new Vector3
            (
                target.x * x,
                target.y * y,
                target.z * z
            );
        }

        public static Vector3 Divide(this Vector3 target, Vector3 divider)
        {
            return new Vector3
            (
                target.x / divider.x,
                target.y / divider.y,
                target.z / divider.z
            );
        }

        public static Vector3 Divide(this Vector3 target, float x, float y, float z)
        {
            return new Vector3
            (
                target.x / x,
                target.y / y,
                target.z / z
            );
        }

        public static float KineticEnergy(this Rigidbody rb)
        {
            return rb.velocity.sqrMagnitude;
        }

        public static float Map(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }

    public static class Const
    {
        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";

        public const string PLAYER = "Player";
        public const string ENEMY = "Enemy";
        public const string MAP = "Map";
    }

    public static class Keyboard
    {
        public static bool IsPressed(params KeyCode[] keys)
        {
            foreach (var key in keys)
            {
                if (Input.GetKeyDown(key)) return true;
            }

            return false;
        }

        public static bool IsReleased(params KeyCode[] keys)
        {
            foreach (var key in keys)
            {
                if (Input.GetKeyUp(key)) return true;
            }

            return false;
        }

        public static bool IsHolding(params KeyCode[] keys)
        {
            foreach (var key in keys)
            {
                if (Input.GetKey(key)) return true;
            }

            return false;
        }
    }
}

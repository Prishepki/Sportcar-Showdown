using UnityEngine;

namespace MonoWaves.QoL
{
    public static class Vector
    {
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

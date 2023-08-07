using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gear : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [SerializeField] private float _mass = 1000000;

    private Rigidbody _rb;

    private void OnValidate()
    {
        if (TryGetComponent(out _rb))
        {
            _rb.useGravity = false;
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.mass = _mass;
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0.1f * _velocity, 0f, 0f), Space.World);
    }
}

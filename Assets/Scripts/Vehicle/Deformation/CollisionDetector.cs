using System.Collections.Generic;
using MonoWaves.QoL;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private GameObject _debugObject;
    [SerializeField] private Rigidbody _rb;

    private readonly List<Vector3> _collisionPoints = new();

    private float _mps;

    public UnityEvent<Vector3, Vector3, float> OnCollisionDetected { get; set; } = new UnityEvent<Vector3, Vector3, float>();

    private void OnValidate()
    {
        TryGetComponent(out _rb);
    }

    private void LateUpdate()
    {
        _mps = _rb.velocity.magnitude;
    }

    private void OnCollisionStay(Collision other)
    {
        if (_debugObject != null)
        {
            if (_mps == 0 && other.relativeVelocity.magnitude == 0) return;

            foreach (var contact in other.contacts)
            {
                Vector3 contactPointRounded = contact.point.Round(0.5f);

                if (_collisionPoints.Contains(contactPointRounded)) break;
                _collisionPoints.Add(contactPointRounded);

                GameObject debugPoint = Instantiate(_debugObject, contact.point, Quaternion.identity);
                debugPoint.transform.SetParent(transform);

                OnCollisionDetected.Invoke(contact.point, contact.normal, other.impulse.magnitude * 0.02f + other.relativeVelocity.magnitude);
            }
        }
    }
}

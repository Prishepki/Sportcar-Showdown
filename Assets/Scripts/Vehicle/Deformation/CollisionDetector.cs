using System.Collections.Generic;
using MonoWaves.QoL;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private GameObject _debugObject;

    private readonly List<Vector3> _collisionPoints = new();

    public UnityEvent<Vector3, Vector3, Vector3> OnCollisionDetected { get; set; } = new UnityEvent<Vector3, Vector3, Vector3>();

    private void OnCollisionStay(Collision other)
    {
        if (other.relativeVelocity.magnitude == 0) return;

        foreach (var contact in other.contacts)
        {
            Vector3 contactPointRounded = contact.point.Round(0.5f);

            if (_collisionPoints.Contains(contactPointRounded)) break;
            _collisionPoints.Add(contactPointRounded);

            OnCollisionDetected.Invoke(contact.point, contact.normal, other.impulse * 0.02f + other.relativeVelocity);

            if (!_debugObject) return;

            GameObject debugPoint = Instantiate(_debugObject, contact.point, Quaternion.identity);
            debugPoint.transform.SetParent(transform);
        }
    }
}

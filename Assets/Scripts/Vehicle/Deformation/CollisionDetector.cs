using System.Collections.Generic;
using MonoWaves.QoL;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private GameObject _debugObject;

    [Header("Properties")]
    [SerializeField] private float _detectionQuality = 3;

    [Header("Debug")]
    [SerializeField] private bool _doDebug;

    private Vector3 _lastRecordedSpeed;
    private Rigidbody _rb;

    private readonly List<Vector3> _collisionPoints = new();

    public UnityEvent<Vector3, Vector3> OnCollisionDetected { get; set; } = new UnityEvent<Vector3, Vector3>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        _lastRecordedSpeed = _rb.velocity.Multiply(ZMath.XZ).ClampMinimum(Vector3.one);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.relativeVelocity.Equals(0, 0, 0)) return;

        foreach (var contact in other.contacts)
        {
            Vector3 contactPointRounded = contact.point.Round(1 / _detectionQuality);

            if (_collisionPoints.Contains(contactPointRounded)) break;
            _collisionPoints.Add(contactPointRounded);

            OnCollisionDetected.Invoke(contact.point, (_rb.KineticEnergy() + other.relativeVelocity.magnitude) * _lastRecordedSpeed.Multiply(contact.normal));

            if (!_doDebug) return;

            GameObject debugPoint = Instantiate(_debugObject, contact.point, Quaternion.identity);
            debugPoint.transform.SetParent(transform);
        }
    }
}

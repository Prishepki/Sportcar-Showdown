using System.Collections.Generic;
using MonoWaves.QoL;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private GameObject _debugObject;

    [Header("Properties")]
    [SerializeField] private float _detectionQuality = 2.5f;
    [SerializeField] private float _impactTreshold = 5f;

    [Header("Debug")]
    [SerializeField] private bool _doDebug;

    private Rigidbody _rb;
    private readonly List<Vector3> _collisionPoints = new();

    public UnityEvent<Vector3, Vector3> OnCollisionDetected { get; set; } = new UnityEvent<Vector3, Vector3>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        float collisionEnergy = other.rigidbody ? _rb.KineticEnergy() + other.rigidbody.KineticEnergy() : _rb.KineticEnergy();

        if (other.impulse.magnitude + collisionEnergy < _impactTreshold) return;

        foreach (var contact in other.contacts)
        {
            Vector3 contactPointRounded = contact.point.Round(1 / _detectionQuality);

            if (!_collisionPoints.Contains(contactPointRounded))
            {
                _collisionPoints.Add(contactPointRounded);

                float impactPerContact = contact.impulse.magnitude + collisionEnergy;
                Vector3 impactVector = contact.normal * impactPerContact;

                if (_doDebug) DoDebug(contact.point, impactVector);
                OnCollisionDetected.Invoke(contact.point, impactVector);
            }
        }
    }

    private void DoDebug(Vector3 point, Vector3 impact)
    {
        Debug.Log($"Invoking OnCollisionDetected at {point} with {impact}");

        GameObject debugPoint = Instantiate(_debugObject, point, Quaternion.identity);
        debugPoint.transform.SetParent(transform);
    }
}

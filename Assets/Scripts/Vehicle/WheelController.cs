using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Suspension")]
    public float restLength = 0.5f;
    public float springTravel = 0.2f;
    public float springStiffness = 30000f;
    public float damperStiffness = 4000f;

    private float _minLength;
    private float _maxLength;
    private float _lastLength;
    private float _springLength;
    private float _springVelocity;
    private float _springForce;
    private float _damperForce;

    private Vector3 _suspensionForce;

    [Header("Wheel")]
    public float wheelRadius = 0.33f;

    private void Start()
    {
        _rb = transform.root.GetComponent<Rigidbody>();

        _minLength = restLength - springTravel;
        _maxLength = restLength + springTravel;
    }

    private void FixedUpdate()
    {
        Suspension();
    }

    private void Suspension()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _maxLength + wheelRadius))
        {
            _lastLength = _springLength;

            _springLength = Mathf.Clamp(hit.distance - wheelRadius, _minLength, _maxLength);

            _springVelocity = (_lastLength - _springLength) / Time.fixedDeltaTime;

            _springForce = springStiffness * (restLength - _springLength);
            _damperForce = damperStiffness * _springVelocity;

            _suspensionForce = (_springForce + _damperForce) * transform.up;

            _rb.AddForceAtPosition(_suspensionForce, hit.point);
        }
    }
}

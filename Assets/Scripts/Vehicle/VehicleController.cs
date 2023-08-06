using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float speed;

    private Rigidbody _rb;
    private WheelController[] _wheels;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.velocity += speed * Time.fixedDeltaTime * transform.forward;
    }
}

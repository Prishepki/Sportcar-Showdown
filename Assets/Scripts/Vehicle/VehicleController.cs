using System;
using System.Collections.Generic;
using MonoWaves.QoL;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public enum VehicleDriveTrain
{
    Front, Rear, All,
};

public class VehicleController : MonoBehaviour
{
    private Rigidbody _rb;
    private List<WheelController> _wheels = new List<WheelController>();

    private float _inputSteering;
    private float _inputThrottle;

    public GameObject WheelModel;
    public WheelSettings WheelSettings;
    public VehicleDriveTrain DriveTrain;

    public float MaxSteering = 35f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // мрак
        foreach (Transform child in transform)
        {
            if (child.name.Length != 2) continue;

            char wheelDriveTrain = child.name[0]; // Front, Rear
            char wheelSide = child.name[1]; // Left, Right

            if (wheelDriveTrain != 'F' && wheelDriveTrain != 'R') continue;
            if (wheelSide != 'L' && wheelSide != 'R') continue;

            WheelSettings settings = WheelSettings.Clone();

            if (DriveTrain == VehicleDriveTrain.All)
                settings.isDriving = true;
            else if (DriveTrain == VehicleDriveTrain.Front && wheelDriveTrain == 'F')
                settings.isDriving = true;
            else if (DriveTrain == VehicleDriveTrain.Rear && wheelDriveTrain == 'R')
                settings.isDriving = true;

            settings.isSteering = wheelDriveTrain == 'F';
            settings.isLeft = wheelSide == 'L';

            WheelController wheel = child.gameObject.AddComponent<WheelController>();
            wheel.Settings = settings;
            wheel.Setup(_rb, WheelModel);

            _wheels.Add(wheel);
        }
    }

    private void Update()
    {
        _inputSteering = Input.GetAxisRaw(Const.HORIZONTAL);
        _inputThrottle = Input.GetAxisRaw(Const.VERTICAL);

        if (Keyboard.IsPressed(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FixedUpdate()
    {
        foreach (var wheel in _wheels)
        {
            float torque = 0;

            if (wheel.Settings.isSteering)
            {
                wheel.transform.localEulerAngles = new(0, _inputSteering * MaxSteering, 0);
            }

            if (wheel.Settings.isDriving)
            {
                torque = _inputThrottle * 600;
            }

            wheel.Step(torque);
        }
    }
}

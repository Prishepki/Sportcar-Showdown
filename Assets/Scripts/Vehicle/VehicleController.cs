using UnityEngine;

public class VehicleController : MonoBehaviour
{
    private Rigidbody _rb;
    private readonly WheelController[] _wheels = new WheelController[4];

    private float _inputSteering;
    private float _inputThrottle;

    public GameObject WheelModel;
    public WheelSettings WheelSettings;

    public float MaxSteering = 35f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // мрак
        foreach (Transform child in transform)
        {
            int index = -1;
            bool isDriving = false;
            bool isSteering = false;

            void SetWheel(int idx, bool steering = false, bool driving = false)
            {
                index = idx;
                isSteering = steering;
                isDriving = driving;
            }

            switch (child.name)
            {
                case "FL":
                    SetWheel(0, steering: true);
                    break;
                case "FR":
                    SetWheel(1, steering: true);
                    break;
                case "RL":
                    SetWheel(2, driving: true);
                    break;
                case "RR":
                    SetWheel(3, driving: true);
                    break;
            }

            if (index == -1) continue;

            WheelController wheel = child.gameObject.AddComponent<WheelController>();
            wheel.Setup(_rb, WheelModel);

            wheel.Settings = WheelSettings.Clone();
            wheel.Settings.isDriving = isDriving;
            wheel.Settings.isSteering = isSteering;

            _wheels[index] = wheel;
        }
    }

    private void Update()
    {
        _inputSteering = Input.GetAxisRaw(MonoWaves.QoL.Const.HORIZONTAL);
        _inputThrottle = Input.GetAxisRaw(MonoWaves.QoL.Const.VERTICAL);
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
                torque = _inputThrottle * 400;
            }

            wheel.Step(torque);
        }
    }
}

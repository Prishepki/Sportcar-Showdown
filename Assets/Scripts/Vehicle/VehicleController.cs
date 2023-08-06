using MonoWaves.QoL;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            WheelSide wheelSide = WheelSide.Right;
            bool isSteering = false;
            bool isDriving = false;

            void SetWheel(int idx, WheelSide side, bool steering = false, bool driving = false)
            {
                index = idx;
                wheelSide = side;
                isSteering = steering;
                isDriving = driving;
            }

            switch (child.name)
            {
                case "FL":
                    SetWheel(0, WheelSide.Left, steering: true);
                    break;
                case "FR":
                    SetWheel(1, WheelSide.Right, steering: true);
                    break;
                case "RL":
                    SetWheel(2, WheelSide.Left, driving: true);
                    break;
                case "RR":
                    SetWheel(3, WheelSide.Right, driving: true);
                    break;
            }

            if (index == -1) continue;

            WheelController wheel = child.gameObject.AddComponent<WheelController>();
            wheel.Setup(_rb, WheelModel, wheelSide);

            wheel.Initialize(new WheelInitializer(WheelSettings, isSteering, isDriving));

            _wheels[index] = wheel;
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
                torque = _inputThrottle * 400;
            }

            wheel.Step(torque);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UfoController : MonoBehaviour
{
    //Manager
    private UfoManager _ufoManager;
    private FuelManager _fuelManager;
    private InputAction _accelAction;
    //reference
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    private Camera _mainCamera;
    //movement
    private Vector2 _moveInput;
    private float _speed;
    [SerializeField] private bool _isAccel;
    //Event
    private bool _isMagnetOn;
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _ufoManager = GetComponent<UfoManager>();
        _fuelManager = GetComponent<FuelManager>();
        _mainCamera = Camera.main;
    }
    void Start()
    {
        _isMagnetOn = true;
    }
    private void OnEnable()
    {
        _accelAction = GetComponent<PlayerInput>().actions["Accel"];

        _accelAction.started += ctx => AccelOn();
        _accelAction.canceled += ctx => AccelOff();

        _accelAction.Enable();
    }
    private void OnDisable()
    {
        _accelAction.started -= ctx => AccelOn();
        _accelAction.canceled -= ctx => AccelOff();

        _accelAction.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        ClampCharacterPosition();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (_moveInput.magnitude <= 0.01f)
        {
            return;
        }

        if (_isAccel)
        {
            _rigidbody2D.AddForce(_moveInput * _ufoManager._accelSpeed);
            _fuelManager.UseFuel(_fuelManager.FuelConsumption);
        }
        else
        {
            _rigidbody2D.AddForce(_moveInput * _ufoManager._speed);
        }
    }
    /// <summary>
    /// 캐릭터 위치 제한
    /// </summary>
    private void ClampCharacterPosition()
    {
        Vector3 position = transform.position;
        Vector3 viewPos = _mainCamera.WorldToViewportPoint(position);

        bool clampedX = false;
        bool clampedY = false;

        if (viewPos.x < 0.03f || viewPos.x > 0.97f)
            clampedX = true;

        if (viewPos.y < 0f || viewPos.y > 1f)
            clampedY = true;

        viewPos.x = Mathf.Clamp(viewPos.x, 0.03f, 0.97f);
        viewPos.y = Mathf.Clamp(viewPos.y, 0.05f, 0.95f);
        position = _mainCamera.ViewportToWorldPoint(viewPos);
        transform.position = position;

        if (clampedX)
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        }

        if (clampedY)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        }
    }
    //Player Input-----------------------------------------------------------
    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void AccelOn()
    {
        if (_fuelManager.CanAccel())
        {
            _isAccel = true;
        }
    }
    private void AccelOff()
    {
        _isAccel = false;
    }

    void OnSwitch(InputValue value)
    {
        if (_isMagnetOn)
        {
            if (_ufoManager._magnetState == MagnetState.N)
            {
                _ufoManager._magnetState = MagnetState.S;
            }
            else if (_ufoManager._magnetState == MagnetState.S)
            {
                _ufoManager._magnetState = MagnetState.N;
            }
        }
    }
}

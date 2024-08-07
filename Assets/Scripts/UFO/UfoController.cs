using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    private Speed_UI _swapImage;
    //movement
    private Vector2 _moveInput;
    private float _speed;
    private bool _isAccel;
    //Event
    private bool _isMagnetOn;
    public bool _isEMP;
    public bool _isDecoy; //전파교란

    [SerializeField] GameObject _DecoyEffect;
    //UI
    GameUIManager _UIManager; 


    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _ufoManager = GetComponent<UfoManager>();
        _fuelManager = GetComponent<FuelManager>();
        _UIManager = GameObject.Find("Canvas").GetComponent<GameUIManager>();
        _swapImage = GameObject.Find("Swap_Magnet").GetComponent<Speed_UI>();
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
        TiltCharacter();
        _DecoyEffect.SetActive(_isDecoy);
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
    private void TiltCharacter()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (moveX != 0)
        {
            float targetAngle = -moveX * 15;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector2.zero), Time.fixedDeltaTime * 5);
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
        _moveInput = _isDecoy ? -value.Get<Vector2>() : value.Get<Vector2>();
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
        if (_isMagnetOn && _swapImage.isSwap == false && !_isEMP)
        {
            _swapImage.isSwap = true;
            if (_ufoManager._magnetState == MagnetState.N)
            {
                _ufoManager._magnetState = MagnetState.S;
            }
            else if (_ufoManager._magnetState == MagnetState.S)
            {
                _ufoManager._magnetState = MagnetState.N;
            }
        }
        if (_isEMP)
        {
            _UIManager.showAlert("EMP로 인해 변경이 불가합니다");
        }
    }
}

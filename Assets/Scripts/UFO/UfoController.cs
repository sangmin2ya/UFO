using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UfoController : MonoBehaviour
{
    //Manager
    private UfoManager _ufoManager;
    private HPManager _hpManager;
    private ObstacleManager _obstacleManager;
    //reference
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    private Camera _mainCamera;
    private Speed_UI _swapImage;
    //movement
    private bool _movable = true;
    private Vector2 _moveInput;
    private float _speed;
    //Event
    private bool _isMagnetOn;
    public bool _isEMP;
    public bool _isDecoy; //전파교란
    public bool _isStorm;
    public bool _isOnComet;

    [SerializeField] GameObject _DecoyEffect;
    [SerializeField] GameObject StormEffect;
    [SerializeField] GameObject UI_EMP_Effect;
    //UI
    GameUIManager _UIManager;
    private Collider2D _collider;
    public GameObject _magnetFieldMinigame;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _ufoManager = GetComponent<UfoManager>();
        _hpManager = GetComponent<HPManager>();
        _obstacleManager = GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>();

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
        _ufoManager.EnterMagnetFieldEvent += EnterMagnetField;
    }
    // Update is called once per frame
    void Update()
    {
        if (_movable)
            ClampCharacterPosition();
    }
    private void FixedUpdate()
    {
        if (_movable&&!GameManager.Instance._isDead)
        {
            Move();
            TiltCharacter();
            _DecoyEffect.SetActive(_isDecoy);
            StormEffect.SetActive(_isStorm);
            UI_EMP_Effect.SetActive(_isEMP);
        }

    }
    private void Move()
    {
        if (_moveInput.magnitude <= 0.01f)
        {
            return;
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
    public void SetUnmovable()
    {
        _movable = false;
    }
    public void SetMovable()
    {
        _movable = true;
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
    private void EnterMagnetField(Collider2D collider)
    {
        _collider = collider;
        _movable = false;
        Instantiate(_magnetFieldMinigame, GameObject.Find("PlayerCanvas").transform);
    }
    /// <summary>
    /// keep destroying magnet field
    /// </summary>
    public void ExitMagnetField()
    {
        if (_collider != null)
            _collider.GetComponent<UnknownMagnetField>().StartDestroyMagnetField();
        _collider = null;
        _movable = true;
    }
    //Player Input-----------------------------------------------------------
    void OnMove(InputValue value)
    {
        _moveInput = _isDecoy ? -value.Get<Vector2>() : value.Get<Vector2>();
    }
    void OnSwitch(InputValue value)
    {
        if (_isMagnetOn && _swapImage.isSwap == false && !_isEMP && _movable && Time.timeScale != 0)

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
    }
    void OnBomb(InputValue value)
    {
        if (_ufoManager.CanUseBomb() && Time.timeScale != 0 && _movable && LoadingScene.instance.canBomb == true)
        {
            Debug.Log("bomb!!");
            _ufoManager.DestroyEveryTrash();
            //폭탄 사용
            if(SceneManager.GetActiveScene().name == "Space_X")
            {
                GameObject.Find("MiddleBoss").GetComponent<SpaceX>().Take_Damage(GameObject.Find("MiddleBoss").GetComponent<SpaceX>().BombDamage);
                GameObject.Find("MiddleBoss").GetComponent<SpaceX>().SpawnBombEffect();
                Camera.main.GetComponent<CameraShake>().startCameraShake(.02f, .2f);
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UfoManager : MonoBehaviour
{
    //Data
    public List<ItemInfo> _AttacedObjects;//수정필요
    [SerializeField] private float _setSpeed;
    [SerializeField] private int _maxStuckCount;
    public float _speed { get; private set; }
    public float _accelSpeed { get; private set; }
    public MagnetState _magnetState { get; set; }
    //UI
    private Transform _handle;
    //Image
    [SerializeField] private List<Sprite> _magnetSprites;
    private Image _speedBar;
    private Image _weightBar;
    private Image _magnetImg;
    void Start()
    {
        _AttacedObjects = new List<ItemInfo>();
        _handle = GameObject.Find("Handle").transform;
        _speedBar = GameObject.Find("Speed_Image").GetComponent<Image>();
        _magnetImg = GameObject.Find("Current_Magnet").GetComponent<Image>();
        _speed = _setSpeed;
        _accelSpeed = _setSpeed * 2f;
    }
    void Update()
    {
        UpdateSpeed();
    }
    /// <summary>
    /// 달린 물체의 수에 따라 속도 조절, 속도 0이되면 게임오버
    /// </summary>
    private void UpdateSpeed()
    {
        _speed = Mathf.Max(0, _setSpeed * ((_maxStuckCount - _AttacedObjects.Count) / (float)_maxStuckCount));
        _accelSpeed = _speed * 2f;
        UpdateUI();
        if (_speed <= 0)
        {
            DestroyUfo();
        }
    }
    private void AttachObject(GameObject obj)
    {
        obj.GetComponent<ItemInfo>().Freeze();
        obj.transform.SetParent(transform);
        _AttacedObjects.Add(obj.GetComponent<ItemInfo>());
    }
    private void DetachObject(GameObject obj)
    {
        // obj.GetComponent<ItemInfo>().DestroyItem();
        _AttacedObjects.Remove(obj.GetComponent<ItemInfo>());
    }
    private void DestroyUfo()
    {
        //Destroy(gameObject);
    }
    private void UpdateUI()
    {
        if (_weightBar != null)
            _weightBar.fillAmount = _speed / _setSpeed;
        if (_speedBar != null)
        {
            var speedAmount = GetComponent<Rigidbody2D>().velocity.magnitude * 2 / _setSpeed;
            _speedBar.fillAmount = speedAmount;
            Camera.main.GetComponent<CameraShake>().shakeMagnitude = .017f * speedAmount;
            for (int i = 0; i < _handle.childCount; i++) {
                _handle.GetChild(i).GetComponent<Image>().color = new Color(1, 1 - speedAmount, 1 - speedAmount, 0.3f);
            }
        }
        if (_magnetImg != null)
            _magnetImg.sprite = _magnetState == MagnetState.N ? _magnetSprites[0] : _magnetSprites[1];
        transform.Find("Circle").GetComponent<SpriteRenderer>().color = _magnetState == MagnetState.S ? new Color(0, 1, 1, 0.05f) : new Color(1, 0, 0, 0.05f);
    }
    /// <summary>
    /// 충돌시 쓰레기면 부착, 아이템이면 효과적용 후 삭제
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            if (collision.GetComponent<ItemInfo>().type == ItemType.Item)
            {
                //아이템 효과 적용하는 함수
                collision.GetComponent<ItemInfo>().DestroyItem();
            }
            else if (collision.GetComponent<ItemInfo>().type == ItemType.Obstacle)
            {
                AttachObject(collision.gameObject);
            }
        }
    }
    /// <summary>
    /// 자식 객체에 충돌시 부착 처리
    /// </summary>
    /// <param name="other"></param>
    private void OnChildTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ItemInfo>().type == ItemType.Obstacle)
        {
            AttachObject(collision.gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UfoManager : MonoBehaviour
{
    //Data
    public List<GameObject> _AttacedObjects { get; set; } //수정필요
    [SerializeField] private float _setSpeed;
    [SerializeField] private int _maxStuckCount;
    public float _speed { get; private set; }
    public float _accelSpeed { get; private set; }
    public MagnetState _magnetState { get; set; }
    //Image
    private Image _speedBar;
    private Image _lifeBar;
    void Start()
    {
        _AttacedObjects = new List<GameObject>();
        _speedBar = GameObject.Find("Speed_Image").GetComponent<Image>();
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
        _speed = Mathf.Max(0, _setSpeed * ((_maxStuckCount - _AttacedObjects.Count) / _maxStuckCount));
        _accelSpeed = _speed * 2f;
        UpdateSpeedUI();
        if (_speed <= 0)
        {
            DestroyUfo();
        }
    }
    private void AttachObject(GameObject obj)
    {
        // obj.GetComponent<ItemInfo>().Freeze();
        obj.transform.SetParent(transform);
        _AttacedObjects.Add(obj);
    }
    private void DetachObject(GameObject obj)
    {
        // obj.GetComponent<ItemInfo>().DestroyItem();
        _AttacedObjects.Remove(obj);
    }
    private void DestroyUfo()
    {
        //Destroy(gameObject);
    }
    private void UpdateSpeedUI()
    {
        if (_lifeBar != null)
            _lifeBar.fillAmount = _speed / _setSpeed;
        if (_speedBar != null)
            _speedBar.fillAmount = (GetComponent<Rigidbody2D>().velocity.magnitude * 2) / _setSpeed;
    }
    /// <summary>
    /// 충돌시 쓰레기면 부착, 아이템이면 효과적용 후 삭제
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            //     if (collision.GetComponent<ItemInfo>()._itemType == ItemType.Item)
            //     {
            //         //아이템 효과 적용하는 함수
            //         //collision.GetComponent<ItemInfo>().DestroyItem();
            //     }
            //     else if (collision.GetComponent<ItemInfo>()._itemType == ItemType.Obstacle)
            //     {
            //         AttachObject(collision.gameObject);
            //     }
            // }
        }
    }
}
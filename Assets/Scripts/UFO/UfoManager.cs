using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UfoManager : MonoBehaviour
{
    //Event
    public event Action<Collider2D> EnterMagnetFieldEvent;
    //Data
    [SerializeField] private int _bombCount;
    [SerializeField] private List<ItemInfo> _AttacedObjects;
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
    private List<Image> _bombImages = new List<Image>();
    void Start()
    {
        _AttacedObjects = new List<ItemInfo>();
        _handle = GameObject.Find("Handle").transform;
        _speedBar = GameObject.Find("Speed_Image").GetComponent<Image>();
        _magnetImg = GameObject.Find("Current_Magnet").GetComponent<Image>();
        _bombImages.Add(GameObject.Find("Mass").GetComponent<Image>()); //needs to change
        _bombImages.Add(GameObject.Find("MainSkill").GetComponent<Image>()); //needs to change
        _speed = _setSpeed;
        _accelSpeed = _setSpeed * 2f;
    }
    void Update()
    {
        UpdateSpeed();
        UpdateUI();
    }
    /// <summary>
    /// 달린 물체의 수에 따라 속도 조절, 속도 0이되면 게임오버
    /// </summary>
    private void UpdateSpeed()
    {
        _speed = Mathf.Max(0, _setSpeed * ((_maxStuckCount - _AttacedObjects.Count) / (float)_maxStuckCount));
        _accelSpeed = _speed * 2f;
        if (_speed <= 0)
        {
            DestroyUfo();
        }
    }
    private void AttachObject(GameObject obj)
    {
        GameObject.Find("Canvas").GetComponent<GameUIManager>().showDamaged();
        obj.GetComponent<ItemInfo>().Freeze();
        obj.transform.SetParent(transform);
        _AttacedObjects.Add(obj.GetComponent<ItemInfo>());
    }
    private void DetachObject(GameObject obj)
    {
        obj.GetComponent<ItemInfo>().DestroyItem();
        _AttacedObjects.Remove(obj.GetComponent<ItemInfo>());
    }
    private void DestroyUfo()
    {
        //Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }
    private void UpdateUI()
    {
        //무게
        if (_weightBar != null)
            _weightBar.fillAmount = _speed / _setSpeed;
        //속도
        if (_speedBar != null)
        {
            var speedAmount = GetComponent<Rigidbody2D>().velocity.magnitude * 2 / _setSpeed;
            _speedBar.fillAmount = speedAmount;
            //Camera.main.GetComponent<CameraShake>().shakeMagnitude = .017f * speedAmount;
            for (int i = 0; i < _handle.childCount; i++)
            {
                _handle.GetChild(i).GetComponent<Image>().color = new Color(1, 1 - speedAmount, 1 - speedAmount, 0.3f);
            }
        }
        //자기
        if (_magnetImg != null)
            _magnetImg.sprite = _magnetState == MagnetState.N ? _magnetSprites[0] : _magnetSprites[1];
        transform.Find("Circle").GetComponent<SpriteRenderer>().color = _magnetState == MagnetState.S ? new Color(0, 1, 1, 0.05f) : new Color(1, 0, 0, 0.05f);
        //폭탄
        for (int i = 0; i < _bombImages.Count; i++)
        {
            _bombImages[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < _bombCount; i++)
        {
            _bombImages[i].gameObject.SetActive(true);
        }
    }
    public bool CanUseBomb()
    {
        if (_bombCount > 0)
        {
            _bombCount--;
            return true;
        }
        return false;
    }
    public void ClearAll()
    {
        _AttacedObjects.Clear();
    }
    private void CleanSurface()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_AttacedObjects.Count > 0)
            {
                DetachObject(_AttacedObjects[_AttacedObjects.Count - 1].gameObject);
            }
        }
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
                switch (collision.GetComponent<ItemInfo>().ability)
                {
                    case ItemAbility.FuelFilling:
                        GetComponent<FuelManager>().AddFuelPercent(0.1f); //add 10% of max fuel
                        break;
                    case ItemAbility.PoleChange:
                        GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>().OnPoleChange(_magnetState); //Change all obstacles' pole
                        break;
                    case ItemAbility.SurfaceCleaning:
                        CleanSurface(); //Clean 3 trash
                        break;
                }
                collision.GetComponent<ItemInfo>().DestroyItem();
            }
            else if (collision.GetComponent<ItemInfo>().type == ItemType.Obstacle)
            {
                AttachObject(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("MagnetField"))
        {
            collision.GetComponent<UnknownMagnetField>().StopDestroyMagnetField();
            EnterMagnetFieldEvent?.Invoke(collision);
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
            if (Vector2.Distance(collision.transform.position, transform.position) > 2.5f)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
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
    public List<ItemInfo> _AttacedObjects { get; private set; }
    [SerializeField] private float _setSpeed;
    [SerializeField] private int _maxStuckCount;
    public float _speed { get; private set; }
    public float _accelSpeed { get; private set; }
    public MagnetState _magnetState { get; set; }
    //UI
    private Transform _handle;
    [SerializeField] private GameObject _gameOverCanvas;
    //Image
    [SerializeField] private List<Sprite> _magnetSprites;
    private Image _speedBar;
    private Image _weightBar;
    private Image _magnetImg;
    private List<Image> _bombImages = new List<Image>();
    private bool onComet;
    [SerializeField] HPManager hpManager;
    private GameUIManager gameUIManager;
    void Start()
    {
        _AttacedObjects = new List<ItemInfo>();
        _handle = GameObject.Find("Handle").transform;
        _speedBar = GameObject.Find("Speed_Image").GetComponent<Image>();
        _magnetImg = GameObject.Find("Current_Magnet").GetComponent<Image>();
        _bombImages.Add(GameObject.Find("Bomb2Img").GetComponent<Image>()); //needs to change
        _bombImages.Add(GameObject.Find("Bomb1Img").GetComponent<Image>()); //needs to change
        _weightBar = GameObject.Find("Mass_Image").GetComponent<Image>();
        gameUIManager = GameObject.Find("Canvas").GetComponent<GameUIManager>();
        _speed = _setSpeed;
        _accelSpeed = _setSpeed * 3f;
        if (GameManager.Instance._bombCount == 0)
            GetComponent<UfoManager>().AddBombCount();
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
        _accelSpeed = _speed * 3f;
        if (_speed <= 0)
        {
            //DestroyUfo();
            Invoke("DestroyUfo", 2f); // 2초 후 DestroyUfo() 호출
        }
    }
    private void AttachObject(GameObject obj)
    {
        GameObject.Find("Canvas").GetComponent<GameUIManager>().showDamaged();
        gameObject.GetComponent<HPManager>().Damaged(10); //add 10% damage
        obj.GetComponent<ItemInfo>().Freeze();
        obj.transform.SetParent(transform);
        _AttacedObjects.Add(obj.GetComponent<ItemInfo>());
        _weightBar.transform.parent.GetComponent<Animator>().Play("Add_Mass");
    }
    public void DestroyEveryObstacle()
    {
        for (int i = 0; i < _AttacedObjects.Count; i++)
        {
            _AttacedObjects[i].DestroyItem();
        }
        GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>().OnBomb();
        _AttacedObjects.Clear();
    }
    //destroy attached objects only
    public void DestroyAttachedObject()
    {
        for (int i = 0; i < _AttacedObjects.Count; i++)
        {
            _AttacedObjects[i].DestroyItem();
        }
        _AttacedObjects.Clear();
    }
    private void DetachObject(GameObject obj)
    {
        obj.GetComponent<ItemInfo>().DestroyItem();
        _AttacedObjects.Remove(obj.GetComponent<ItemInfo>());
    }
    public void DestroyUfo()
    {
        GameManager.Instance._isDead = true;
        //Destroy(gameObject);  
        if (SceneManager.GetActiveScene().name == "Main")
        {
            StartCoroutine(DestroyUfoWithDelay());
        }
        else
        {
            SceneManager.LoadScene("BadEnding");
        }
    }
    IEnumerator DestroyUfoWithDelay()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("Burst_effect").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
        _gameOverCanvas.SetActive(true);
    }
    private void UpdateUI()
    {
        //무게
        if (_weightBar != null)
        {
            _weightBar.fillAmount = 1 - (_speed / _setSpeed);
            if (_weightBar.fillAmount > 0.8f)
            {
                _weightBar.color = new Color(1, 0, 0, 1f);
            }
            else
            {
                _weightBar.color = new Color(1, 1, 1, 1f);
            }
        }
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
        for (int i = 0; i < GameManager.Instance._bombCount; i++)
        {
            _bombImages[i].gameObject.SetActive(true);
        }
    }
    public bool CanUseBomb()
    {
        if (GameManager.Instance._bombCount > 0 && Time.timeScale != 0)
        {
            GameManager.Instance._bombCount--;
            return true;
        }
        return false;
    }
    public void AddBombCount()
    {
        GameManager.Instance._bombCount = Mathf.Min(2, GameManager.Instance._bombCount + 1);
    }
    public void ClearAll()
    {
        _AttacedObjects.Clear();
    }
    public void CleanSurface() //Clean 3 trash
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
                    case ItemAbility.Repair:
                        GetComponent<HPManager>().RepairedPercent(0.15f); //add 10% repair
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

    private IEnumerator CometCrashed()
    {
        onComet = true;
        hpManager.Damaged(5.0f);
        gameUIManager.showDamaged();
        yield return new WaitForSeconds(0.3f);
        onComet = false;
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

                DestroyUfo();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Comet") && !onComet)
            {
                StartCoroutine(CometCrashed());
            }
        }
    }
}
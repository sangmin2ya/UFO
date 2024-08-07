using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResqueUFOController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1.5f;
    private List<ItemInfo> _AttacedObjects = new List<ItemInfo>();
    private TextMeshProUGUI _helpText;
    private bool _isRescued = false;
    private Vector2 _moveVector = Vector2.left;
    // Start is called before the first frame update
    void Start()
    {
        GetAllChildObjects();
        transform.position = new Vector2(15, Random.Range(-3.5f, 3f));
        _helpText = GameObject.Find("HelpText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (_isRescued)
        {
            GameObject.Find("UFO").GetComponent<UfoManager>().AddBombCount();
            _isRescued = false;
            _moveVector += (transform.position.y > GameObject.Find("UFO").transform.position.y ? Vector2.up : Vector2.down);
        }
        transform.Translate(_moveVector.normalized * _moveSpeed * Time.deltaTime);
    }
    private void GetAllChildObjects()
    {
        // 모든 자식 오브젝트를 리스트에 추가
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Item"))
            {
                transform.GetChild(i).GetComponent<ItemMovement>().enabled = false;
                transform.GetChild(i).GetComponent<Rigidbody2D>().isKinematic = true;
                _AttacedObjects.Add(transform.GetChild(i).GetComponent<ItemInfo>());
            }
        }
    }
    public void TakeObstacle(ItemInfo itemInfo)
    {
        _AttacedObjects.Remove(itemInfo);
        if (_AttacedObjects.Count == 0)
        {
            _helpText.text = "고마워요";
            _isRescued = true;
        }
    }
}

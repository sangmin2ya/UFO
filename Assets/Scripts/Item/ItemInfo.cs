using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public float size;

    // type of item. (Item, Obstacle, Stuck)
    public ItemType type;

    // N, S, Off state
    public MagnetState magnetState;

    // ability of item (FuelFilling, SurfaceCleaning, PoleChange)
    public ItemAbility ability;

    public int direction;
    public int index;
    // Sprite Renderer
    private SpriteRenderer spriteRenderer;

    // N, S sprite
    public Sprite spriteN;
    public Sprite spriteS;

    // Item Type sprite
    public Sprite spriteA;
    public Sprite spriteB;
    public Sprite spriteC;

    public bool _notSpawned = false;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �������� N�� S�� ������
        if (!_notSpawned)
            magnetState = (Random.value > 0.5f) ? MagnetState.N : MagnetState.S;

        // If the type is Item, set the ability randomly
        if (type == ItemType.Item)
        {
            ability = (ItemAbility)Random.Range(0, System.Enum.GetValues(typeof(ItemAbility)).Length);
        }

        // change sprite
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if (type == ItemType.Obstacle || type == ItemType.Stuck)
        {
            switch (magnetState)
            {
                case MagnetState.N:
                    spriteRenderer.sprite = spriteN;
                    break;
                case MagnetState.S:
                    spriteRenderer.sprite = spriteS;
                    break;
                case MagnetState.Off:
                    if (gameObject != null)
                    {
                        spriteRenderer.color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                    }
                    break;
            }
        }
        else if (type == ItemType.Item)
        {
            switch (ability)
            {
                case ItemAbility.Repair:
                    spriteRenderer.sprite = spriteA;

                    if (magnetState == MagnetState.N) spriteRenderer.color = new Color(1, 0, 0);
                    else
                        spriteRenderer.color = new Color(0, 0, 1);
                    break;
                case ItemAbility.SurfaceCleaning:
                    spriteRenderer.sprite = spriteB;
                    if (magnetState == MagnetState.N)
                        spriteRenderer.color = new Color(1, 0, 0);
                    else
                        spriteRenderer.color = new Color(0, 0, 1);
                    break;
                case ItemAbility.PoleChange:
                    spriteRenderer.sprite = spriteC;
                    if (magnetState == MagnetState.N)
                        spriteRenderer.color = new Color(1, 0, 0);
                    else
                        spriteRenderer.color = new Color(0, 0, 1);
                    break;
            }
        }

    }


    // �������� �ɷ¿� ���� �ൿ�� ����
    public void ExecuteAbility()
    {
        switch (type)
        {
            case ItemType.Item: // Item
                ExecuteItemAbility();
                break;
            case ItemType.Obstacle: // Obstacle
                ExecuteObstacleAbility();
                break;
            default:
                // Debug.Log("Type not specified");
                break;
        }
    }

    private void ExecuteItemAbility()
    {
        // index_ability�� ���� Item �ɷ� ����
        switch (ability)
        {
            case ItemAbility.Repair: // ���� ����
                FuelFilling();
                break;
            case ItemAbility.SurfaceCleaning: // ���� ��ֹ� ����
                SurfaceCleaning();
                break;
            case ItemAbility.PoleChange: // �ʿ� �ִ� ��� ������Ʈ �ϳ��� pole���� ��ȯ
                PoleChange();
                break;
        }
    }

    // ��ֹ��� ������ N �ذ� S �ظ� �������ָ� ��. ������ �ɷ� ����. �÷��̾�� �ٱ⸸ ��.
    private void ExecuteObstacleAbility()
    {
        //Debug.Log(magnetState);
    }


    public void FuelFilling()
    {
        Debug.Log("Fuel Filling");

        DestroyItem();
    }

    public void SurfaceCleaning()
    {
        Debug.Log("Surface Cleaning");

        DestroyItem();
    }

    public void PoleChange()
    {
        Debug.Log("Pole Change");

        DestroyItem();
    }

    // Change ItemType to stuck and stop moving
    public void Freeze()
    {
        this.type = ItemType.Stuck;
        this.magnetState = MagnetState.Off;
        this.UpdateSprite();
        GetComponent<ItemMovement>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void DestroyItem()
    {
        switch (type)
        {
            case ItemType.Item: // Item
                GameObject.Find("ItemSpawnController").GetComponent<ObstacleSpawner>().DestroyObstacle(direction, index);
                Destroy(gameObject);
                break;

            case ItemType.Obstacle: // Obstacle
                ExecuteObstacleAbility();
                GameObject.Find("SpawnController").GetComponent<ObstacleSpawner>().DestroyObstacle(direction, index);
                StartCoroutine(DestroyItem(gameObject));
                break;
            default:
                Debug.Log("Type not specified");
                StartCoroutine(DestroyItem(gameObject));
                break;
        }
        // Play Effect

    }
    IEnumerator DestroyItem(GameObject item)
    {
        item.transform.GetChild(0).gameObject.SetActive(true);
        item.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(item);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ExecuteAbility();
        }
        if (collision.CompareTag("Item") && this.type == ItemType.Stuck)
        {
            transform.parent.SendMessage("OnChildTriggerEnter2D", collision, SendMessageOptions.DontRequireReceiver);
        }
    }
}

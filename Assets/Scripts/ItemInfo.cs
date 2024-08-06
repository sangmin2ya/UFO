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

    // Sprite Renderer
    private SpriteRenderer spriteRenderer;

    // N, S, Off sprite
    public Sprite spriteN;
    public Sprite spriteS;
    //public Sprite spriteOff;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �������� N�� S�� ������
        magnetState = (Random.value > 0.5f) ? MagnetState.N : MagnetState.S;

        // change sprite
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        switch (magnetState)
        {
            case MagnetState.N:
                spriteRenderer.sprite = spriteN;
                break;
            case MagnetState.S:
                spriteRenderer.sprite = spriteS;
                break;
            /*
            case MagnetState.Off:
                spriteRenderer.sprite = spriteOff;
                break;
            */
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
                Debug.Log("Type not specified");
                break;
        }
    }

    private void ExecuteItemAbility()
    {
        // index_ability�� ���� Item �ɷ� ����
        switch (ability)
        {
            case ItemAbility.FuelFilling: // ���� ����
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
        Debug.Log(magnetState);
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
        GetComponent<ItemMovement>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void DestroyItem()
    {
        // Play Effect
        Destroy(gameObject);
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

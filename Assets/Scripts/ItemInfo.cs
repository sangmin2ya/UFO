using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    // �������� ũ��
    public float size;

    // �������� Ÿ��. (Item, Obstacle, Stuck)
    public ItemType type;

    // N��, S��, Off ����
    public MagnetState magnetState;

    // ������ �ɷ� ����
    public ItemAbility ability;

    // ������, ��ֹ��� �ɷ� ���� ���� ��ȣ
    public int index_ability;

    private void Start()
    {
        // �������� N�� S�� ������
        magnetState = (Random.value > 0.5f) ? MagnetState.N : MagnetState.S;
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
                Debug.Log("Ÿ���� �������� ����");
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


    // Item_�÷��̾� ���� ����
    public void FuelFilling()
    {
        Debug.Log("���� ����");
        // ���� ���� �����ؼ� ���� �÷��ָ� ��.

        DestroyItem();
    }

    // Item_ ���� ��ֹ� ����
    public void SurfaceCleaning()
    {
        Debug.Log("���� ��ֹ� ����");
        // �÷��̾�� ���� ��, �ڽ� ������Ʈ �� Stuck Ÿ�� �������� ����

        DestroyItem();
    }

    public void PoleChange()
    {
        Debug.Log("pole ��ȯ");
        // �迭�� �ִ� �����۵��� magnetState �� ��ȯ
        // N �� S �� ����?

        DestroyItem();
    }

    // Stuck_ItemType �� stuck ���� �ٲٰ�, �̵� ����
    public void Freeze()
    {
        this.type = ItemType.Stuck;
        this.magnetState = MagnetState.Off;
        GetComponent<ItemMovement>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void DestroyItem()
    {
        // �������� �ڿ������� �ִϸ��̼� ���� ����ϸ� ����
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

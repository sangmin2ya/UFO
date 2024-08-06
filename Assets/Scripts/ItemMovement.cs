using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    // �� ũ��(ī�޶� ����)
    public Vector2 viewSize;

    // ������, ��ֹ� �ӵ�
    public float obstacleSpeed = 5f;

    // ������, ��ֹ� ���� �ð�
    private float destroyTime = 5f;

    // ������ �����ϱ� ���� ����
    private float angleRange = 0.5f;

    public int start;

    // �ڼ� ���� ����
    public float magnetRange = 5f;

    // ������� �ӵ�
    public float pullPower = 1f;

    // �о����� �ӵ�
    public float pushPower = 0.5f;

    // �÷��̾� ����
    private Transform player;
    private UfoManager _playerUfoManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerUfoManager = player.GetComponent<UfoManager>();

        // ������ �����ǿ� ���� ���ư��� ���� ������
        Vector2 direction = GetMovementDirection(transform.position);

        Debug.Log(direction);

        rb = GetComponent<Rigidbody2D>();

        // ���� ����ȭ �ϰ� velocity ������
        rb.velocity = direction.normalized * obstacleSpeed;

        // ���� ������ �� �ڿ� �Ҹ�
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= magnetRange)
        {
            AdjustDirectionBasedOnMagnetism(distanceToPlayer);
        }
    }

    void AdjustDirectionBasedOnMagnetism(float distanceToPlayer)
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        ItemInfo itemInfo = GetComponent<ItemInfo>();

        
        if (itemInfo.magnetState == _playerUfoManager._magnetState)
        {
            // ���� ���̸� �ݴ� �������� ���ư�
            rb.velocity = -directionToPlayer * obstacleSpeed * pushPower; // ������ �ݴ� ��������
        }
        else
        {
            // �ٸ� ���̸� �÷��̾� ������ ���ư�
            rb.velocity = directionToPlayer * obstacleSpeed * pullPower; // ������ �÷��̾� ������
        }
        
    }


    Vector2 GetMovementDirection(Vector2 spawnPosition)
    {
        Vector2 direction = Vector2.zero;

        // ���� ��ġ�� ���� ���� ����
        if (spawnPosition.y > viewSize.y) // Top
        {
            direction = new Vector2(Random.Range(-angleRange, angleRange), -1f);
        }
        else if (spawnPosition.y < -viewSize.y) // Bottom
        {
            direction = new Vector2(Random.Range(-angleRange, angleRange), 1f);
        }
        else if (spawnPosition.x < -viewSize.x) // Left
        {
            direction = new Vector2(1f, Random.Range(-angleRange, angleRange));
        }
        else if (spawnPosition.x > viewSize.x) // Right
        {
            direction = new Vector2(-1f, Random.Range(-angleRange, angleRange));
        }

        // ����� ������ ���ư��� �ʵ��� ���� ����
        if (Mathf.Abs(spawnPosition.x) > Mathf.Abs(spawnPosition.y))
        {
            // �¿찡 �� �ִٸ� ���� ���⸸ �ٲٱ�
            direction.x = (spawnPosition.x > 0) ? -1f : 1f;
            direction.y = Random.Range(-angleRange, angleRange); // ���� �������� ����� ������ ���ư��� �ʵ��� ��
        }
        else
        {
            // ���ϰ� �� �ִٸ� �¿� ���⸸ �ٲٱ�
            direction.y = (spawnPosition.y > 0) ? -1f : 1f;
            direction.x = Random.Range(-angleRange, angleRange); // ���� �������� ����� ������ ���ư��� �ʵ��� ��
        }

        return direction;
    }
}

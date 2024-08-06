using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    // 맵 크기(카메라 범위)
    public Vector2 viewSize;

    // 아이템, 장애물 속도
    public float obstacleSpeed = 5f;

    // 아이템, 장애물 제거 시간
    private float destroyTime = 5f;

    // 각도를 설정하기 위한 범위
    private float angleRange = 0.5f;

    public int start;

    // 자성 반응 범위
    public float magnetRange = 5f;

    // 당겨지는 속도
    public float pullPower = 1f;

    // 밀어지는 속도
    public float pushPower = 0.5f;

    // 플레이어 정보
    private Transform player;
    private PlayerMagnet playerMagnet;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMagnet = player.GetComponent<PlayerMagnet>();

        // 리스폰 포지션에 따라 날아가는 방향 정해줌
        Vector2 direction = GetMovementDirection(transform.position);

        Debug.Log(direction);

        rb = GetComponent<Rigidbody2D>();

        // 방향 정규화 하고 velocity 정해줌
        rb.velocity = direction.normalized * obstacleSpeed;

        // 접촉 안했을 때 자연 소멸
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // 플레이어와의 거리 계산
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

        
        if (itemInfo.magnetState == playerMagnet.magnetState)
        {
            // 같은 극이면 반대 방향으로 날아감
            rb.velocity = -directionToPlayer * obstacleSpeed * pushPower; // 빠르게 반대 방향으로
        }
        else
        {
            // 다른 극이면 플레이어 쪽으로 날아감
            rb.velocity = directionToPlayer * obstacleSpeed * pullPower; // 빠르게 플레이어 쪽으로
        }
        
    }


    Vector2 GetMovementDirection(Vector2 spawnPosition)
    {
        Vector2 direction = Vector2.zero;

        // 스폰 위치에 따라 방향 설정
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

        // 가까운 면으로 날아가지 않도록 방향 조정
        if (Mathf.Abs(spawnPosition.x) > Mathf.Abs(spawnPosition.y))
        {
            // 좌우가 더 멀다면 상하 방향만 바꾸기
            direction.x = (spawnPosition.x > 0) ? -1f : 1f;
            direction.y = Random.Range(-angleRange, angleRange); // 가로 방향으로 가까운 면으로 날아가지 않도록 함
        }
        else
        {
            // 상하가 더 멀다면 좌우 방향만 바꾸기
            direction.y = (spawnPosition.y > 0) ? -1f : 1f;
            direction.x = Random.Range(-angleRange, angleRange); // 세로 방향으로 가까운 면으로 날아가지 않도록 함
        }

        return direction;
    }
}

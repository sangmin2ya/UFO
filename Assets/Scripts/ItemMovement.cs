using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    // size of camera view
    public Vector2 viewSize;

    // item speed
    public float obstacleSpeed = 5f;

    // destroy time of item
    private float destroyTime = 5f;

    private float angleRange = 0.5f;

    public int start;

    // Magnetic influence range
    public float magnetRange = 2.5f;

    public float pullPower = 1f;

    public float pushPower = 0.5f;

    // player
    private Transform player;
    private UfoManager _playerUfoManager;

    // item
    private ItemInfo itemInfo;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerUfoManager = player.GetComponent<UfoManager>();
        itemInfo = GetComponent<ItemInfo>();
        // Set movement direction
        Vector2 direction = GetMovementDirection(transform.position);

        //Debug.Log(direction);

        rb = GetComponent<Rigidbody2D>();

        // Normalize direction and set speed
        rb.velocity = direction.normalized * obstacleSpeed;


        // Check during destroyTime
        StartCoroutine(CheckAndDestroy());
    }

    void Update()
    {
        // Calculate distance from player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= magnetRange)
        {
            AdjustDirectionBasedOnMagnetism(distanceToPlayer);
        }
    }

    // Movement settings according to magnetism
    void AdjustDirectionBasedOnMagnetism(float distanceToPlayer)
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        if (itemInfo.magnetState == _playerUfoManager._magnetState)
        {
            // Magnetism is the same as the player.
            rb.velocity = -directionToPlayer * obstacleSpeed * pushPower;
        }
        else
        {
            // Magnetism is different from the player
            rb.velocity = directionToPlayer * obstacleSpeed * pullPower;
        }

    }

    IEnumerator CheckAndDestroy()
    {
        yield return new WaitForSeconds(destroyTime);

        // Stuck 상태가 아닌 경우에만 파괴
        if (itemInfo.type != ItemType.Stuck)
        {
            Destroy(gameObject);
        }
    }


    // Set movement direction
    Vector2 GetMovementDirection(Vector2 spawnPosition)
    {
        Vector2 direction = Vector2.zero;

        // Set direction based on spawn location
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

        // Adjust direction to avoid flying into the nearby plane
        if (Mathf.Abs(spawnPosition.x) > Mathf.Abs(spawnPosition.y))
        {
            // If left and right are further apart, just change the up and down direction.
            direction.x = (spawnPosition.x > 0) ? -1f : 1f;
            direction.y = Random.Range(-angleRange, angleRange); // Avoid flying to the nearest surface in the horizontal direction.
        }
        else
        {
            // If the top and bottom are further apart, just change the left and right directions.
            direction.y = (spawnPosition.y > 0) ? -1f : 1f;
            direction.x = Random.Range(-angleRange, angleRange); // Avoid flying towards the near surface in the vertical direction.
        }

        return direction;
    }
}

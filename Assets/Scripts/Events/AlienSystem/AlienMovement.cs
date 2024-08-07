using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    // size of camera view
    public Vector2 viewSize;

    // item speed
    private float obstacleSpeed = 2f;
    // item rotation speed
    private float rotationSpeed;

    private float angleRange = 0.2f;

    public int start;




    void Start()
    {
        // Set movement direction
        Vector2 direction = GetMovementDirection(transform.position);
        rb = GetComponent<Rigidbody2D>();

        // Normalize direction and set speed
        rb.velocity = direction.normalized * obstacleSpeed;

        // Set random rotation speed
        rotationSpeed = Random.Range(40f, 70f);

    }

    void Update()
    {
        // Rotate item
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
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

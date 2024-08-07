using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInBlackhole : MonoBehaviour
{
    public float forceMagnitude = 5f;  // Adjust the force magnitude as needed
    public float cooldownTime = 0.5f;  // Cooldown time in seconds
    private bool isCooldown = false;   // Flag to track cooldown state

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is tagged as "Obstacle" and if cooldown is not active
        if (collision.gameObject.tag == "Obstacle" && !isCooldown)
        {
            // Get the direction vector from the player to the obstacle
            Vector2 collisionDirection = transform.position - collision.transform.position;

            // Normalize the direction vector to get the direction only (unit vector)
            collisionDirection.Normalize();

            // Apply a force in the opposite direction
            GetComponent<Rigidbody2D>().AddForce(collisionDirection * forceMagnitude, ForceMode2D.Impulse);

            // Destroy the obstacle
            Destroy(collision.gameObject);

            // Start the cooldown coroutine
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        // Set the cooldown flag to true
        isCooldown = true;

        // Wait for the cooldown time
        yield return new WaitForSeconds(cooldownTime);

        // Reset the cooldown flag to false
        isCooldown = false;
    }
}

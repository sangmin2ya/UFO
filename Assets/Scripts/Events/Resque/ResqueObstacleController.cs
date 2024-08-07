using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResqueObstacleController : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= GetComponent<ItemMovement>().magnetRange)
        {
            GetComponent<ItemMovement>().enabled = true;
            GetComponent<ItemInfo>().enabled = true;
            GetComponent<Rigidbody2D>().isKinematic = false;

            transform.parent.GetComponent<ResqueUFOController>().TakeObstacle(GetComponent<ItemInfo>());
            transform.parent = null;
            this.enabled = false;
        }
    }
}

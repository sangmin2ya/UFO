using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float speed = 1f;

    float total_dist = 0;

    private void FixedUpdate() {
        transform.Translate(Vector2.right* -speed);
        total_dist += speed;

        if(total_dist >= 75) Destroy(gameObject);
    }


}

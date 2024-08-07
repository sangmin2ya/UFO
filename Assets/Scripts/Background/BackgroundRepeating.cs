using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeating : MonoBehaviour
{
    public float speed = 1f;

    private void Update()
    {
        transform.Translate(Vector2.right * -speed);

        if (transform.position.x <= -38) transform.position = new Vector2(19, transform.position.y);
    }
}

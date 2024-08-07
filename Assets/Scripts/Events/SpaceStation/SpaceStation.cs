using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    [SerializeField] bool NiceDocking = false;
    [SerializeField] float speed = .02f;
    private void FixedUpdate()
    {
        transform.parent.gameObject.transform.Translate(Vector2.up * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FuelManager>(out var result))
        {
            print("플레이어 충돌");
            if(!NiceDocking) result._fuel *= .8f;
            else
            {
                //좋은 보상
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    [SerializeField] bool NiceDocking = false;
    [SerializeField] float speed = .02f;

    [SerializeField] Collider2D good, bad;
    private void FixedUpdate()
    {
        transform.parent.gameObject.transform.Translate(Vector2.up * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FuelManager>(out var result))
        {
            good.enabled = false;
            bad.enabled = false;
            print("�÷��̾� �浹");
            if(!NiceDocking) result._fuel *= .8f;
            else
            {
                //���� ����
            }
        }

    }
}

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

            if (!NiceDocking)
            {
                // 덜 좋은 효과. 최대 75%까지만 주유. 이미 75% 이상이라면 주유 되지 않음
                result._fuel = Mathf.Max(75, result._fuel);
            }
            else
            {
                // 좋은 효과. 주유 가득
                result.AddFuel(100);
            }
        }

    }
}

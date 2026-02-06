using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElonMissile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float burst_Time = 5f;
    GameObject player;
    Vector2 direction;
    public MagnetState state;
    public bool isFollowPlayer = false;

    [SerializeField] Sprite[] missiles;
    [SerializeField] Gradient[] gradients;
    [SerializeField] GameObject BurstEffect;
    float t_burst = 0;

    bool isTurn = false;

    // Magnetic influence range
    public float magnetRange = 2.5f;

    private void Start()
    {
        if (isFollowPlayer)
        {
            GetComponent<SpriteRenderer>().sprite = state == MagnetState.N ? missiles[0] : missiles[1];
            GetComponent<SpriteRenderer>().color = state == MagnetState.N ? new Color(1, 0.5f, 0.5f, 1) : new Color(0.5f, 0.5f, 1f, 1);
            GetComponentInChildren<TrailRenderer>().colorGradient = state == MagnetState.N ? gradients[0] : gradients[1];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = missiles[2];
            GetComponentInChildren<TrailRenderer>().colorGradient = gradients[2];
        }

        player = GameObject.FindGameObjectWithTag("Player");
        checkDirection();
    }

    private void Update()
    {
        t_burst += Time.deltaTime;
        if (t_burst > burst_Time)
        {
            var effect = Instantiate(BurstEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1);
            Destroy(gameObject);
        }

        if (state != player.GetComponent<UfoManager>()._magnetState) checkDirection();
        else if (state == player.GetComponent<UfoManager>()._magnetState && Vector2.Distance(transform.position, player.transform.position) <= magnetRange && !isTurn)
        {
            isTurn = true;
            direction *= -1;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }


    void checkDirection()
    {
        isTurn = false;
        direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<HPManager>(out var result))
        {
            result._hp *= .9f;
            var effect = Instantiate(BurstEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1);
            Destroy(gameObject);
        }
    }
}

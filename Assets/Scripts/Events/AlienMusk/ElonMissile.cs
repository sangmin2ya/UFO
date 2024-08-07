using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElonMissile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float burst_Time = 5f;
    GameObject player;
    Animator anim;
    bool shooted = false;
    Vector2 direction;
    public MagnetState state;
    public bool isFollowPlayer = false;

    [SerializeField] Sprite[] missiles;
    [SerializeField] Gradient[] gradients;
    [SerializeField] GameObject BurstEffect;
    float t_burst = 0;
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
        anim = GetComponent<Animator>();
        anim.Play("Elon_Missile");
        checkDirection();
    }

    private void Update()
    {
        if (!shooted) return;
        t_burst += Time.deltaTime;
        if (t_burst > burst_Time)
        {
            var effect = Instantiate(BurstEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1);
            Destroy(gameObject);
        }

        if (state != player.GetComponent<UfoManager>()._magnetState && isFollowPlayer) checkDirection();
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    public void shootMissile() => shooted = true;

    void checkDirection()
    {
        direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<FuelManager>(out var result))
        {
            result._fuel *= .9f;
            var effect = Instantiate(BurstEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blackhole : MonoBehaviour
{
    public bool onBlackhole;
    [SerializeField] ObstacleManager bomb;
    [Header("������Ʈ �������� �ӵ�")]
    public float basePullStrength = 100f;

    void FixedUpdate()
    {
        if (onBlackhole)
        {
            GameObject[] Obstacles = bomb.FindAllObstacles();
            // ��� ������ٵ�2D�� ã���ϴ�.

            foreach (var Obstacle in Obstacles)
            {
                Rigidbody2D rb = Obstacle.GetComponent<Rigidbody2D>();

                // ����Ȧ�� �� ������ٵ� ������ �Ÿ��� ����մϴ�.
                float distance = Vector2.Distance(transform.position, rb.position);
                float pullStrength;

                // �÷��̾�� �Ϲ� ������Ʈ�� ���Է��� �ٸ��� �����մϴ�.
                if (!Obstacle.CompareTag("Player"))
                {
                    rb.position = Vector2.MoveTowards(rb.position, transform.position, basePullStrength * Time.deltaTime);
                    Obstacle.gameObject.GetComponent<ItemMovement>().enabled = false;
                    Obstacle.gameObject.GetComponent<ItemInfo>().enabled = false;
                }
            }
        }
    }

    public void SpawnBlackhole(float time)
    {
        StartCoroutine(AppearBlackhole(time));
    }

    IEnumerator AppearBlackhole(float time)
    {
        onBlackhole = true;
        yield return new WaitForSeconds(time);
        onBlackhole = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}

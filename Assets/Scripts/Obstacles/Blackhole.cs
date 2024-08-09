using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public bool onBlackhole;
    [SerializeField] ObstacleManager bomb;
    [Header("������Ʈ �������� �ӵ�")]
    public float basePullStrength = 100f; // �⺻ ���Է�
    [Header("�÷��̾� �������� �ӵ�")]
    public float playerPullStrength = 5f; // �÷��̾ ���� ���Է�

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
                if (rb.CompareTag("Player"))
                {
                    pullStrength = playerPullStrength;
                    // �Ÿ� �ݺ�ʷ� ���Է��� ������ŵ�ϴ�.
                    pullStrength /= distance;

                    // ����Ȧ �������� ���� ���մϴ�.
                    Vector2 direction = (Vector2)transform.position - rb.position;
                    rb.AddForce(direction.normalized * pullStrength * Time.fixedDeltaTime);
                }
                else
                {
                    rb.isKinematic = true;
                    rb.position = Vector2.MoveTowards(rb.position, transform.position, basePullStrength * Time.deltaTime);
                    Obstacle.GetComponent<CircleCollider2D>().enabled = false;
                }

                Obstacle.gameObject.GetComponent<ItemMovement>().enabled = false;
                Obstacle.gameObject.GetComponent<ItemInfo>().enabled = false;
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
        yield return new WaitForSeconds(2.0f);
        GameObject[] Obstacles = bomb.FindAllObstacles();
        foreach (var obstacle in Obstacles)
        {
            if (obstacle != null)
            {
                obstacle.gameObject.GetComponent<ItemMovement>().enabled = true;
                obstacle.gameObject.GetComponent<ItemInfo>().enabled = true;
                obstacle.GetComponent<CircleCollider2D>().enabled = true;
            }
        }
    }
}

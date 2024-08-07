using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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


    void Start()
    {
        SpawnBlackhole(5.0f);
    }

    void FixedUpdate()
    {
        if(onBlackhole)
        {
            GameObject[] Obstacles = bomb.FindAllObstacles();
            // ��� ������ٵ�2D�� ã���ϴ�.
            Rigidbody2D[] allRigidbodies = FindObjectsOfType<Rigidbody2D>();
            Debug.Log("���� : " + allRigidbodies.Length);

            foreach (var Obstacle in Obstacles)
            {
                Rigidbody2D rb = Obstacle.GetComponent<Rigidbody2D>();
                // ����Ȧ�� �� ������ٵ� ������ �Ÿ��� ����մϴ�.
                float distance = Vector2.Distance(transform.position, rb.position);
                float pullStrength;

                // �÷��̾�� �Ϲ� ������Ʈ�� ���Է��� �ٸ��� �����մϴ�.
                if (rb.gameObject.tag == "Player")
                {
                    pullStrength = playerPullStrength;
                }
                else
                {
                    pullStrength = basePullStrength;
                }

                // �Ÿ� �ݺ�ʷ� ���Է��� ������ŵ�ϴ�.
                pullStrength /= distance;

                // ����Ȧ �������� ���� ���մϴ�.
                Vector2 direction = (Vector2)transform.position - rb.position;
                rb.AddForce(direction * pullStrength * Time.fixedDeltaTime);
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
}

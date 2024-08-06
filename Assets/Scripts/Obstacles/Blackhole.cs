using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public bool onBlackhole;

    [Header("오브젝트 빨려들어가는 속도")]
    public float basePullStrength = 100f; // 기본 흡입력
    [Header("플레이어 빨려들어가는 속도")]
    public float playerPullStrength = 5f; // 플레이어를 향한 흡입력

    void Start()
    {
        SpawnBlackhole(5.0f);
    }

    void FixedUpdate()
    {
        if(onBlackhole)
        {
            // 모든 리지드바디2D를 찾습니다.
            Rigidbody2D[] allRigidbodies = FindObjectsOfType<Rigidbody2D>();
            Debug.Log("길이 : " + allRigidbodies.Length);

            foreach (Rigidbody2D rb in allRigidbodies)
            {
                // 블랙홀과 각 리지드바디 사이의 거리를 계산합니다.
                float distance = Vector2.Distance(transform.position, rb.position);
                float pullStrength;

                // 플레이어와 일반 오브젝트의 흡입력을 다르게 설정합니다.
                if (rb.gameObject.tag == "Player")
                {
                    pullStrength = playerPullStrength;
                }
                else
                {
                    pullStrength = basePullStrength;
                }

                // 거리 반비례로 흡입력을 증가시킵니다.
                pullStrength /= distance;

                // 블랙홀 방향으로 힘을 가합니다.
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

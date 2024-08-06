using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("스폰 할 오브젝트")]
    [SerializeField] GameObject obstacle;

    // X축, Y축 배열 초기화 값
    public const int Xlength = 25;
    public const int Ylength = 15;
    [Header("중심 0,0으로부터 X축, Y축의 길이의 폭")]
    public float xRange = 0;
    public float yRange = 0;
    [Header("오브젝트 지름")]
    public float obstacleRadius;

    [Header("오브젝트 생성 갯수/간격(최솟값~최댓값)")]
    public int[] spawnCount = new int[2];
    public float[] Interval = new float[2];
    [Header("오브젝트 여러개 생성 시 생성 간격")]
    [Range(0, 3)] public float spawnInterval;

    [Header("오브젝트 배열(크기를 지정해주면 그 크기만큼의 범위 내에서 생성)")]
    public int[] topObstacleList = new int[Xlength];
    public int[] bottomObstacleList = new int[Xlength];
    public int[] leftObstacleList = new int[Ylength];
    public int[] rightObstacleList = new int[Ylength];

    [Header("퍼센트는 오브젝트 배열 값 중 제일 큰 값 +1로 해주세요")]
    public float[] percents = new float[Xlength+1];

    void Start()
    {
        // 각 방향 정보 배열 가중치를 주기 위한 숫자 초기화
        for (int i = 0; i < topObstacleList.Length; i++)
        {
            topObstacleList[i] = 1;
            bottomObstacleList[i] = 1;
        }

        for (int i = 0; i < leftObstacleList.Length; i++)
        {
            leftObstacleList[i] = 1;
            rightObstacleList[i] = 1;
        }

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(true)
        {
            int count = Random.Range(spawnCount[0], spawnCount[1]+1);
            for (int i = 0; i < count; i++)
            {
                SpwanObstacle(); 
                yield return new WaitForSeconds(spawnInterval);
            }
            float tempinterval = Random.Range(Interval[0], Interval[1]);
            yield return new WaitForSeconds(tempinterval);
        }
    }

    public void SpwanObstacle()
    {
        // 방향은 각 25%
        int direction = Random.Range(0, 4);
        int index = 0;
        float tempnum;

        switch (direction)
        {
            case 0:
                for (int i = 0; i<topObstacleList.Length; i++)
                {
                    // 각 배열의 숫자를 역수로 한 확률을 계속 더해나가고
                    percents[i + 1] = percents[i] + 1/ (float)topObstacleList[i];
                }

                // 그 더한 값의 총합까지 범위에서 랜덤으로 값을 하나 뽑아서
                tempnum = Random.Range(0, percents[topObstacleList.Length]);

                // 그 값이 역수로 한 값의 범위에 있으면 그 값으로 설정
                for (int i = topObstacleList.Length; i > 0; i--)
                {
                    if (tempnum < percents[i])
                    {
                        index = i;
                    }
                }

                // 그렇게 구한 배열은 1부터 시작이어서 1 빼줌(percents[i+1]임)
                index--;
                // 그 번째 배열에 오브젝트 추가했다고 배열에 값 추가
                topObstacleList[index]++;
                GameObject obstacleTop = GameObject.Instantiate(obstacle, new Vector2(-xRange + obstacleRadius * index, yRange), Quaternion.identity);
                // 배열에 방향 저장
                obstacleTop.GetComponent<Obstacle>().start = 0;
                break;

            case 1:
                for (int i = 0; i < bottomObstacleList.Length; i++)
                {
                    percents[i + 1] = percents[i] + 1/ (float)bottomObstacleList[i];
                }

                tempnum = Random.Range(0, percents[topObstacleList.Length]);

                for (int i = bottomObstacleList.Length; i > 0; i--)
                {
                    if (tempnum < percents[i])
                    {
                        index = i;
                    }
                }

                index--;
                bottomObstacleList[index]++;
                GameObject obstacleBottom = GameObject.Instantiate(obstacle, new Vector2(-xRange + obstacleRadius * index, -yRange), obstacle.transform.rotation);
                obstacleBottom.GetComponent<Obstacle>().start = 1;
                break;

            case 2:
                for (int i = 0; i < leftObstacleList.Length; i++)
                {
                    percents[i + 1] = percents[i] + 1 / (float)leftObstacleList[i];
                }

                tempnum = Random.Range(0, percents[leftObstacleList.Length]);

                for (int i = leftObstacleList.Length; i > 0; i--)
                {
                    if (tempnum < percents[i])
                    {
                        index = i;
                    }
                }

                index--;
                leftObstacleList[index]++;
                GameObject obstacleLeft = GameObject.Instantiate(obstacle, new Vector2(-xRange, -yRange + obstacleRadius * index), obstacle.transform.rotation);
                obstacleLeft.GetComponent<Obstacle>().start = 2;
                break;

            case 3:
                for (int i = 0; i < rightObstacleList.Length; i++)
                {
                    percents[i + 1] = percents[i] + 1 / (float)rightObstacleList[i];
                }

                tempnum = Random.Range(0, percents[rightObstacleList.Length]);

                for (int i = rightObstacleList.Length; i > 0; i--)
                {
                    if (tempnum < percents[i])
                    {
                        index = i;
                    }
                }

                index--;
                rightObstacleList[index]++;
                GameObject obstacleRight = GameObject.Instantiate(obstacle, new Vector2(xRange, -yRange + obstacleRadius * index), obstacle.transform.rotation);
                obstacleRight.GetComponent<Obstacle>().start = 3;
                break;
        }
    }
}

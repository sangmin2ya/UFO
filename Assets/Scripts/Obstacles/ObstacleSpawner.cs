using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstacle;

    public const int Xlength = 25;
    public const int Ylength = 15;

    public float xRange = 0;
    public float yRange = 0;
    public float obstacleRadius;

    public int[] spawnCount = new int[2];
    public float[] Interval = new float[2];
    [Range(0, 3)] public float spawnInterval;

    public int[] topObstacleList = new int[Xlength];
    public int[] bottomObstacleList = new int[Xlength];
    public int[] leftObstacleList = new int[Ylength];
    public int[] rightObstacleList = new int[Ylength];

    public float[] percents = new float[Xlength+1];

    void Start()
    {
        ResetList();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(true)
        {
            int count = Random.Range(spawnCount[0], spawnCount[1]+1);
            for (int i = 0; i < count; i++)
            {
                SpawnObstacle(); 
                yield return new WaitForSeconds(spawnInterval);
            }
            float tempinterval = Random.Range(Interval[0], Interval[1]);
            yield return new WaitForSeconds(tempinterval);
        }
    }

    public void ResetList()
    {
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
    }

    public void DestroyObstacle(int direction, int index)
    {
        switch (direction)
        {
            case 0:
                topObstacleList[index]--;
                break;
            case 1:
                bottomObstacleList[index]--;
                break;
            case 2:
                leftObstacleList[index]--;
                break;
            case 3:
                rightObstacleList[index]--;
                break;
        }
    }

    public void SpawnObstacle()
    {
        // ������ �� 25%
        int direction = Random.Range(0, 4);
        int index = 0;
        float tempnum;

        switch (direction)
        {
            case 0:
                for (int i = 0; i<topObstacleList.Length; i++)
                {
                    // �� �迭�� ���ڸ� ������ �� Ȯ���� ��� ���س�����
                    percents[i + 1] = percents[i] + 1/ (float)topObstacleList[i];
                }

                // �� ���� ���� ���ձ��� �������� �������� ���� �ϳ� �̾Ƽ�
                tempnum = Random.Range(0, percents[topObstacleList.Length]);

                // �� ���� ������ �� ���� ������ ������ �� ������ ����
                for (int i = topObstacleList.Length; i > 0; i--)
                {
                    if (tempnum <= percents[i])
                    {
                        index = i;
                    }
                }

                // �׷��� ���� �迭�� 1���� �����̾ 1 ����(percents[i+1]��)
                index --;
                Debug.Log(index);
                // �� ��° �迭�� ������Ʈ �߰��ߴٰ� �迭�� �� �߰�
                topObstacleList[index]++;
                GameObject obstacleTop = GameObject.Instantiate(obstacle, new Vector2(-xRange + obstacleRadius * index, yRange), Quaternion.identity);
                // �迭�� ���� ����
                obstacleTop.GetComponent<ItemInfo>().direction = 0;
                obstacleTop.GetComponent<ItemInfo>().index = index;
                break;

            case 1:
                for (int i = 0; i < bottomObstacleList.Length; i++)
                {
                    percents[i + 1] = percents[i] + 1/ (float)bottomObstacleList[i];
                }

                tempnum = Random.Range(0, percents[topObstacleList.Length]);

                for (int i = bottomObstacleList.Length; i > 0; i--)
                {
                    if (tempnum <= percents[i])
                    {
                        index = i;
                    }
                }

                index--;
                bottomObstacleList[index]++;
                GameObject obstacleBottom = GameObject.Instantiate(obstacle, new Vector2(-xRange + obstacleRadius * index, -yRange), obstacle.transform.rotation);
                obstacleBottom.GetComponent<ItemInfo>().direction = 1;
                obstacleBottom.GetComponent<ItemInfo>().index = index;
                break;

            case 2:
                for (int i = 0; i < leftObstacleList.Length; i++)
                {
                    percents[i + 1] = percents[i] + 1 / (float)leftObstacleList[i];
                }

                tempnum = Random.Range(0, percents[leftObstacleList.Length]);

                for (int i = leftObstacleList.Length; i > 0; i--)
                {
                    if (tempnum <= percents[i])
                    {
                        index = i;
                    }
                }

                index--;
                GameObject obstacleLeft = GameObject.Instantiate(obstacle, new Vector2(-xRange, -yRange + obstacleRadius * index), obstacle.transform.rotation);
                obstacleLeft.GetComponent<ItemInfo>().direction = 2;
                obstacleLeft.GetComponent<ItemInfo>().index = index;
                break;

            case 3:
                for (int i = 0; i < rightObstacleList.Length; i++)
                {
                    percents[i + 1] = percents[i] + 1 / (float)rightObstacleList[i];
                }

                tempnum = Random.Range(0, percents[rightObstacleList.Length]);

                for (int i = rightObstacleList.Length; i > 0; i--)
                {
                    if (tempnum <= percents[i])
                    {
                        index = i;
                    }
                }

                index--;
                GameObject obstacleRight = GameObject.Instantiate(obstacle, new Vector2(xRange, -yRange + obstacleRadius * index), obstacle.transform.rotation);
                obstacleRight.GetComponent<ItemInfo>().direction = 3;
                obstacleRight.GetComponent<ItemInfo>().index = index;
                break;
        }
    }
}

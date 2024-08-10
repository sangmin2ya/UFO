using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEventManager : MonoBehaviour
{
    [SerializeField] ObstacleSpawner obstacleSpawner;
    [SerializeField] ObstacleSpawner itemManager;
    [SerializeField] float[] stagelength;

    [SerializeField] int[] obstacleInterval;
    [SerializeField] int[] obstacleSpawnCount;

    [SerializeField] int[] itemInterval;
    [SerializeField] int[] itemSpawnCount;

    public void MapSetting(int lv)
    {
        obstacleSpawner.spawnCount = obstacleSpawnCount[lv];
        obstacleSpawner.Interval = obstacleInterval[lv];
        itemManager.spawnCount = itemSpawnCount[lv];
        itemManager.Interval = itemInterval[lv];

        GameManager.Instance.SetProgressSpeed(stagelength[lv]);
    }

}
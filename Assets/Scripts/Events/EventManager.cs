using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] ObstacleSpawner obstacleSpawner;
    [SerializeField] ObstacleSpawner itemManager;
    // 외계인 생성기
    // 워프게이트 생성기
    // 파워 업 물품 생성기

    [SerializeField] float[] stagelength;

    [SerializeField] int[] obstacleInterval;
    [SerializeField] int[] obstacleSpawnCount;

    [SerializeField] int[] itemInterval;
    [SerializeField] int[] itemSpawnCount;

    [SerializeField] Blackhole blackhole;
    [SerializeField] GameObject UnknownMagneticField;
    [SerializeField] GameObject SpaceStation;
    [SerializeField] GameObject CometShower;

    public void MapSetting(int lv)
    {
        obstacleSpawner.spawnCount = obstacleSpawnCount[lv];
        obstacleSpawner.Interval = obstacleInterval[lv];
        itemManager.spawnCount = itemSpawnCount[lv];
        itemManager.Interval = itemInterval[lv];

        GameManager.Instance.SetProgressSpeed(stagelength[lv]);
    }

    public void OnBlackhole(float time)
    {
        blackhole.SpawnBlackhole(time);
    }

    public void OnUnknownMagneticField()
    {
        Instantiate(UnknownMagneticField);
    }

    public void OnSpaceStation()
    {
        Instantiate(SpaceStation);
    }

    public void OnCometShower()
    {
        Instantiate(CometShower);
    }
}

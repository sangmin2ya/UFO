using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] ObstacleSpawner obstacleSpawner;
    [SerializeField] ObstacleSpawner itemManager;

    [SerializeField] private GameObject obstacleSpawnerObj;
    [SerializeField] private GameObject itemManagerObj;
    
    // �ܰ��� ������
    // ��������Ʈ ������
    // �Ŀ� �� ��ǰ ������

    [SerializeField] float[] stagelength;

    [SerializeField] int[] obstacleInterval;
    [SerializeField] int[] obstacleSpawnCount;

    [SerializeField] int[] itemInterval;
    [SerializeField] int[] itemSpawnCount;

    [SerializeField] Blackhole blackhole;
    [SerializeField] GameObject UnknownMagneticField;
    [SerializeField] GameObject SpaceStation;
    [SerializeField] GameObject CometShower;
    [SerializeField] GameObject EpicItems;
    [SerializeField] GameObject commonEpics;

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
        blackhole.gameObject.SetActive(true);
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

    public void OnEpicItems()
    {
        EpicItems.SetActive(true);
    }

    public void OffEpicItems()
    {
        commonEpics.SetActive(false);
    }

    public void OnObstacles()
    {
        obstacleSpawnerObj.SetActive(true);
        itemManagerObj.SetActive(true);
    }

    public void OffObstacles()
    {
        obstacleSpawnerObj.SetActive(false);
        itemManagerObj.SetActive(false);
    }
}

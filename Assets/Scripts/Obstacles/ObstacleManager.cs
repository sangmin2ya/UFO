using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] FindAllObstacles()
    {
        GameObject[] tempGameObjects = GameObject.FindGameObjectsWithTag("Item");
        return tempGameObjects;
    }

    public void OnBomb()
    {
        GameObject[] delGameObstacles = FindAllObstacles();
        foreach (var delObstacle in delGameObstacles)
        {
            delObstacle.GetComponent<ItemInfo>().DestroyItem();
        }
    }
    public void OnPoleChange(MagnetState ufoMagnetState)
    {
        GameObject[] changeGameObstacles = FindAllObstacles();
        foreach (var changeObstacle in changeGameObstacles)
        {
            ItemInfo itemInfo = changeObstacle.GetComponent<ItemInfo>();
            if (itemInfo.type == ItemType.Obstacle)
            {
                itemInfo.magnetState = ufoMagnetState;
                itemInfo.UpdateSprite();
            }
        }
    }
}

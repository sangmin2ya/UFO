using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
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
            Destroy(delObstacle);
        }
    }
}

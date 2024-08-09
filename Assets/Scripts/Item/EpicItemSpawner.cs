using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EpicItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject epicItem;
    [Range(0,120)]
    [SerializeField] private float minInterval;
    [Range(0, 120)]
    [SerializeField] private float maxInterval;

    void Start()
    {
        StartCoroutine(SpawnItem());
    }

    IEnumerator SpawnItem()
    {
        while (true)
        {
            float tempTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(tempTime);
            /*
            float direction = Random.Range(0, 1.0f);
            float tempX;
            float tempY;
            if (direction <= 0.5)
            {
                tempX = Random.Range(-12, 12);
                float tempdirection = Random.Range(0, 1.0f);
                if (tempdirection <= 0.5)
                {
                    tempY = 7;
                }
                else
                {
                    tempY = -7;
                }
            }
            else
            {
                tempY = Random.Range(-7, 7);
                float tempdirection = Random.Range(0, 1.0f);
                if (tempdirection <= 0.5)
                {
                    tempX = 12;
                }
                else
                {
                    tempX = -12;
                }
            }

            Instantiate(epicItem, new Vector2(tempX, tempY), Quaternion.identity);
            */

            Instantiate(epicItem);
        }
    }
}

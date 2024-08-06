using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] GameObject[] Planets;

    float minSize = 1f;
    float maxSize = 5f;

    private void Start()
    {
        SpawnPlanet();
    }

    IEnumerator spawnrandomPlanet()
    {
        int x = 0;
        List<int> unique_points = new List<int>();
        foreach (var point in SpawnPoints)
        {
            int rand = Random.Range(0, 101);
            if (rand <= 10 && x <= 4)
            {
                x += 1;
                int planet_rand = Random.Range(0, Planets.Length);
                if (planet_rand != unique_points.Find(x => x == planet_rand))
                {
                    unique_points.Add(planet_rand);
                    var planet = Instantiate(Planets[planet_rand], point.position, Quaternion.identity);
                    float random_scale = Random.Range(minSize, maxSize + 1);
                    planet.transform.localScale = new Vector3(random_scale, random_scale, random_scale);
                    planet.GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(-random_scale);
                    planet.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 1f) / (5.5f - random_scale), Random.Range(0, 1f) / (5.5f - random_scale), Random.Range(0, 1f) / (5.5f - random_scale));
                    planet.GetComponent<Planet>().speed = .0075f * random_scale;
                }
            }
        }
        yield return new WaitForSeconds(Random.Range(5f, 8f));
        SpawnPlanet();
    }


    void SpawnPlanet()
    {
        StartCoroutine(spawnrandomPlanet());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    //Code responsible for spawning fruits

    public GameObject spawnOrange;

    private GameObject environment;

    void Start()
    {
        environment = this.gameObject;
        SpawnOrange();
    }

    public void SpawnOrange()
    {
        GameObject newSpawnedOrange = Instantiate(spawnOrange);
        newSpawnedOrange.transform.SetParent(environment.transform);
    }
}

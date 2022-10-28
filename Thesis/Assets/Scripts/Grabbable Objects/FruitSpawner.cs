using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    //Code responsible for spawning fruits

    public GameObject spawnOrange;
    public GameObject spawnCoffee;

    private GameObject environment;
    private GameObject coffeTray;

    void Start()
    {
        environment = this.gameObject;
        coffeTray = GameObject.Find("Waitress/NPC_LeftHand/Cafe tray");
        SpawnOrange();
        SpawnCoffee();
    }

    public void SpawnOrange()
    {
        GameObject newSpawnedOrange = Instantiate(spawnOrange);
        newSpawnedOrange.transform.SetParent(environment.transform);
    }

    public void SpawnCoffee()
    {
        GameObject newSpawnedCoffee = Instantiate(spawnCoffee);
        newSpawnedCoffee.transform.SetParent(coffeTray.transform);
    }
}

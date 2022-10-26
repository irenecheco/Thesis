using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFruitSpawner : MonoBehaviour
{
    //COde that activate the fruit spawner

    private GameObject environment;

    void Start()
    {
        environment = GameObject.Find("Environment");
    }

    public void ActivateSpawner()
    {
        environment.GetComponent<FruitSpawner>().SpawnOrange();
        Destroy(this.gameObject);
    }
}

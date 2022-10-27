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
        if(this.gameObject.name == "Orange(Clone)")
        {
            environment.GetComponent<FruitSpawner>().SpawnOrange();
        } else if(this.gameObject.name == "Cafe cup(Clone)")
        {
            environment.GetComponent<FruitSpawner>().SpawnCoffee();
        }        
        Destroy(this.gameObject);
    }
}

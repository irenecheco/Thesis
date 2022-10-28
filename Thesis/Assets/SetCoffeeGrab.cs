using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetCoffeeGrab : MonoBehaviour
{
    private GameObject npcLeftHand;
    private GameObject coffeeTray;
    private GameObject coffeeCup;

    private bool childexists;

    void Start()
    {
        childexists = false;
        npcLeftHand = this.gameObject;
        coffeeTray = npcLeftHand.transform.GetChild(2).gameObject;        
    }

    private void Update()
    {
        if(childexists == false)
        {
            if(coffeeTray.transform.childCount >= 0)
            {
                coffeeCup = coffeeTray.transform.GetChild(0).gameObject;
                childexists = true;
            }
        }
    }

    public void SetCoffeeGrabbable()
    {
        if(coffeeCup!= null)
        {
            coffeeCup.GetComponent<XRGrabInteractable>().enabled = true;
        }
    }
}

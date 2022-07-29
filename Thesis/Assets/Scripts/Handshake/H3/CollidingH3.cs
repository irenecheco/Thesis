using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollidingH3 : MonoBehaviour
{
    public bool isGrabbing;

    private GameObject rightController;
    private GameObject rightHand;

    void Start()
    {
        isGrabbing = false;

        rightHand = this.gameObject;
        rightController = rightHand.transform.parent.gameObject; 
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (isGrabbing == true)
        {
            if(collider.gameObject.name == "Sphere")
            {
                rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (isGrabbing == true)
        {
            if (collider.gameObject.name == "Sphere")
            {
                rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(isGrabbing == true)
        {
            if (collider.gameObject.name == "Sphere")
            {
                rightController.GetComponent<ActionBasedController>().enableInputTracking = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using VRatPolito.PrattiToolkit;

public class ActiveHandController : MonoBehaviour
{
    //Code responsible for handling the controller in use H1

    public ActionBasedController LeftHand, RightHand;
    public bool activeLeft;
    public bool isColliding = false;
    public bool isCollidingWithWaitress = false;

    void Start()
    {
        LeftHand.activateAction.action.performed += ctx => {
            if (isColliding)
            {
                if(RightHand.transform.GetChildRecursive("Handshake UI").gameObject.GetComponent<Canvas>().enabled == true)
                {
                    LeftHand.transform.GetChildRecursive("Handshake UI").gameObject.GetComponent<Canvas>().enabled = true;
                    RightHand.transform.GetChildRecursive("Handshake UI").gameObject.GetComponent<Canvas>().enabled = false;
                } else if(RightHand.transform.GetChildRecursive("Wait For Confirm UI").gameObject.GetComponent<Canvas>().enabled == true)
                {
                    LeftHand.transform.GetChildRecursive("Wait For Confirm UI").gameObject.GetComponent<Canvas>().enabled = true;
                    RightHand.transform.GetChildRecursive("Wait For Confirm UI").gameObject.GetComponent<Canvas>().enabled = false;
                }                
            }
            if (isCollidingWithWaitress)
            {
                if (RightHand.transform.GetChildRecursive("Handshake UI").gameObject.GetComponent<Canvas>().enabled == true)
                {
                    LeftHand.transform.GetChildRecursive("Handshake UI").gameObject.GetComponent<Canvas>().enabled = true;
                    RightHand.transform.GetChildRecursive("Handshake UI").gameObject.GetComponent<Canvas>().enabled = false;
                }
            }
            activeLeft = true;
            LeftHand.GetComponent<XRRayInteractor>().enabled = true;
            RightHand.GetComponent<XRRayInteractor>().enabled = false;
            LeftHand.GetComponent<XRInteractorLineVisual>().enabled = true;
            RightHand.GetComponent<XRInteractorLineVisual>().enabled = false;
            
        };
        RightHand.activateAction.action.performed += ctx => {
            if (isColliding)
            {
                if (LeftHand.transform.GetChildRecursive("Handshake UI").GetComponent<Canvas>().enabled == true)
                {
                    LeftHand.transform.GetChildRecursive("Handshake UI").GetComponent<Canvas>().enabled = false;
                    RightHand.transform.GetChildRecursive("Handshake UI").GetComponent<Canvas>().enabled = true;
                } else if(LeftHand.transform.GetChildRecursive("Wait For Confirm UI").gameObject.GetComponent<Canvas>().enabled == true)
                {
                    LeftHand.transform.GetChildRecursive("Wait For Confirm UI").GetComponent<Canvas>().enabled = false;
                    RightHand.transform.GetChildRecursive("Wait For Confirm UI").GetComponent<Canvas>().enabled = true;
                }
                
            }
            if (isCollidingWithWaitress)
            {
                if (LeftHand.transform.GetChildRecursive("Handshake UI").GetComponent<Canvas>().enabled == true)
                {
                    LeftHand.transform.GetChildRecursive("Handshake UI").GetComponent<Canvas>().enabled = false;
                    RightHand.transform.GetChildRecursive("Handshake UI").GetComponent<Canvas>().enabled = true;
                }
            }
            activeLeft = false;            
            LeftHand.GetComponent<XRRayInteractor>().enabled = false;
            RightHand.GetComponent<XRRayInteractor>().enabled = true;
            LeftHand.GetComponent<XRInteractorLineVisual>().enabled = false;
            RightHand.GetComponent<XRInteractorLineVisual>().enabled = true;
            
        };
    }
}

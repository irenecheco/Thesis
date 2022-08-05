using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class CollidingH3 : MonoBehaviour
{
    public bool isGrabbing;

    private GameObject rightController;
    private GameObject rightHand;

    public GameObject otherNetRightHand;

    void Start()
    {
        isGrabbing = false;

        rightHand = this.gameObject;
        rightController = rightHand.transform.parent.gameObject; 
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Sphere")
            {
                PhotonView colliderPhotonView;
                colliderPhotonView = collider.transform.GetComponentInParent<PhotonView>();
                if (!colliderPhotonView.IsMine)
                {
                    if (otherNetRightHand != null)
                    {
                        otherNetRightHand.transform.GetComponent<XRGrabInteractable>().enabled = true;
                    }
                }
        }
    }

    /*private void OnTriggerStay(Collider collider)
    {
        if (isGrabbing == true)
        {
            if (collider.gameObject.name == "Sphere")
            {
                PhotonView colliderPhotonView;
                colliderPhotonView = collider.transform.GetComponentInParent<PhotonView>();
                if (!colliderPhotonView.IsMine)
                {
                    rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
                }
            }
        }
    }*/

    private void OnTriggerExit(Collider collider)
    {
        if(isGrabbing == true)
        {
            if (collider.gameObject.name == "Sphere")
            {
                PhotonView colliderPhotonView;
                colliderPhotonView = collider.transform.GetComponentInParent<PhotonView>();
                if (!colliderPhotonView.IsMine)
                {
                    if(otherNetRightHand != null)
                    {
                        otherNetRightHand.transform.GetComponent<XRGrabInteractable>().enabled = false;
                    }
                }
            }
        }
    }
}

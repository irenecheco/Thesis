using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class CollidingH4 : MonoBehaviour
{
    //Code responsible for handling the collisions: it activates the grabbable component based on collision

    public bool isGrabbing;

    public GameObject otherNetRightHand;

    void Start()
    {
        isGrabbing = false;
    }

    //Function called on collision entered: when the users' heads collide the other user's hand becomes grabbable
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

    //Function called on trigger exit: when the users' heads exit the collision the other user's hand is no more grabbable
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

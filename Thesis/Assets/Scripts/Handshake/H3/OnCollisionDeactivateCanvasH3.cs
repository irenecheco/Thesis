using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class OnCollisionDeactivateCanvasH3 : MonoBehaviourPunCallbacks
{
    //Code responsible for activating the handshake button when two players collide

    public GameObject otherPlayerHead;
    private GameObject messageCanvas;
    private GameObject rightHandController;

    public GameObject otherRightHand;

    public void Start()
    {
        rightHandController = GameObject.Find("Camera Offset/RightHand Controller");
    }

    //Function called on trigger entered: it activates the handshake button only if the two heads collide
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "DeactivateCollider")
        {
            otherPlayerHead = collider.transform.parent.gameObject;
            PhotonView colliderParentPhotonView;
            colliderParentPhotonView = otherPlayerHead.transform.GetComponent<PhotonView>();
            if (!colliderParentPhotonView.IsMine)
            {
                messageCanvas = otherPlayerHead.transform.GetChild(2).gameObject;
                messageCanvas.GetComponent<Canvas>().enabled = false;
                messageCanvas.GetComponent<AudioSource>().enabled = false;
                rightHandController.transform.FindChildRecursive("RightHand").gameObject.GetComponent<GrabbingH3>().isColliding = false;
            }
            rightHandController.GetComponent<XRDirectInteractor>().allowSelect = false;
            if(otherRightHand!= null)
            {
                otherRightHand.GetComponent<MessageActivationH3>().isGrabbing = false;
                //Debug.Log($"{otherRightHand.GetComponent<MessageActivationH3>().isGrabbing} and other hand is {otherRightHand.gameObject.name}");
            }            
        }
    }
}

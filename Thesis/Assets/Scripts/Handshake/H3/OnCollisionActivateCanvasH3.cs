using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class OnCollisionActivateCanvasH3 : MonoBehaviourPunCallbacks
{
    //Code responsible for activating the handshake button when two players collide

    public GameObject otherPlayerHead;
    private GameObject messageCanvas;
    private GameObject rightHand;
    private GameObject rightHandController;

    public void Start()
    {
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        rightHandController = rightHand.transform.parent.gameObject;
    }

    //Function called on trigger entered: it activates the handshake button only if the two heads collide
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Head")
        {
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                messageCanvas = otherPlayerHead.transform.GetChild(2).gameObject;
                messageCanvas.GetComponent<Canvas>().enabled = true;
                messageCanvas.GetComponent<AudioSource>().enabled = true;
                if(rightHand != null)
                {
                    rightHand.GetComponent<HapticController>().amplitude = 0.2f;
                    rightHand.GetComponent<HapticController>().duration = 0.2f;
                    rightHand.GetComponent<HapticController>().SendHaptics();
                    rightHand.GetComponent<GrabbingH3>().isColliding = true;
                }                
                messageCanvas.GetComponent<AudioSource>().Play();
            }
            if(rightHandController!= null)
            {
                rightHandController.GetComponent<XRDirectInteractor>().allowSelect = true;
            }            
        }               
    }
}

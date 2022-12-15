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
    private GameObject otherPlayerRightHand;
    [SerializeField] private GameObject myNetRightHand;

    public bool firstEntered;

    public void Start()
    {   
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        rightHandController = rightHand.transform.parent.gameObject;
        firstEntered = true;
    }

    //Function called on trigger entered: it activates the handshake button only if the two heads collide
    private void OnTriggerEnter(Collider collider)
    {
        if (this.transform.parent.GetComponent<PhotonView>().IsMine)
        {
            if (collider.gameObject.name == "Head")
            {
                PhotonView colliderPhotonView;
                colliderPhotonView = collider.transform.GetComponent<PhotonView>();
                if (!colliderPhotonView.IsMine)
                {
                    if (firstEntered)
                    {
                        //Debug.Log("enter collision");
                        otherPlayerHead = collider.gameObject;
                        otherPlayerRightHand = otherPlayerHead.transform.parent.transform.FindChildRecursive("RightHand").gameObject;
                        rightHand.GetComponent<GrabbingH3>().otherNetRightHand = otherPlayerRightHand;
                        this.transform.parent.transform.FindChildRecursive("DeactivateCollider").gameObject.GetComponent<OnCollisionDeactivateCanvasH3>().otherRightHand = otherPlayerRightHand;
                        messageCanvas = otherPlayerHead.transform.GetChild(2).gameObject;
                        messageCanvas.GetComponent<Canvas>().enabled = true;
                        messageCanvas.GetComponent<AudioSource>().enabled = true;
                        if (rightHand != null)
                        {
                            rightHand.GetComponent<HapticController>().amplitude = 0.2f;
                            rightHand.GetComponent<HapticController>().duration = 0.2f;
                            rightHand.GetComponent<HapticController>().SendHaptics();
                            rightHand.GetComponent<GrabbingH3>().isColliding = true;
                        }
                        messageCanvas.GetComponent<AudioSource>().Play();
                        if (rightHandController != null)
                        {
                            rightHandController.GetComponent<XRDirectInteractor>().allowSelect = true;
                        }
                        firstEntered = false;
                        this.transform.parent.transform.FindChildRecursive("DeactivateCollider").gameObject.GetComponent<OnCollisionDeactivateCanvasH3>().firstExited = true;
                    }                   
                }
            }
        }                       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.SceneManagement;

public class OnCollisionDeactivateCanvasH1 : MonoBehaviour
{
    //Code responsible for deactivating the handshake button when two players exit collision

    private GameObject handshakeUI_l;
    private GameObject handshakeUI_r;
    private GameObject waitingUI_l;
    private GameObject waitingUI_r;
    private GameObject leftHand;
    private GameObject rightHand;

    private XROrigin origin;

    private PhotonView colliderPhotonView;

    public bool firstExited;
    //private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    void Start()
    {
        handshakeUI_l = GameObject.Find("Camera Offset/LeftHand Controller/LeftHand/Handshake UI");
        handshakeUI_r = GameObject.Find("Camera Offset/RightHand Controller/RightHand/Handshake UI");
        handshakeUI_l.GetComponent<Canvas>().enabled = false;
        handshakeUI_r.GetComponent<Canvas>().enabled = false;
        leftHand = handshakeUI_l.transform.parent.gameObject;
        rightHand = handshakeUI_r.transform.parent.gameObject;
        waitingUI_l = leftHand.transform.GetChild(3).gameObject;
        waitingUI_r = rightHand.transform.GetChild(3).gameObject;        

        origin = FindObjectOfType<XROrigin>();

        firstExited = true;
    }

    //Function called on trigger exited: it disables the handshake button if the two heads does not collide anymore
    private void OnTriggerExit(Collider collider)
    {
        if (this.transform.parent.GetComponent<PhotonView>().IsMine)
        {
            if (collider.gameObject.name == "Head")
            {
                colliderPhotonView = collider.transform.GetComponent<PhotonView>();
                if (!colliderPhotonView.IsMine)
                {
                    if (firstExited)
                    {
                        origin.GetComponent<ActiveHandController>().isColliding = false;
                        if (handshakeUI_l.GetComponent<Canvas>().enabled == true)
                        {
                            handshakeUI_l.GetComponent<Canvas>().enabled = false;
                        }
                        if (handshakeUI_r.GetComponent<Canvas>().enabled == true)
                        {
                            handshakeUI_r.GetComponent<Canvas>().enabled = false;
                        }
                        if (waitingUI_l.GetComponent<Canvas>().enabled == true)
                        {
                            waitingUI_l.GetComponent<Canvas>().enabled = false;
                        }
                        if (waitingUI_r.GetComponent<Canvas>().enabled == true)
                        {
                            waitingUI_r.GetComponent<Canvas>().enabled = false;
                        }
                        GameObject myConfirmCanvas = this.transform.parent.GetChild(0).gameObject;
                        GameObject confirmCanvas = collider.transform.GetChild(0).gameObject;
                        confirmCanvas.GetComponent<HandshakeConfirmCanvas>().DeactivateHandshakeConfirmCanvas();
                        myConfirmCanvas.GetComponent<HandshakeConfirmCanvas>().DeactivateHandshakeConfirmCanvas();
                        firstExited = false;
                        this.transform.parent.transform.FindChildRecursive("ActivateCollider").gameObject.GetComponent<OnCollisionActivateButtonH1>().firstEntered = true;
                    }
                }
            }
        }
    }
}

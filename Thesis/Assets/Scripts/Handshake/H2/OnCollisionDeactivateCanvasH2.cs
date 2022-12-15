using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.SceneManagement;

public class OnCollisionDeactivateCanvasH2 : MonoBehaviour
{
    //Code responsible for deactivating the handshake button when two players exit collision

    private GameObject otherPlayerHead;
    private GameObject otherPlayer;
    private GameObject otherRightHand;
    private GameObject otherHandMesh;

    private GameObject thisHead;
    private PhotonView colliderPhotonView;
    private GameObject rightHand;
    private GameObject handMesh;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_waitingCanva = "Handshake 2 waiting";
    private string handshake2_confirmCanva = "Handshake 2 confirm";

    public bool firstExited;

    void Start()
    {
        thisHead = this.transform.parent.gameObject;
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        handMesh = rightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
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
                        //Debug.Log("esce da collisione con altro Deactivate collider");
                        otherPlayerHead = collider.gameObject.transform.gameObject;
                        otherPlayer = otherPlayerHead.transform.parent.gameObject;
                        otherRightHand = otherPlayer.transform.FindChildRecursive("Right Hand").gameObject;
                        otherHandMesh = otherRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;

                        this.transform.parent.gameObject.GetComponent<OnButtonAPressed>().isColliding = false;
                        GameObject confirmCanvas = otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject;

                        handMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                        rightHand.transform.GetChild(2).gameObject.GetComponent<Canvas>().enabled = false;
                        otherHandMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;

                        confirmCanvas.transform.GetComponent<Canvas>().enabled = false;
                        confirmCanvas.transform.GetComponent<AudioSource>().enabled = false;
                        otherPlayerHead.transform.FindChildRecursive(handshake2_messageCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;

                        firstExited = false;
                        this.transform.parent.transform.FindChildRecursive("ActivateCollider").gameObject.GetComponent<OnCollisionActivateCanvasH2>().firstEntered = true;
                    }
                }
            }
        }
    }
}

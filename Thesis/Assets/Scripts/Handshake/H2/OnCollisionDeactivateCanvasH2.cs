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

    private GameObject thisHead;
    private PhotonView colliderParentPhotonView;
    private GameObject colliderParent;
    private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_waitingCanva = "Handshake 2 waiting";
    private string handshake2_confirmCanva = "Handshake 2 confirm";

    private bool h2_messageActive;

    public bool buttonAPressed = false;

    void Start()
    {
        thisHead = this.transform.parent.gameObject;
        thisHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<HandshakeWaitingCanvasH2>().DeactivateHandshakeWaitingCanvas();
        thisHead.transform.Find(handshake2_confirmCanva).gameObject.transform.GetComponent<HandshakeConfirmCanvasH2>().DeactivateHandshakeConfirmCanvas();
        h2_messageActive = false;
    }

    //Function called on trigger exited: it disables the handshake button if the two heads does not collide anymore
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "DeactivateCollider")
        {
            colliderParent = collider.transform.parent.gameObject;
            colliderParentPhotonView = colliderParent.transform.GetComponent<PhotonView>();
            if (!colliderParentPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject.transform.parent.gameObject;

                otherPlayerHead.transform.FindChildRecursive(handshake2_waitingCanva).gameObject.transform.GetComponent<HandshakeWaitingCanvasH2>().DeactivateHandshakeWaitingCanvas();
                otherPlayerHead.transform.FindChildRecursive(handshake2_confirmCanva).gameObject.transform.GetComponent<HandshakeConfirmCanvasH2>().DeactivateHandshakeConfirmCanvas();
                otherPlayerHead.transform.FindChildRecursive(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeMessageCanvas();
                buttonAPressed = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class OnCollisionActivateCanvasH2 : MonoBehaviourPunCallbacks, IPunObservable
{
    //Code responsible for activating the handshake message when two players collide

    public GameObject otherPlayerHead;
  
    private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_waitingCanva = "Handshake 2 waiting";
    private string handshake2_confirmCanva = "Handshake 2 confirm";

    private bool h2_messageActive;

    public bool buttonAPressed = false;

    //Method of the Photon Pun library that lets you keep track of a variable through the network (class IPunObservable)
    //In this case it keeps track of a bool that is true when the message canvas is active
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*if (stream.IsWriting)
        {
            stream.SendNext(h2_messageActive);
        }
        else
        {
            this.h2_messageActive = (bool)stream.ReceiveNext();
        }*/
    }

    void Start()
    {
        this.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<HandshakeWaitingCanvasH2>().DeactivateHandshakeConfirmCanvas();
        this.transform.Find(handshake2_confirmCanva).gameObject.transform.GetComponent<HandshakeConfirmCanvasH2>().DeactivateHandshakeConfirmCanvas();
        h2_messageActive = false;
    }

    void Update()
    {
        /*if (h2_messageActive == false)
        {                
            this.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeConfirmCanvas();
            this.gameObject.transform.GetComponent<OnButtonAPressed>().isColliding = false;
        }*/

        /*if (buttonAPressed == true)
        {
            this.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeConfirmCanvas();
        }*/
    }

    //Function called on trigger entered: it activates the message canvas on the other player head only if the two heads collide
    //and button A is not pressed
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Head")
        {
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;

                if (buttonAPressed == false)
                {
                    //When two players' heads are colliding it saves the other player
                    this.gameObject.transform.GetComponent<OnButtonAPressed>().otherPlayerHead = otherPlayerHead;
                    this.gameObject.transform.GetComponent<OnButtonAPressed>().isColliding = true;
                    otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().ActivateHandshakeConfirmCanvas();
                }
            }
        }
    }

    //Function called on trigger stay: it activates the message canvas on the other player head only if the two heads collide and button A is not pressed
   /* private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.name == "Head")
        {
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                
                if (buttonAPressed == false)
                {
                    h2_messageActive = true;
                } else
                {
                    h2_messageActive = false;
                }
            }
        }
    }*/

    //Function called on trigger exit: it disable the message canvas if the two heads are not colliding
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Head")
        {
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                
                //h2_messageActive = false;
                otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<HandshakeWaitingCanvasH2>().DeactivateHandshakeConfirmCanvas();
                otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject.transform.GetComponent<HandshakeConfirmCanvasH2>().DeactivateHandshakeConfirmCanvas();
                otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeConfirmCanvas();
                buttonAPressed = false;
            }
        }
    }
}

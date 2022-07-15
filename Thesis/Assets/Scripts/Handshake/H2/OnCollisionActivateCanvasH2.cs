using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class OnCollisionActivateCanvasH2 : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject otherPlayerHead;
  
    private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_waitingCanva = "Handshake 2 waiting";
    private string handshake2_confirmCanva = "Handshake 2 confirm";
    //private PhotonView photonViewParent;

    private int sceneIndex;

    private bool h2_messageActive;

    public bool buttonAPressed = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(h2_messageActive);
        }
        else
        {
            this.h2_messageActive = (bool)stream.ReceiveNext();
            //Debug.Log($"{this.h2_messageActive}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        this.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<HandshakeWaitingCanvasH2>().DeactivateHandshakeConfirmCanvas();
        this.transform.Find(handshake2_confirmCanva).gameObject.transform.GetComponent<HandshakeConfirmCanvasH2>().DeactivateHandshakeConfirmCanvas();
        h2_messageActive = false;
        //photonViewParent = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (h2_messageActive == false)
        {
            //Debug.Log($"messaggio � {h2_messageActive}");                
            this.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeConfirmCanvas();
            this.gameObject.transform.GetComponent<OnButtonAPressed>().isColliding = false;
        }

        if (buttonAPressed == true)
        {
            this.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeConfirmCanvas();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Head")
        {
            //Debug.Log($"{collider.transform.GetComponent<PhotonView>()}");
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                
                if (buttonAPressed == false)
                {
                    h2_messageActive = true;
                    this.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().ActivateHandshakeConfirmCanvas();
                    this.gameObject.transform.GetComponent<OnButtonAPressed>().otherPlayerHead = otherPlayerHead;
                    this.gameObject.transform.GetComponent<OnButtonAPressed>().isColliding = true;
                    otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().ActivateHandshakeConfirmCanvas();
                    //otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<OnButtonAPressed>().isColliding = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider collider)
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
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Head")
        {
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                
                h2_messageActive = false;
                otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<HandshakeWaitingCanvasH2>().DeactivateHandshakeConfirmCanvas();
                otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject.transform.GetComponent<HandshakeConfirmCanvasH2>().DeactivateHandshakeConfirmCanvas();
                otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeConfirmCanvas();
                buttonAPressed = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeMessageCanvasH2 : MonoBehaviour, IPunObservable
{
    //Code responsible for the message canvas (if it has to be shown or not) through the network

    private bool messageActive;
    private GameObject handshake2MessageCanvas;

    //Method of the Photon Pun library that lets you keep track of a variable through the network (class IPunObservable)
    //In this case it keeps track of a bool that is true when the message canvas needs to be active
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(messageActive);
        }
        else
        {
            this.messageActive = (bool)stream.ReceiveNext();
        }
    }

    void Start()
    {
        handshake2MessageCanvas = this.gameObject;
        //handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled = false;
    }

    //Called when the message canvas needs to be active
    public void ActivateHandshakeConfirmCanvas()
    {
        messageActive = true;
    }

    //Called when the message canvas needs to be disabled
    public void DeactivateHandshakeConfirmCanvas()
    {
        messageActive = false;
    }

    void Update()
    {
        //Check if the bool is true to enable or disable the canvas
        if (messageActive == false)
        {
            handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled = false;
        }
        else
        {
            handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled = true;
        }
    }
}

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
    private bool previousFrame;

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
        previousFrame = false;
    }

    //Called when the message canvas needs to be active
    public void ActivateHandshakeMessageCanvas()
    {
        messageActive = true;
        if(handshake2MessageCanvas != null)
        {
            handshake2MessageCanvas.GetComponent<Canvas>().enabled = true;
            handshake2MessageCanvas.GetComponent<AudioSource>().enabled = true;
            handshake2MessageCanvas.GetComponent<AudioSource>().Play();
        }        
    }

    //Called when the message canvas needs to be disabled
    public void DeactivateHandshakeMessageCanvas()
    {
        messageActive = false;
        if(handshake2MessageCanvas != null)
        {
            handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled = false;
        }        
    }

    /*void Update()
    {
        //Check if the bool is true to enable or disable the canvas
        if (messageActive == false)
        {
            if(previousFrame == true)
            {
                if(handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled == true)
                {
                    handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled = false;
                }                
                previousFrame = false;
            }            
        }
        else
        {
            if(previousFrame == false)
            {
                if (handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled == false)
                {
                    handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled = true;
                    handshake2MessageCanvas.GetComponent<AudioSource>().enabled = true;
                    handshake2MessageCanvas.GetComponent<AudioSource>().Play();
                }                
                previousFrame = true;
            }            
        }
    }*/
}

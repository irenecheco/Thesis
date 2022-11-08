using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeConfirmCanvasH2 : MonoBehaviour,  IPunObservable
{
    //Code responsible for the confirm canvas (if it has to be shown or not) through the network

    private bool confirmActive;
    private GameObject handshake2ConfirmCanvas;
    private bool previousFrame;

    //Method of the Photon Pun library that lets you keep track of a variable through the network (class IPunObservable)
    //In this case it keeps track of a bool that is true when the confirm canvas needs to be active
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(confirmActive);
        }
        else
        {
            this.confirmActive = (bool)stream.ReceiveNext();
        }
    }

    void Start()
    {
        handshake2ConfirmCanvas = this.gameObject;
        previousFrame = false;
        //handshake2ConfirmCanvas.transform.GetComponent<Canvas>().enabled = false;
    }

    //Called when the confirm canvas needs to be active
    public void ActivateHandshakeConfirmCanvas()
    {
        confirmActive = true;
    }

    //Called when the confirm canvas needs to be disabled
    public void DeactivateHandshakeConfirmCanvas()
    {
        confirmActive = false;
    }

    void Update()
    {
        //Check if the bool is true to enable or disable the canvas
        if (confirmActive == false)
        {
            if(previousFrame == true)
            {
                if(handshake2ConfirmCanvas.transform.GetComponent<Canvas>().enabled == true)
                {
                    handshake2ConfirmCanvas.transform.GetComponent<Canvas>().enabled = false;
                }                
                previousFrame = false;
            }            
        }
        else
        {
            if(previousFrame == false)
            {
                if (handshake2ConfirmCanvas.transform.GetComponent<Canvas>().enabled == false)
                {
                    handshake2ConfirmCanvas.transform.GetComponent<Canvas>().enabled = true;
                    handshake2ConfirmCanvas.GetComponent<AudioSource>().enabled = true;
                    handshake2ConfirmCanvas.GetComponent<AudioSource>().Play();
                }                
                previousFrame = true;
            }            
        }
    }
}

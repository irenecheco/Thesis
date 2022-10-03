using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeConfirmCanvas : MonoBehaviour, IPunObservable
{
    //Code responsible for the confirm canvas (if it has to be shown or not) through the network

    private GameObject handshakeConfirm;
    private GameObject handshakeConfirmButton;
    private bool confirmActive;

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
        handshakeConfirm = this.gameObject;
        handshakeConfirm.transform.GetComponent<Canvas>().enabled = false;
        handshakeConfirmButton = handshakeConfirm.transform.GetChild(2).gameObject;
        confirmActive = false;
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
            handshakeConfirm.transform.GetComponent<Canvas>().enabled = false;
            handshakeConfirmButton.GetComponent<Button>().interactable = false;
        }
        else
        {
             handshakeConfirm.transform.GetComponent<Canvas>().enabled = true;
             handshakeConfirmButton.GetComponent<Button>().interactable = true;
        }
    }
}

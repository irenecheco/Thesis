using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeMessageCanvasH2 : MonoBehaviour, IPunObservable
{
    //Code responsible for the message canvas (if it has to be shown or not) through the network

    private bool messageActive;

    private PhotonView parentPhotonView;

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
        parentPhotonView = this.transform.parent.GetComponent<PhotonView>();
    }

    //Called when the message canvas needs to be active
    public void ActivateHandshakeMessageCanvas()
    {
        messageActive = true;
        //Debug.Log($"chiamato activate, messageActive è {messageActive}");
    }

    //Called when the message canvas needs to be disabled
    public void DeactivateHandshakeMessageCanvas()
    {
        messageActive = false;
        //Debug.Log($"chiamato deactivate, messageActive è {messageActive}");
    }
}

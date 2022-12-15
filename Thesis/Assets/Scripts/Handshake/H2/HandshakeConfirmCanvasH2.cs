using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeConfirmCanvasH2 : MonoBehaviour,  IPunObservable
{
    //Code responsible for the confirm canvas (if it has to be shown or not) through the network

    private bool confirmActive;

    private PhotonView parentPhotonView;

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
        parentPhotonView = this.transform.parent.GetComponent<PhotonView>();
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
}

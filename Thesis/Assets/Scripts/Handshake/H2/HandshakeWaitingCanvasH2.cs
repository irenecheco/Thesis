using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeWaitingCanvasH2 : MonoBehaviour, IPunObservable
{
    private bool waitingActive;
    private GameObject handshake2WaitingCanvas;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(waitingActive);
        }
        else
        {
            this.waitingActive = (bool)stream.ReceiveNext();
            //Debug.Log($"{this.confirmActive}");
        }
    }

    void Start()
    {
        handshake2WaitingCanvas = this.gameObject;
        handshake2WaitingCanvas.transform.GetComponent<Canvas>().enabled = false;
    }

    public void ActivateHandshakeConfirmCanvas()
    {
        //Debug.Log("ActivateHandshakeConfirm entered");
        waitingActive = true;
    }

    public void DeactivateHandshakeConfirmCanvas()
    {
        //Debug.Log("Entered DeactivateHandshakeConfirmCanvas");
        waitingActive = false;
    }

    void Update()
    {
        if (waitingActive == false)
        {
            handshake2WaitingCanvas.transform.GetComponent<Canvas>().enabled = false;
        }
        else
        {
            handshake2WaitingCanvas.transform.GetComponent<Canvas>().enabled = true;
        }
    }
}

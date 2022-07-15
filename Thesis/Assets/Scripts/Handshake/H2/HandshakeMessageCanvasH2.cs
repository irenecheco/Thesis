using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeMessageCanvasH2 : MonoBehaviour, IPunObservable
{
    private bool messageActive;
    private GameObject handshake2MessageCanvas;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(messageActive);
        }
        else
        {
            this.messageActive = (bool)stream.ReceiveNext();
            //Debug.Log($"{this.confirmActive}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        handshake2MessageCanvas = this.gameObject;
        handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled = false;
    }

    public void ActivateHandshakeConfirmCanvas()
    {
        //Debug.Log("ActivateHandshakeConfirm entered");
        messageActive = true;
    }

    public void DeactivateHandshakeConfirmCanvas()
    {
        //Debug.Log("Entered DeactivateHandshakeConfirmCanvas");
        messageActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (messageActive == false)
        {
            handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled = false;
        }
        else
        {
            //Debug.Log("Should activate Canvas");
            handshake2MessageCanvas.transform.GetComponent<Canvas>().enabled = true;
            //Debug.Log($"{handshakeConfirm.transform.GetComponent<Canvas>().enabled}");
        }
    }
}

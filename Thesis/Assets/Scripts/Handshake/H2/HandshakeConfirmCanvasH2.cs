using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeConfirmCanvasH2 : MonoBehaviour,  IPunObservable
{
    private bool confirmActive;
    private GameObject handshake2ConfirmCanvas;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(confirmActive);
        }
        else
        {
            this.confirmActive = (bool)stream.ReceiveNext();
            //Debug.Log($"{this.confirmActive}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        handshake2ConfirmCanvas = this.gameObject;
        handshake2ConfirmCanvas.transform.GetComponent<Canvas>().enabled = false;
    }

    public void ActivateHandshakeConfirmCanvas()
    {
        //Debug.Log("ActivateHandshakeConfirm entered");
        confirmActive = true;
    }

    public void DeactivateHandshakeConfirmCanvas()
    {
        //Debug.Log("Entered DeactivateHandshakeConfirmCanvas");
        confirmActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (confirmActive == false)
        {
            handshake2ConfirmCanvas.transform.GetComponent<Canvas>().enabled = false;
        }
        else
        {
            //Debug.Log("Should activate Canvas");
            handshake2ConfirmCanvas.transform.GetComponent<Canvas>().enabled = true;
            //Debug.Log($"{handshakeConfirm.transform.GetComponent<Canvas>().enabled}");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeConfirm : MonoBehaviour, IPunObservable
{
    private GameObject handshakeConfirm;
    private GameObject handshakeConfirmButton;
    private bool confirmActive;

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
        handshakeConfirm = this.gameObject;
        handshakeConfirm.transform.GetComponent<Canvas>().enabled = false;
        handshakeConfirmButton = handshakeConfirm.transform.GetChild(2).gameObject;
        confirmActive = false;

    }

    public void ActivateHandshakeConfirmCanvas()
    {
        Debug.Log("ActivateHandshakeConfirm entered");
        confirmActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (confirmActive == false)
        {
            handshakeConfirm.transform.GetComponent<Canvas>().enabled = false;
        } else
        {
            handshakeConfirm.transform.GetComponent<Canvas>().enabled = true;
            handshakeConfirmButton.GetComponent<Button>().interactable = true;
        }
    }
}

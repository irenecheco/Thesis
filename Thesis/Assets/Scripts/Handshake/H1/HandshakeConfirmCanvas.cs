using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeConfirmCanvas : MonoBehaviour, IPunObservable
{
    //Code responsible for the confirm canvas (if it has to be shown or not) through the network

    private GameObject handshakeConfirm;
    private GameObject player_head;
    private GameObject player;
    private GameObject player_rightController;
    private GameObject player_rightMesh;
    public bool confirmActive;
    private bool firstSound;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color waitingColor = new Color(0.4135279f, 0.7409829f, 0.9056604f, 1.0f);

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
        player_head = handshakeConfirm.transform.parent.gameObject;
        player = player_head.transform.parent.gameObject;
        player_rightController = player.transform.FindChildRecursive("Right Hand").gameObject;
        player_rightMesh = player_rightController.transform.FindChildRecursive("hands:Lhand").gameObject;
        handshakeConfirm.transform.GetComponent<Canvas>().enabled = false;
        confirmActive = false;
        firstSound = true;
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
            player_rightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
            firstSound = true;
        }
        else
        {
            handshakeConfirm.transform.GetComponent<Canvas>().enabled = true;
            player_rightMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
            if (firstSound == true)
            {
                handshakeConfirm.GetComponent<AudioSource>().Play();
                firstSound = false;
            }
        }
    }
}

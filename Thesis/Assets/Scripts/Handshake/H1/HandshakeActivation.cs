using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandshakeActivation : MonoBehaviour
{
    //Code responsible for handshake activation in H1

    private GameObject confirmCanvas;
    private GameObject confirmHead;
    private GameObject confirmPlayer;
    private string player1ID;
    private string player2ID;

    void Start()
    {
        confirmCanvas = this.gameObject.transform.parent.gameObject;
        confirmHead = confirmCanvas.transform.parent.gameObject;
        confirmPlayer = confirmHead.transform.parent.gameObject;
    }

    //Function called when confirm button pressed: it saves the ids and call the methed to activate the animation over the network
    public void CallHeadMethod()
    {
        foreach(var item in PhotonNetwork.PlayerList)
        {
            if((object)item.TagObject == confirmPlayer)
            {
                //Debug.Log($"{item.UserId}");
                player1ID = item.UserId;
            }
        }

        player2ID = PhotonNetwork.LocalPlayer.UserId;

        confirmPlayer.GetComponent<NetworkHandshakeActivationH1>().CallActivationOverNetwork(player1ID, player2ID);
    }
}

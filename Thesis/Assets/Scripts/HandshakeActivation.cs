using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandshakeActivation : MonoBehaviour
{
    private GameObject confirmCanvas;
    private GameObject confirmHead;
    private GameObject confirmPlayer;
    private string player1ID;
    private string player2ID;

    // Start is called before the first frame update
    void Start()
    {
        confirmCanvas = this.gameObject.transform.parent.gameObject;
        confirmHead = confirmCanvas.transform.parent.gameObject;
        confirmPlayer = confirmHead.transform.parent.gameObject;

    }

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

        confirmPlayer.GetComponent<NetworkHandshakeActivation>().CallActivationOverNetwork(player1ID, player2ID);

    }
}

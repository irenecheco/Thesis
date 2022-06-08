using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReturnToLobby : MonoBehaviour
{
    private GameObject networkVoice;

    private void Start()
    {
        networkVoice = GameObject.Find("Network Voice"); ;
    }
    public void OnClick_ReturnToLobby()
    {
        Destroy(networkVoice);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
}

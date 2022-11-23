using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //Code responsible for the setup of the Network Manager, it lets you access the tutorials
    
    public GameObject roomUI;
    public GameObject startButton;
    public GameObject tryingToConnectCanva;

    //Functions that are called every time a new user opens the platform and click on "Start tutorial" button: they connect him to the server and start the tutorial.
    public void ConnectToServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Try Connect To Server...");
            startButton.SetActive(false);
            tryingToConnectCanva.SetActive(true);
        } else
        {
            startButton.SetActive(false);
            roomUI.SetActive(true);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Lobby joined");
        startButton.SetActive(false);
        tryingToConnectCanva.SetActive(false);
        roomUI.SetActive(true);
    }

    //Function called when user starts the tutorial and choose H1: it loads the scene for the H1 tutorial
    public void StartTutorialH1()
    {
        Application.LoadLevel("Tutorial H1");
    }

    //Function called when user starts the tutorial and choose H2: it loads the scene for the H2 tutorial
    public void StartTutorialH2()
    {
        Application.LoadLevel("Tutorial H2");
    }

    //Function called when user starts the tutorial and choose H3: it loads the scene for the H3 tutorial
    public void StartTutorialH3()
    {
        Application.LoadLevel("Tutorial H3");
    }

    public void StartTutorialH4()
    {
        Application.LoadLevel("Tutorial H4");
    }
}

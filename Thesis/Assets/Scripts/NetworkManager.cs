using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//Class with room information: useful to add new rooms and change their characteristics directly from Unity
[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //Code responsible for the setup of the Network Manager, so that multiple users can conncet and switch between rooms

    public List<DefaultRoom> defaultRooms;
    public GameObject roomUI;
    public GameObject connectButton;

    //Functions that are called every time a new user opens the platform and click on connect button: they connect him to the server.
    public void ConnectToServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Try Connect To Server...");
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
        connectButton.SetActive(false);
        roomUI.SetActive(true);
    }

    //Function called every time a user select a room: it loads the scene and connect the player to the room (with the other players)
    public void InitializeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //Load scene
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);

        //Create Room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.PublishUserId = true;
        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
    }

    //Functions called when player joins a room: mainly for debug
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined A Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }

}

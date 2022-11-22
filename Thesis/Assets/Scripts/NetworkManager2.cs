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

public class NetworkManager2 : MonoBehaviourPunCallbacks
{
    //Code responsible for the setup of the Network Manager, so that multiple users can conncet and switch between rooms

    public List<DefaultRoom> defaultRooms;

    public int countPl = 0;
    private GameObject localPlayer;

    //Function called when user wants to get back to handshake selection
    public void BackToTutorialMainRoom()
    {
        PhotonNetwork.LoadLevel(0);
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
        /*countPl = PhotonNetwork.CountOfPlayers;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                localPlayer = (GameObject)item.TagObject;
            }
        }
        if (countPl <= 1)
        {
            localPlayer.GetComponent<firstPlayer>().isFirstPlayer = true;
        }
        else
        {
            localPlayer.GetComponent<firstPlayer>().isFirstPlayer = false;
        }*/
    }
}

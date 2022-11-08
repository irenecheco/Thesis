using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHandshakePressedA : MonoBehaviour
{
    //Code responsible for keeping track of the A button pressure: it changes the canvas according to the button pressure

    private string[] playersID = new string[2];
    private GameObject otherPlayer;
    private GameObject myHead;
    private GameObject otherPlayerHead;
    private GameObject myPlayer;
    private GameObject myRightController;
    private GameObject myRightHand;
    private GameObject otherPlayerRightController;
    private GameObject otherPlayerRightHand;
    private GameObject rightHand;

    private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_waitingCanva = "Handshake 2 waiting";
    private string handshake2_confirmCanva = "Handshake 2 confirm";

    private PhotonView photonView;

    void Start()
    {
        photonView = this.transform.GetComponent<PhotonView>();
        myHead = this.gameObject;
        myPlayer = this.gameObject.transform.parent.gameObject;
        myRightController = myPlayer.gameObject.transform.GetChild(2).gameObject;
        myRightHand = myRightController.gameObject.transform.GetChild(0).gameObject;
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
    }

    //Functions called when a user is pressing the A button
    public void CallPressedAOverNetwork(string pl1ID, string pl2ID)
    {
        playersID[0] = pl1ID;
        playersID[1] = pl2ID;

        //Photon Pun method that calls a function over the network
        photonView.RPC("PressedAOverNetwork", RpcTarget.All, playersID as object[]);
    }

    //Functions called when a user is releasing the A button
    public void CallReleasedAOverNetwork(string pl1ID, string pl2ID)
    {
        playersID[0] = pl1ID;
        playersID[1] = pl2ID;

        //Photon Pun method that calls a function over the network
        photonView.RPC("ReleasedAOverNetwork", RpcTarget.All, playersID as object[]);
    }

    //Function called over the network (on every network player): it checks if the player is involved in the handshake and,
    //if it is, it changes the canvas accordingly
    [PunRPC]
    public void PressedAOverNetwork(object[] ids)
    {
        string[] playersIds = new string[2];
        playersIds[0] = (string)ids[0];
        playersIds[1] = (string)ids[1];

        if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
        {
            //My user id is in position 0: I am pressing A button

            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[1])
                {
                    otherPlayer = (GameObject)item.TagObject;
                    otherPlayerHead = otherPlayer.transform.GetChild(0).gameObject;
                    otherPlayerRightController = otherPlayer.gameObject.transform.GetChild(2).gameObject;
                    otherPlayerRightHand = otherPlayerRightController.gameObject.transform.GetChild(0).gameObject;
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                if(myHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed == true && otherPlayerHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed == false)
                {
                    otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<HandshakeWaitingCanvasH2>().ActivateHandshakeWaitingCanvas();
                    otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeMessageCanvas();

                    myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = true;
                }             
            }
        }
        else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
        {
            //My user id is in position 1: the other player is pressing A button

            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[0])
                {
                    otherPlayer = (GameObject)item.TagObject;
                    otherPlayerHead = otherPlayer.transform.GetChild(0).gameObject;
                    otherPlayerRightController = otherPlayer.gameObject.transform.GetChild(2).gameObject;
                    if(otherPlayerRightController.transform.childCount >= 1)
                    {
                        otherPlayerRightHand = otherPlayerRightController.gameObject.transform.GetChild(0).gameObject;
                    }
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null && myHead.GetComponent<OnButtonAPressed>().isPressed == false)
            {
                otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeMessageCanvas();
                otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject.transform.GetComponent<HandshakeConfirmCanvasH2>().ActivateHandshakeConfirmCanvas();

                //myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = true;
            }
        }
    }

    //Function called over the network (on every network player): it checks if the player is involved in the handshake and,
    //if it is, it changes the canvas accordingly
    [PunRPC]
    public void ReleasedAOverNetwork(object[] ids)
    {
        string[] playersIds = new string[2];
        playersIds[0] = (string)ids[0];
        playersIds[1] = (string)ids[1];
        if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
        {
            //My user id is in position 0: I am releasing A button
            myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = false;

            otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().ActivateHandshakeMessageCanvas();
            otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<HandshakeWaitingCanvasH2>().DeactivateHandshakeWaitingCanvas();
        }
        else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
        {
            //My user id is in position 1: the other player is releasing A button
            //myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = false;

            otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().ActivateHandshakeMessageCanvas();
            otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject.transform.GetComponent<HandshakeConfirmCanvasH2>().DeactivateHandshakeConfirmCanvas();
        }
    }
}

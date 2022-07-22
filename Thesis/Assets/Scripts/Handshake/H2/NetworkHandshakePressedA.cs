using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHandshakePressedA : MonoBehaviour
{
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

    public void CallPressedAOverNetwork(string pl1ID, string pl2ID)
    {
        playersID[0] = pl1ID;
        playersID[1] = pl2ID;
        //Debug.Log($"id 1 è {playersID[0]}, id 2 è {playersID[1]}");
        photonView.RPC("PressedAOverNetwork", RpcTarget.All, playersID as object[]);
    }

    public void CallReleasedAOverNetwork(string pl1ID, string pl2ID)
    {
        playersID[0] = pl1ID;
        playersID[1] = pl2ID;
        //Debug.Log($"id 1 è {playersID[0]}, id 2 è {playersID[1]}");
        photonView.RPC("ReleasedAOverNetwork", RpcTarget.All, playersID as object[]);
    }

    [PunRPC]
    public void PressedAOverNetwork(object[] ids)
    {
        string[] playersIds = new string[2];
        playersIds[0] = (string)ids[0];
        playersIds[1] = (string)ids[1];

        if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
        {
            //Debug.Log($"quindi mio id è al posto 0");

            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[1])
                {
                    otherPlayer = (GameObject)item.TagObject;
                    otherPlayerHead = otherPlayer.transform.GetChild(0).gameObject;
                    otherPlayerRightController = otherPlayer.gameObject.transform.GetChild(2).gameObject;
                    otherPlayerRightHand = otherPlayerRightController.gameObject.transform.GetChild(0).gameObject;

                    //Debug.Log($"Other player name is {otherPlayer.name}");
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                if(myHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed == true && otherPlayerHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed == false)
                {
                    otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<HandshakeWaitingCanvasH2>().ActivateHandshakeConfirmCanvas();
                    otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeConfirmCanvas();

                    myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = true;
                }             
            }
        }
        else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
        {
            //Debug.Log($"quindi mio id è al posto 1");

            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[0])
                {
                    otherPlayer = (GameObject)item.TagObject;
                    otherPlayerHead = otherPlayer.transform.GetChild(0).gameObject;
                    otherPlayerRightController = otherPlayer.gameObject.transform.GetChild(2).gameObject;
                    otherPlayerRightHand = otherPlayerRightController.gameObject.transform.GetChild(0).gameObject;
                    //Debug.Log($"Other player name is {otherPlayer.name} and my id is 1");
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().DeactivateHandshakeConfirmCanvas();
                otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject.transform.GetComponent<HandshakeConfirmCanvasH2>().ActivateHandshakeConfirmCanvas();

                myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = true;
            }
        }
    }

    [PunRPC]
    public void ReleasedAOverNetwork(object[] ids)
    {
        string[] playersIds = new string[2];
        playersIds[0] = (string)ids[0];
        playersIds[1] = (string)ids[1];
        if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
        {
            myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = false;

            otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().ActivateHandshakeConfirmCanvas();
            otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<HandshakeWaitingCanvasH2>().DeactivateHandshakeConfirmCanvas();

        }
        else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
        {
            myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = false;

            otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<HandshakeMessageCanvasH2>().ActivateHandshakeConfirmCanvas();
            otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject.transform.GetComponent<HandshakeConfirmCanvasH2>().DeactivateHandshakeConfirmCanvas();
        }
    }
}

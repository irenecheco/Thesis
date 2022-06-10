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

    private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_waitingCanva = "Handshake 2 waiting";
    private string handshake2_confirmCanva = "Handshake 2 confirm";

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = this.transform.GetComponent<PhotonView>();
        myHead = this.gameObject;
    }

    public void CallPressedAOverNetwork(string pl1ID, string pl2ID)
    {
        playersID[0] = pl1ID;
        playersID[1] = pl2ID;
        Debug.Log($"id 1 è {playersID[0]}, id 2 è {playersID[1]}");
        photonView.RPC("PressedAOverNetwork", RpcTarget.All, playersID as object[]);
    }

    [PunRPC]
    public void PressedAOverNetwork(object[] ids)
    {
        string[] playersIds = new string[2];
        playersIds[0] = (string)ids[0];
        playersIds[1] = (string)ids[1];
        if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
        {
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[1])
                {
                    otherPlayer = (GameObject)item.TagObject;
                    otherPlayerHead = otherPlayer.transform.GetChild(0).gameObject;
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                myHead.transform.Find(handshake2_confirmCanva).gameObject.SetActive(true);
                myHead.transform.Find(handshake2_confirmCanva).gameObject.GetComponent<Canvas>().enabled = true;
                otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.SetActive(true);
                otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.GetComponent<Canvas>().enabled = true;
                otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.SetActive(false);
                otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.GetComponent<Canvas>().enabled = false;
                myHead.transform.GetComponent<OnCollisionActivateButton>().buttonAPressed = true;
                myHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;
                myHead.transform.Find(handshake2_messageCanva).gameObject.SetActive(false);
                otherPlayerHead.transform.GetComponent<OnCollisionActivateButton>().buttonAPressed = true;
            }
        }
        else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
        {
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[0])
                {
                    otherPlayer = (GameObject)item.TagObject;
                    otherPlayerHead = otherPlayer.transform.GetChild(0).gameObject;
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject.SetActive(true);
                otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject.GetComponent<Canvas>().enabled = true;
                myHead.transform.Find(handshake2_waitingCanva).gameObject.SetActive(true);
                myHead.transform.Find(handshake2_waitingCanva).gameObject.GetComponent<Canvas>().enabled = true;
                myHead.transform.Find(handshake2_messageCanva).gameObject.SetActive(false);
                myHead.transform.Find(handshake2_messageCanva).gameObject.GetComponent<Canvas>().enabled = false;
                otherPlayerHead.transform.GetComponent<OnCollisionActivateButton>().buttonAPressed = true;
                otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;
                otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.SetActive(false);
                myHead.transform.GetComponent<OnCollisionActivateButton>().buttonAPressed = true;
            }
        }
    }
}

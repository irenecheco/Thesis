using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeButton : MonoBehaviour
{
    private GameObject player;
    private GameObject myPlayer;
    private GameObject myPlayerHead;
    private GameObject myPlayerConfirm;
    private GameObject handshakeUI;
    private GameObject waitConfirmUI;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject rightController;

    // Start is called before the first frame update
    void Start()
    {
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        rightController = GameObject.Find("Camera Offset/RightHand Controller");

        player = GameObject.Find("Player");
        player.transform.position = GameObject.Find("Camera Offset/RightHand Controller").transform.position;
        if(this.gameObject.name == "Handshake Button")
        {
            handshakeUI = this.gameObject.transform.parent.gameObject;
            leftHand = handshakeUI.transform.parent.gameObject;
            waitConfirmUI = leftHand.transform.GetChild(3).gameObject;
            waitConfirmUI.GetComponent<Canvas>().enabled = false;
        }
    }

    public void OnHandshakePressed()
    {
        for(int i=0; i<PhotonNetwork.PlayerList.Length; i++)
        {
            foreach(var item in PhotonNetwork.PlayerList)
            {
                if (item.IsLocal)
                {
                    myPlayer = (GameObject)item.TagObject;
                }
            }
            //myPlayer = GameObject.Find($"Network Player {i}");
            if(myPlayer.GetComponent<PhotonView>().IsMine)
            {
                break;
            }
        }
        myPlayerHead = myPlayer.transform.GetChild(0).gameObject;
        myPlayerConfirm = myPlayerHead.transform.GetChild(3).gameObject;
        myPlayerConfirm.GetComponent<HandshakeConfirmCanvas>().ActivateHandshakeConfirmCanvas();
        if (this.gameObject.name == "Handshake Button")
        {
            handshakeUI.GetComponent<Canvas>().enabled = false;
            waitConfirmUI.GetComponent<Canvas>().enabled = true;
        }
    }

    public void SetBackComponent()
    {
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<HandController>();
        rightController.GetComponent<HandController>().hand = rightHand.GetComponent<Hand>();
        rightHand.GetComponent<Hand>().flag = false;
    }

}

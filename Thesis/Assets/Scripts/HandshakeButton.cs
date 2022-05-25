using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeButton : MonoBehaviour
{
    private GameObject rightHand;
    private GameObject rightController;
    private GameObject player;
    private GameObject camera;
    private GameObject otherPlayer;
    private GameObject netPlayer;
    private GameObject rHandContainer;
    private GameObject rHand;
    private GameObject myPlayer;
    private GameObject myPlayerHead;
    private GameObject myPlayerConfirm;
    private GameObject handshakeUI;
    private GameObject waitConfirmUI;
    private GameObject leftHand;


    float y_angle;
    //float x_angle;
    //float z_angle;
    private Vector3 direction;
    //private Animation handshakeAnimation;
    //private AnimatorStateInfo animStateInfo;
    //private Vector3 rHandPosition;
    //rivate Vector3 lHandPosition;

    // Start is called before the first frame update
    void Start()
    {
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        player = GameObject.Find("Player");
        camera = GameObject.Find("Camera Offset/Main Camera");
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
            myPlayer = GameObject.Find($"Network Player {+i}");
            if(myPlayer.GetComponent<PhotonView>().Owner.IsLocal)
            {
                break;
            }
        }
        myPlayerHead = myPlayer.transform.GetChild(0).gameObject;
        myPlayerConfirm = myPlayerHead.transform.GetChild(0).gameObject;
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
    }

}

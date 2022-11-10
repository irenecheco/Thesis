using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class NetworkHandshakeActivationH1 : MonoBehaviour
{
    //Code responsible for the activation of the handshake animation over the network: calls a method over the network so
    //that the involved users get notified and start the animation

    private GameObject rightHand;
    private GameObject leftHand;
    private GameObject rightController;
    private GameObject camera;
    private GameObject rHandContainer;
    private GameObject rHand;
    private GameObject otherPlayer;
    private GameObject confirmCanvas;
    private GameObject waitConfirmUI_l;
    private GameObject handshakeUI_l;
    private GameObject waitConfirmUI_r;
    private GameObject handshakeUI_r;
    private GameObject myHead;
    private PhotonView photonView;

    private GameObject fakeHandHolder;
    private GameObject fakeHand;
    
    private string[] playersID = new string[2];

    void Awake()
    {
        photonView = this.GetComponent<PhotonView>();
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        leftHand = GameObject.Find("Camera Offset/LeftHand Controller/LeftHand");

        waitConfirmUI_l = leftHand.transform.GetChild(3).gameObject;
        handshakeUI_l = leftHand.transform.GetChild(2).gameObject;
        waitConfirmUI_r = rightHand.transform.GetChild(3).gameObject;
        handshakeUI_r = rightHand.transform.GetChild(2).gameObject;

        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        fakeHandHolder = GameObject.Find("FakeHandHolder");
        fakeHand = fakeHandHolder.transform.GetChild(0).gameObject;
        camera = GameObject.Find("Camera Offset/Main Camera");
        myHead = this.gameObject.transform.GetChild(0).gameObject;

        confirmCanvas = myHead.transform.GetChild(0).gameObject;        
    }

    //Functions called when the handshake confirm button is pressed
    public void CallActivationOverNetwork(string pl1ID, string pl2ID)
    {
        playersID[0] = pl1ID;
        playersID[1] = pl2ID;

        if(pl1ID != null && pl2ID != null)
        {
            //Photon Pun method that calls a function over the network
            photonView.RPC("ActivateHandshakeOverNetwork", RpcTarget.All, playersID as object[]);
        }
    }

    //Function called over the network (on every network player): it checks if the player is involved in the handshake and,
    //if it is, it triggers the animation
    [PunRPC]
    public void ActivateHandshakeOverNetwork(object[] ids)
    {
        string[] playersIds = new string[2];
        playersIds[0] = (string)ids[0];
        playersIds[1] = (string)ids[1];
        if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
        {
            foreach( var item in PhotonNetwork.PlayerList)
            {
                if(item.UserId == playersIds[1])
                {
                    otherPlayer = (GameObject)item.TagObject;
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                rHandContainer = otherPlayer.transform.GetChild(2).gameObject;
                rHand = rHandContainer.transform.GetChild(0).gameObject;
                if (rHand.name == "RightHand")
                {
                    rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, rightController.transform.position);
                }
                else
                {
                    rHand = rHandContainer.transform.GetChild(1).gameObject;
                    rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, rightController.transform.position);
                }
            }
            StartCoroutine(Wait());

        } else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
        {
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[0])
                {
                    otherPlayer = (GameObject)item.TagObject;
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                rHandContainer = otherPlayer.transform.GetChild(2).gameObject;
                rHand = rHandContainer.transform.GetChild(0).gameObject;
                if (rHand.name == "RightHand")
                {
                    rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, rightController.transform.position);
                }
                else
                {
                    rHand = rHandContainer.transform.GetChild(1).gameObject;
                    rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, rightController.transform.position);
                }
            }
            StartCoroutine(Wait());
        }
    }

    //Coroutine that trigger the animation on the network player
    public IEnumerator Wait()
    {
        float time = (float)0.25;
        GameObject head = otherPlayer.transform.GetChild(0).gameObject;
        yield return new WaitForSeconds(time);

        rightHand.SetActive(false);
        fakeHand.SetActive(true);

        fakeHand.GetComponent<HandshakeFakeHand>().DoHandshake(camera.transform.position, head.transform.position);

        if(waitConfirmUI_l.GetComponent<Canvas>().enabled == true)
        {
            waitConfirmUI_l.GetComponent<Canvas>().enabled = false;
            handshakeUI_l.GetComponent<Canvas>().enabled = true;
        }
        if(waitConfirmUI_r.GetComponent<Canvas>().enabled == true)
        {
            waitConfirmUI_r.GetComponent<Canvas>().enabled = false;
            handshakeUI_r.GetComponent<Canvas>().enabled = true;
        }       

        confirmCanvas.GetComponent<HandshakeConfirmCanvas>().DeactivateHandshakeConfirmCanvas();
    }
}

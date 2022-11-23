using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkHandshakeActivationH4 : MonoBehaviour
{
    //Code responsible for the activation of the handshake animation over the network: calls a method over the network so
    //that the involved users get notified and start the animation

    private GameObject rightHand;
    private GameObject leftHand;
    private GameObject rightController;
    private GameObject camera;
    private GameObject rHandContainer;
    private GameObject otherHand;
    private GameObject otherPlayer;
    private GameObject confirmCanvas;
    /*private GameObject waitConfirmUI_l;
    private GameObject handshakeUI_l;
    private GameObject waitConfirmUI_r;
    private GameObject handshakeUI_r;*/
    private GameObject myHead;
    private GameObject localNetPlayer;
    private PhotonView photonView;

    private GameObject fakeHandHolder;
    private GameObject fakeHand;
    
    private string[] playersID = new string[2];
    private int[] playersHands = new int[2];

    void Awake()
    {
        photonView = this.GetComponent<PhotonView>();
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        leftHand = GameObject.Find("Camera Offset/LeftHand Controller/LeftHand");
        if (this.GetComponent<PhotonView>().IsMine)
        {
            localNetPlayer = this.gameObject;
        }

        /*waitConfirmUI_l = leftHand.transform.GetChild(3).gameObject;
        handshakeUI_l = leftHand.transform.GetChild(2).gameObject;
        waitConfirmUI_r = rightHand.transform.GetChild(3).gameObject;
        handshakeUI_r = rightHand.transform.GetChild(2).gameObject;*/

        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        fakeHandHolder = GameObject.Find("FakeHandHolder");
        fakeHand = fakeHandHolder.transform.GetChild(0).gameObject;
        camera = Camera.main.gameObject;
        myHead = this.gameObject.transform.GetChild(0).gameObject;

        confirmCanvas = myHead.transform.GetChild(0).gameObject;        
    }

    //Functions called when the handshake confirm button is pressed
    public void CallActivationOverNetwork(string pl1ID, string pl2ID, int myNetRightHand, int otherHand)
    {
        playersID[0] = pl1ID;
        playersID[1] = pl2ID;
        playersHands[0] = myNetRightHand;
        playersHands[1] = otherHand;

        if(pl1ID != null && pl2ID != null)
        {
            //Photon Pun method that calls a function over the network
            photonView.RPC("ActivateHandshakeOverNetwork", RpcTarget.All, playersID as object[], playersHands as int[]);
        }
    }

    //Function called over the network (on every network player): it checks if the player is involved in the handshake and,
    //if it is, it triggers the animation
    [PunRPC]
    public void ActivateHandshakeOverNetwork(object[] ids, int[] hands)
    {
        if (!this.GetComponent<PhotonView>().IsMine)
        {
            //Debug.Log($"{pl1_rightHand} è mano pl 1, {pl2_rightHand} è mano pl 2");
            string[] playersIds = new string[2];
            playersIds[0] = (string)ids[0];
            playersIds[1] = (string)ids[1];
            if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
            {
                //My player is in position 0: I am calling the handshake
                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.UserId == playersIds[1])
                    {
                        otherPlayer = (GameObject)item.TagObject;
                    }
                    if(item.UserId == PhotonNetwork.LocalPlayer.UserId)
                    {
                        localNetPlayer = (GameObject)item.TagObject;
                    }
                }
                otherHand = PhotonView.Find(hands[1]).gameObject;
                if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
                {
                    //rHandContainer = otherPlayer.transform.GetChild(2).gameObject;
                    //rHand = rHandContainer.transform.GetChild(0).gameObject;
                    /*if (otherHand.name == "RightHand")
                    {*/
                    otherHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, true);
                    otherHand.GetComponent<MessageActivationH4>().isGrabbing = false;
                    
                    /*}
                    else
                    {
                        otherHand = rHandContainer.transform.GetChild(1).gameObject;
                        otherHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, true);
                    }*/
                    StartCoroutine(Wait());
                }

            }
            else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
            {
                //My player is in position 1: the other player is calling the handshake
                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.UserId == playersIds[0])
                    {
                        otherPlayer = (GameObject)item.TagObject;
                    }
                }
                otherHand = PhotonView.Find(hands[0]).gameObject;
                if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
                {
                    //rHandContainer = otherPlayer.transform.GetChild(2).gameObject;
                    //otherHand = rHandContainer.transform.GetChild(0).gameObject;
                    /*if (otherHand.name == "RightHand")
                    {*/
                    otherHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, false);
                    otherHand.GetComponent<MessageActivationH4>().isGrabbing = false;
                    
                    /*}
                    else
                    {
                        otherHand = rHandContainer.transform.GetChild(1).gameObject;
                        otherHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, false);
                    }*/
                    StartCoroutine(Wait());
                }
            }
        }
    }

    //Coroutine that trigger the animation on the network player
    public IEnumerator Wait()
    {
        float time = (float)0.25;
        GameObject head = otherPlayer.transform.GetChild(0).gameObject;
        yield return new WaitForSeconds(time);
        rightController.GetComponent<XRDirectInteractor>().allowSelect = false;
        rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
        

        rightHand.SetActive(false);
        fakeHand.SetActive(true);

        fakeHand.GetComponent<HandshakeFakeHand>().DoHandshakeH4(camera.transform.position, head.transform.position);

        /*if(waitConfirmUI_l.GetComponent<Canvas>().enabled == true)
        {
            waitConfirmUI_l.GetComponent<Canvas>().enabled = false;
            handshakeUI_l.GetComponent<Canvas>().enabled = true;
        }
        if(waitConfirmUI_r.GetComponent<Canvas>().enabled == true)
        {
            waitConfirmUI_r.GetComponent<Canvas>().enabled = false;
            handshakeUI_r.GetComponent<Canvas>().enabled = true;
        }*/       

        //confirmCanvas.GetComponent<HandshakeConfirmCanvas>().DeactivateHandshakeConfirmCanvas();
    }
}

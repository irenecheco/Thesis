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
    private GameObject handMesh;
    private GameObject otherHandMesh;

    private System.DateTime initialTimeH2Player;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color waitingColor = new Color(0.4135279f, 0.7409829f, 0.9056604f, 1.0f);

    private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_confirmCanva = "Handshake 2 confirm";

    private PhotonView photonView;

    void Start()
    {
        photonView = this.transform.GetComponent<PhotonView>();
        myHead = this.gameObject;
        //Debug.Log($"{myHead.transform.parent.gameObject.name}");
        myPlayer = this.gameObject.transform.parent.gameObject;
        myRightController = myPlayer.gameObject.transform.GetChild(2).gameObject;
        myRightHand = myRightController.gameObject.transform.GetChild(0).gameObject;
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        handMesh = rightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
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
        //Debug.Log("called pressed over network");
            //Debug.Log($"{myHead.gameObject.name} and parent {myHead.transform.parent.gameObject.name}");
            string[] playersIds = new string[2];
            playersIds[0] = (string)ids[0];
            playersIds[1] = (string)ids[1];

            if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
            {
                //My user id is in position 0: I am pressing A button
                //Debug.Log("my player is realeasing");

                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.UserId == playersIds[1])
                    {
                        otherPlayer = (GameObject)item.TagObject;
                        otherPlayerHead = otherPlayer.transform.GetChild(0).gameObject;
                        otherPlayerRightController = otherPlayer.gameObject.transform.GetChild(2).gameObject;
                        otherPlayerRightHand = otherPlayerRightController.gameObject.transform.GetChild(0).gameObject;
                        otherHandMesh = otherPlayerRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                    }
                }
                if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
                {
                    if (myHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed == true && otherPlayerHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed == false)
                    {
                        GameObject confirmCanvas = otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject;

                    InteractionsCount.startedInteractionsFromTesterH2++;
                        // cambia colore alla mia mano locale
                        //handMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
                        rightHand.GetComponent<HapticController>().amplitude = 0.1f;
                        rightHand.GetComponent<HapticController>().duration = 0.1f;
                        rightHand.GetComponent<HapticController>().SendHaptics();

                        rightHand.transform.GetChild(2).gameObject.GetComponent<Canvas>().enabled = true;
                        //otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<Canvas>().enabled = true;
                        otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;
                        confirmCanvas.transform.GetComponent<Canvas>().enabled = false;
                        confirmCanvas.transform.GetComponent<AudioSource>().enabled = false;

                        //myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = true;
                    }
                }
            }
            else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
            {
            GameObject localNetPlayer = null;
            GameObject localPlayerHead = null;
                //My user id is in position 1: the other player is pressing A button
                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.UserId == playersIds[0])
                    {
                        otherPlayer = (GameObject)item.TagObject;
                        otherPlayerHead = otherPlayer.transform.GetChild(0).gameObject;
                        otherPlayerRightController = otherPlayer.gameObject.transform.GetChild(2).gameObject;
                        otherPlayerRightHand = otherPlayerRightController.gameObject.transform.GetChild(0).gameObject;
                        otherHandMesh = otherPlayerRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;

                    } else if(item.UserId == playersIds[1])
                {
                    localNetPlayer = (GameObject)item.TagObject;
                    localPlayerHead = localNetPlayer.transform.GetChild(0).gameObject;
                }
                }
                if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null && myHead.GetComponent<OnButtonAPressed>().isPressed == false)
                {
                    GameObject confirmCanvas = otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject;

                // colora mano dell'altro
                if (!this.GetComponent<PhotonView>().IsMine)
                {
                    InteractionsCount.startedInteractionsFromExperimenterH2++;
                    initialTimeH2Player = System.DateTime.UtcNow;
                    //Debug.Log($"{initialTimeH2Player}");
                    //Debug.Log($"Entra qui e initial tim è {initialTimeH2Player}");
                    localPlayerHead.GetComponent<OnButtonAPressed>().initialTimeH2Player = initialTimeH2Player;
                }
                
                otherHandMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;

                    rightHand.transform.GetChild(2).gameObject.GetComponent<Canvas>().enabled = false;
                    otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;
                    //otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;
                    confirmCanvas.transform.GetComponent<Canvas>().enabled = true;
                    confirmCanvas.transform.GetComponent<AudioSource>().enabled = true;
                    confirmCanvas.transform.GetComponent<AudioSource>().Play();

                    //myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = true;
                }
            }
    }

    //Function called over the network (on every network player): it checks if the player is involved in the handshake and,
    //if it is, it changes the canvas accordingly
    [PunRPC]
    public void ReleasedAOverNetwork(object[] ids)
    {
        //Debug.Log("called release over network");
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
                        otherPlayerRightController = otherPlayer.gameObject.transform.GetChild(2).gameObject;
                        otherPlayerRightHand = otherPlayerRightController.gameObject.transform.GetChild(0).gameObject;
                        otherHandMesh = otherPlayerRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                    }
                }
                //My user id is in position 0: I am releasing A button
                //Debug.Log("my player is realeasing");
                //myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = false;

                if (otherPlayerHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed == true)
                {
                    GameObject confirmCanvas = otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject;

                    //colora mano dell'altro
                    otherHandMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;

                    rightHand.transform.GetChild(2).gameObject.GetComponent<Canvas>().enabled = false;
                    otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;
                    //otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;
                    confirmCanvas.transform.GetComponent<Canvas>().enabled = true;
                    confirmCanvas.transform.GetComponent<AudioSource>().enabled = true;
                    confirmCanvas.transform.GetComponent<AudioSource>().Play();
                }
                else
                {
                    GameObject confirmCanvas = otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject;

                    //colore mano locale normale
                    //handMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;

                    rightHand.transform.GetChild(2).gameObject.GetComponent<Canvas>().enabled = false;
                    otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<Canvas>().enabled = true;
                    //otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;
                    confirmCanvas.transform.GetComponent<Canvas>().enabled = false;
                    confirmCanvas.transform.GetComponent<AudioSource>().enabled = false;
                }
            }
            else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
            {
            GameObject localPlayer;
            GameObject localHead = myHead;
            //My user id is in position 1: the other player is releasing A button
            foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.UserId == playersIds[0])
                    {
                        otherPlayer = (GameObject)item.TagObject;
                        otherPlayerHead = otherPlayer.transform.GetChild(0).gameObject;
                        otherPlayerRightController = otherPlayer.gameObject.transform.GetChild(2).gameObject;
                        otherPlayerRightHand = otherPlayerRightController.gameObject.transform.GetChild(0).gameObject;
                        otherHandMesh = otherPlayerRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                    } else if(item.UserId == playersIds[1])
                    {
                        localPlayer = (GameObject)item.TagObject;
                        localHead = localPlayer.transform.GetChild(0).gameObject;
                    }
                }

                //Debug.Log("other player is realeasing");
                //myHead.transform.GetComponent<OnCollisionActivateCanvasH2>().buttonAPressed = false;
                
                if (localHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed == true)
                {
                    //Debug.Log($"my ispressed is {myHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed} and my parent is {myHead.transform.parent.gameObject.name}");
                    GameObject confirmCanvas = otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject;

                    //cambia colore mano locale
                    //handMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
                    rightHand.GetComponent<HapticController>().amplitude = 0.1f;
                    rightHand.GetComponent<HapticController>().duration = 0.1f;
                    rightHand.GetComponent<HapticController>().SendHaptics();

                    otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;
                    confirmCanvas.transform.GetComponent<Canvas>().enabled = false;
                    confirmCanvas.transform.GetComponent<AudioSource>().enabled = false;
                    rightHand.transform.GetChild(2).gameObject.GetComponent<Canvas>().enabled = true;
                    //otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<Canvas>().enabled = true;
                }
                else
                {
                    //Debug.Log($"my ispressed is {myHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed} and my head is {myHead.gameObject.name}");
                    GameObject confirmCanvas = otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject;

                    // colore altra mano normale
                    otherHandMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;

                    rightHand.transform.GetChild(2).gameObject.GetComponent<Canvas>().enabled = false;
                    otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject.transform.GetComponent<Canvas>().enabled = true;
                    confirmCanvas.transform.GetComponent<Canvas>().enabled = false;
                    confirmCanvas.transform.GetComponent<AudioSource>().enabled = false;
                    //otherPlayerHead.transform.Find(handshake2_waitingCanva).gameObject.transform.GetComponent<Canvas>().enabled = false;
                }
            }
        }
}

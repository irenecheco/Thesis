using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkGrabMessageActivationH3 : MonoBehaviour
{
    //Code responsible fot the activation of the message canvas over the network when user grab the hand of the other user

    private string[] playersID = new string[2];

    private PhotonView photonView;

    private GameObject rightController;
    private GameObject rightHand;

    private GameObject GrabbedNetPlayer;
    private GameObject GrabbedNetRightController;
    private GameObject GrabbedNetRightHand;

    private GameObject GrabbingNetPlayer;
    private GameObject GrabbingNetHead;
    private GameObject GrabbingNetMessageCanvas;

    private GameObject ReleasedNetPlayer;
    private GameObject ReleasedNetRightController;
    private GameObject ReleasedNetRightHand;
    private GameObject ReleasedNetHead;
    private GameObject ReleasedNetMessageCanvas;

    void Start()
    {
        photonView = this.transform.GetParentComponent<PhotonView>();

        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        rightHand = rightController.transform.GetChild(0).gameObject;
    }

    //Functions called when user grab the other user hand
    public void CallActivateGrabMessage(string plGrabbing, string plGrabbed)
    {
        playersID[0] = plGrabbing;
        playersID[1] = plGrabbed;
        if (plGrabbing != null && plGrabbed != null)
        {
            //Photon Pun method that calls a function over the network
            photonView.RPC("ActivateGrabMessageOverNetwork", RpcTarget.All, playersID as object[]);
        }
    }

    //Functions called when user release the other user hand
    public void CallDeactivateGrabMessage(string plGrabbing, string plGrabbed)
    {
        playersID[0] = plGrabbing;
        playersID[1] = plGrabbed;
        if (plGrabbing != null && plGrabbed != null)
        {
            //Photon Pun method that calls a function over the network
            photonView.RPC("DeactivateGrabMessageOverNetwork", RpcTarget.All, playersID as object[]);
        }
    }

    //Function called over the network (on every network player): it checks if the player is involved in the handshake and,
    //if it is, it triggers the message
    [PunRPC]
    public void ActivateGrabMessageOverNetwork(object[] ids)
    {
        string plGrabbing;
        string plGrabbed;
        plGrabbing = (string)ids[0];
        plGrabbed = (string)ids[1];
        if(plGrabbed == PhotonNetwork.LocalPlayer.UserId)
        {
            //My user id is in position 1: my hand is being grabbed

            rightHand.GetComponent<HapticController>().SendHaptics1H3();
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == plGrabbed)
                {
                    GrabbedNetPlayer = (GameObject)item.TagObject;
                    GrabbedNetRightController = GrabbedNetPlayer.transform.GetChild(2).gameObject;
                    GrabbedNetRightHand = GrabbedNetRightController.transform.GetChild(0).gameObject;
                }
            }

            //If I'm not already grabbing the other users's hand, a message appears in front of me
            if(GrabbedNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == false)
            {
                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.UserId == plGrabbing)
                    {
                        GrabbingNetPlayer = (GameObject)item.TagObject;
                        GrabbingNetHead = GrabbingNetPlayer.transform.GetChild(0).gameObject;
                        GrabbingNetMessageCanvas = GrabbingNetHead.transform.GetChild(2).gameObject;
                    }
                }

                GrabbingNetMessageCanvas.GetComponent<Canvas>().enabled = true;
            }
        } else if(plGrabbing == PhotonNetwork.LocalPlayer.UserId)
        {
            //My user id is in position 0: I'm grabbing, I get an haptic feedback
            rightHand.GetComponent<HapticController>().SendHaptics1H3();
        }
    }

    //Function called over the network (on every network player): it checks if the player is involved in the handshake and,
    //if it is, it triggers the message
    [PunRPC]
    public void DeactivateGrabMessageOverNetwork(object[] ids)
    {
        string plReleasing;
        string plReleased;
        plReleasing = (string)ids[0];
        plReleased = (string)ids[1];
        if (plReleasing == PhotonNetwork.LocalPlayer.UserId)
        {
            //My user id is in position 0: I'm releasing the other user's hand
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == plReleased)
                {
                    ReleasedNetPlayer = (GameObject)item.TagObject;
                    ReleasedNetRightController = ReleasedNetPlayer.transform.GetChild(2).gameObject;
                    ReleasedNetRightHand = ReleasedNetRightController.transform.GetChild(0).gameObject;
                    ReleasedNetHead = ReleasedNetPlayer.transform.GetChild(0).gameObject;
                    ReleasedNetMessageCanvas = ReleasedNetHead.transform.GetChild(2).gameObject;
                }
            }

            //If the other user is still grabbing my hand, a message appears in front of me
            if (ReleasedNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == true)
            {
                ReleasedNetMessageCanvas.GetComponent<Canvas>().enabled = true;
            } else
            {
                ReleasedNetMessageCanvas.GetComponent<Canvas>().enabled = false;
            }
        }
    }
}

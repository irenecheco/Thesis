using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class MessageActivationH3 : MonoBehaviour, IPunObservable
{
    //Code responsible fot the activation of the message canvas locally when user grab the hand of the other user

    private GameObject rightController;
    private GameObject rightHand;
    private GameObject mainCamera;
    private GameObject headLocal;
    private GameObject messageCanvas;
    private GameObject myNetPlayer;
    private GameObject myNetRightController;
    private GameObject myNetRightHand;
    private GameObject otherPlayerRightHand;
    private GameObject thisRightHand;
    private GameObject thisRightController;
    private GameObject thisPlayer;

    private string thisId;
    private string localId;

    public bool isGrabbing;

    private PhotonView photonView;

    void Start()
    {
        isGrabbing = false;
        photonView = this.transform.GetParentComponent<PhotonView>();
        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        rightHand = rightController.transform.GetChild(0).gameObject;
        mainCamera = GameObject.Find("Camera Offset/Main Camera");
        headLocal = mainCamera.transform.GetChild(0).gameObject;
        messageCanvas = headLocal.transform.GetChild(1).gameObject;

        //If the network player's object is mine, I cannot grab the hand
        if (photonView.IsMine)
        {
            this.GetComponent<XRGrabInteractable>().enabled = false;
        }

        if (!photonView.IsMine)
        {
            thisRightHand = this.gameObject;
            thisRightController = thisRightHand.transform.parent.gameObject;
            thisPlayer = thisRightController.transform.parent.gameObject;

            foreach (var item in PhotonNetwork.PlayerList)
            {
                if ((object)item.TagObject == thisPlayer)
                {
                    thisId = item.UserId;
                }
            }

            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == PhotonNetwork.LocalPlayer.UserId)
                {
                    myNetPlayer = (GameObject)item.TagObject;
                    myNetRightController = myNetPlayer.transform.GetChild(2).gameObject;
                    myNetRightHand = myNetRightController.transform.GetChild(0).gameObject;
                    localId = item.UserId;
                }
            }
        }
    }

    //When I grab the other user's hand, a message appears in front of me and the network activation is called
    public void ActivateMessage()
    {
        if (!photonView.IsMine && photonView != null)
        {
            messageCanvas.GetComponent<Canvas>().enabled = true;
            rightController.GetComponent<ActionBasedController>().enableInputTracking = false;
            rightHand.GetComponent<GrabbingH3>().SetGrabbing(this.gameObject, thisId);
            myNetPlayer.GetComponent<NetworkGrabMessageActivationH3>().CallActivateGrabMessage(localId, thisId);
        }
    }

    //When I release the other user's hand, if I have a message in front of me, it disappears and the network deactivation
    //is called
    public void DeactivateMessage()
    {
        if (!photonView.IsMine && photonView != null)
        {
            messageCanvas.GetComponent<Canvas>().enabled = false;
            rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
            if (myNetRightHand != null)
            {
                myNetRightHand.GetComponent<MessageActivationH3>().isGrabbing = false;
            }
            myNetPlayer.GetComponent<NetworkGrabMessageActivationH3>().CallDeactivateGrabMessage(localId, thisId);
        }
    }

    //Method of the Photon Pun library that lets you keep track of a variable through the network (class IPunObservable)
    //In this case it keeps track of a bool that is true when my user is grabbing
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isGrabbing);
        }
        else
        {
            this.isGrabbing = (bool)stream.ReceiveNext();
        }
    }
}

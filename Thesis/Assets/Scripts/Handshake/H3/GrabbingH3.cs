using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbingH3 : MonoBehaviour
{
    private GameObject myNetPlayer;
    private GameObject myNetRightController;
    private GameObject myNetRightHand;

    private GameObject otherNetPlayer;
    private GameObject otherNetRightController;
    private GameObject otherNetRightHand;

    private GameObject rightController;

    private string myId;
    private string otherPlayerId;

    void Start()
    {
        rightController = this.transform.parent.gameObject;

        myId = PhotonNetwork.LocalPlayer.UserId;        
    }

    public void SetGrabbing(string otherPlId)
    {
        otherPlayerId = otherPlId;
        
        myNetRightHand.GetComponent<MessageActivationH3>().isGrabbing = true;
        otherPlayerId = otherPlId;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.UserId == otherPlayerId)
            {
                otherNetPlayer = (GameObject)item.TagObject;
                otherNetRightController = otherNetPlayer.transform.GetChild(2).gameObject;
                if(otherNetRightController.transform.childCount > 0)
                {
                    otherNetRightHand = otherNetRightController.transform.GetChild(0).gameObject;
                }                
            }
        }
    }

    void Update()
    {
        if (myNetPlayer == null)
        {
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == myId)
                {
                    myNetPlayer = (GameObject)item.TagObject;
                    myNetRightController = myNetPlayer.transform.GetChild(2).gameObject;
                    myNetRightHand = myNetRightController.transform.GetChild(0).gameObject;
                }
            }
        }

        if (otherNetRightHand != null)
        {
            if (myNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == true)
            {
                if (otherNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == true)
                {
                    rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
                    this.GetComponent<CollidingH3>().isGrabbing = true;
                }
                else
                {
                    this.GetComponent<CollidingH3>().isGrabbing = false;
                }
            }
            else
            {
                this.GetComponent<CollidingH3>().isGrabbing = false;
            }
        }
    }
}

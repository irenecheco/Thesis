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
    private GameObject otherNetHead;
    private GameObject otherNetGrabMessageCanvas;
    public GameObject otherNetRightHand;

    private GameObject rightController;

    private GameObject mainCamera;
    private GameObject headLocal;
    private GameObject messageCanvas;

    private string myId;
    private string otherPlayerId;

    private bool areShaking;

    private int frameNumber;
    private bool firstFrame;

    void Start()
    {
        rightController = this.transform.parent.gameObject;

        myId = PhotonNetwork.LocalPlayer.UserId;

        mainCamera = GameObject.Find("Camera Offset/Main Camera");
        headLocal = mainCamera.transform.GetChild(0).gameObject;
        messageCanvas = headLocal.transform.GetChild(1).gameObject;

        areShaking = false;
        frameNumber = 0;
        firstFrame = true;
    }

    public void SetGrabbing(GameObject otherNetPlRightHand, string otherPlId)
    {        
        myNetRightHand.GetComponent<MessageActivationH3>().isGrabbing = true;
        otherNetRightHand = otherNetPlRightHand;

        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.UserId == otherPlId)
            {
                otherNetPlayer = (GameObject)item.TagObject;
                otherNetHead = otherNetPlayer.transform.GetChild(0).gameObject;
                otherNetGrabMessageCanvas = otherNetHead.transform.GetChild(2).gameObject;
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
        } else
        {
            //Debug.Log($"my net right hand is {myNetRightHand.name} e isGrabbing è {myNetRightHand.GetComponent<MessageActivationH3>().isGrabbing}");
        }

        if (otherNetRightHand != null)
        {
            //Debug.Log($"my net right hand is {otherNetRightHand.name} e isGrabbing è {otherNetRightHand.GetComponent<MessageActivationH3>().isGrabbing}");
            if (myNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == true)
            {
                if (otherNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == true)
                {
                    rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
                    rightController.GetComponent<HandController>().isGrabbingH3 = true;
                    myNetRightController.GetComponent<NetworkHandController>().isGrabbingH3 = true;
                    otherNetRightHand.GetComponent<NetworkHand>().SetGrip(0);
                    messageCanvas.GetComponent<Canvas>().enabled = false;
                    this.GetComponent<CollidingH3>().otherNetRightHand = otherNetRightHand;
                    this.GetComponent<CollidingH3>().isGrabbing = true;
                    otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                    areShaking = true;

                    this.GetComponent<HapticController>().SendHaptics2H3();
                    if(firstFrame == true)
                    {
                        this.GetComponent<Outline>().enabled = true;
                        otherNetRightHand.GetComponent<Outline>().enabled = true;
                        otherNetRightHand.GetComponent<AudioSource>().Play();
                        firstFrame = false;
                    }
                    
                    frameNumber++;
                    if(frameNumber >= 30)
                    {
                        frameNumber = 0;
                        this.GetComponent<Outline>().enabled = false;
                        otherNetRightHand.GetComponent<Outline>().enabled = false;
                    }
                }
                else
                {
                    this.GetComponent<CollidingH3>().isGrabbing = false;
                    messageCanvas.GetComponent<Canvas>().enabled = true;
                    otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                    rightController.GetComponent<HandController>().isGrabbingH3 = false;
                    myNetRightController.GetComponent<NetworkHandController>().isGrabbingH3 = false;

                    areShaking = false;
                    firstFrame = true;
                }
            }
            else
            {
                if (otherNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == false)
                {
                    otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                }
                else
                {
                    otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = true;
                }
                messageCanvas.GetComponent<Canvas>().enabled = false;
                this.GetComponent<CollidingH3>().isGrabbing = false;
                rightController.GetComponent<HandController>().isGrabbingH3 = false;
                myNetRightController.GetComponent<NetworkHandController>().isGrabbingH3 = false;
                areShaking = false;
                firstFrame = true;
            }
        }
    }
}

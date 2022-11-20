using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbingH3 : MonoBehaviour
{
    //Code responsible for keeping track of grab button pressure

    private GameObject myNetPlayer;
    private GameObject myNetHead;
    private GameObject myNetRightController;
    private GameObject myNetRightHand;
    private GameObject myNetRightMesh;

    private GameObject otherNetPlayer;
    private GameObject otherNetHead;
    private GameObject otherNetGrabMessageCanvas;
    private GameObject otherNetGrabConfirmCanvas;
    public GameObject otherNetRightHand;
    public GameObject otherNetRightMesh;

    private GameObject rightController;

    private GameObject mainCamera;
    private GameObject headLocal;
    private GameObject messageCanvas;

    private string myId;
    private string otherPlayerId;

    private bool areShaking;

    private int frameNumber;
    private bool firstFrame;
    public bool isColliding;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color waitingColor = new Color(0.4135279f, 0.7409829f, 0.9056604f, 1.0f);

    void Start()
    {
        rightController = this.transform.parent.gameObject;

        myId = PhotonNetwork.LocalPlayer.UserId;

        //mainCamera = Camera.main.gameObject;
        //headLocal = mainCamera.transform.GetChild(0).gameObject;
        messageCanvas = this.transform.GetChild(3).gameObject;

        areShaking = false;
        frameNumber = 0;
        firstFrame = true;
        isColliding = false;
    }

    //When I'm grabbing the other user, this function get called and it saves the other user game object
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
                otherNetGrabConfirmCanvas = otherNetHead.transform.GetChild(3).gameObject;
            }
        }

        myNetHead.transform.FindChildRecursive("DeactivateCollider").gameObject.GetComponent<OnCollisionDeactivateCanvasH3>().otherRightHand = otherNetPlRightHand;
    }

    public void SetReleasing(GameObject otherNetPlRightHand, string otherPlId)
    {
        myNetRightHand.GetComponent<MessageActivationH3>().isGrabbing = false;
        //otherNetRightHand = otherNetPlRightHand;
    }

    void Update()
    {
        //It saves my network player's object whenever it gets instantiated
        if (myNetPlayer == null)
        {
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == myId)
                {
                    myNetPlayer = (GameObject)item.TagObject;
                    myNetHead = myNetPlayer.transform.GetChild(0).gameObject;
                    myNetRightController = myNetPlayer.transform.GetChild(2).gameObject;
                    myNetRightHand = myNetRightController.transform.GetChild(0).gameObject;
                }
            }
        }
        else
        {
            //Checks if I'm grabbing and if the other user is grabbing
            if (myNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == true)
            {
                //When I grab the other user's hand it gets saved in SetGrabbing() and this object is not null anymore
                if (otherNetRightHand != null)
                {
                    if(otherNetPlayer == null)
                    {
                        otherNetPlayer = otherNetRightHand.transform.parent.transform.parent.gameObject;
                        otherNetHead = otherNetPlayer.transform.GetChild(0).gameObject;
                        otherNetGrabMessageCanvas = otherNetHead.transform.GetChild(2).gameObject;
                        otherNetGrabConfirmCanvas = otherNetHead.transform.GetChild(3).gameObject;
                    }
                    if (otherNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == true)
                    {
                        if (firstFrame == true)
                        {
                            //If both users are grabbing they can handshake freely, if one of them release or they get to far they
                            //cannot handshake anymore
                            rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
                            rightController.GetComponent<HandController>().isGrabbingH3 = true;
                            myNetRightController.GetComponent<NetworkHandController>().isGrabbingH3 = true;
                            myNetRightMesh = myNetRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                            myNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                            otherNetRightHand.GetComponent<NetworkHand>().SetGrip(0);
                            otherNetRightMesh = otherNetRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                            otherNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                            messageCanvas.GetComponent<Canvas>().enabled = false;
                            this.GetComponent<CollidingH3>().otherNetRightHand = otherNetRightHand;
                            this.GetComponent<CollidingH3>().isGrabbing = true;
                            otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                            otherNetGrabConfirmCanvas.GetComponent<Canvas>().enabled = false;
                            areShaking = true;

                            //Haptic, visual and sound feedback are provided during the handshake
                            this.GetComponent<HapticController>().SendHaptics2H3();

                            this.GetComponent<Outline>().enabled = true;
                            otherNetRightHand.GetComponent<Outline>().enabled = true;
                            otherNetRightHand.GetComponent<AudioSource>().Play();
                            firstFrame = false;
                            Invoke("disableOutline", 0.30f);
                        }

                    }
                    else
                    {
                        //If I'm grabbing, but the other user is not, I cannot move my right hand until I release the button or
                        //the other user grab my hand
                        this.GetComponent<CollidingH3>().isGrabbing = true;
                        messageCanvas.GetComponent<Canvas>().enabled = true;
                        otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                        rightController.GetComponent<HandController>().isGrabbingH3 = true;
                        myNetRightController.GetComponent<NetworkHandController>().isGrabbingH3 = true;

                        areShaking = false;
                        firstFrame = true;
                    }
                }
            }
            else
            {
                if (otherNetRightHand != null)
                {
                    if (otherNetPlayer == null)
                    {
                        otherNetPlayer = otherNetRightHand.transform.parent.transform.parent.gameObject;
                        otherNetHead = otherNetPlayer.transform.GetChild(0).gameObject;
                        otherNetGrabMessageCanvas = otherNetHead.transform.GetChild(2).gameObject;
                        otherNetGrabConfirmCanvas = otherNetHead.transform.GetChild(3).gameObject;
                    }
                    if (otherNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == false)
                    {
                        otherNetRightMesh = otherNetRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                        otherNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                        if (isColliding)
                        {
                            otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = true;
                        }
                        else
                        {
                            otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                        }
                    }
                    else
                    {
                        otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                        otherNetRightMesh = otherNetRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                        otherNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
                    }
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

    private void disableOutline()
    {
        this.GetComponent<Outline>().enabled = false;
        otherNetRightHand.GetComponent<Outline>().enabled = false;
    }
}

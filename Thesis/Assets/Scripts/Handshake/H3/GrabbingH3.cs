using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using NLog.Unity;

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

    [SerializeField] private GameObject leftHand;

    private GameObject rightController;

    private GameObject messageCanvas;

    private string myId;
    private string otherPlayerId;

    private bool firstFrame;
    private bool firstFrameForCount;
    private bool firstFrameForCount2;
    private bool otherGrabbedFirst;
    public bool isColliding;
    public bool npcAnimationGoing;

    private System.DateTime initialTimeH3Player;
    private System.DateTime finalTimeH3Player;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color waitingColor = new Color(0.4135279f, 0.7409829f, 0.9056604f, 1.0f);

    void Start()
    {
        rightController = this.transform.parent.gameObject;
        npcAnimationGoing = false;

        myId = PhotonNetwork.LocalPlayer.UserId;

        messageCanvas = this.transform.GetChild(3).gameObject;

        firstFrame = true;
        firstFrameForCount = true;
        firstFrameForCount2 = true;
        isColliding = false;
        otherGrabbedFirst = false;
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
                            if (otherGrabbedFirst)
                            {
                                InteractionsCount.finishedInteractionsWithExperimenterH3++;
                                finalTimeH3Player = System.DateTime.UtcNow;
                                NLogConfig.LogLine($"{"Avatar"};TimeFromCanvasAppearing;{(finalTimeH3Player - initialTimeH3Player).TotalSeconds.ToString("#.000")};s");
                                otherGrabbedFirst = false;
                            }                            
                            firstFrameForCount = true;
                            firstFrameForCount2 = true;

                            leftHand.GetComponent<TotalHandshakeCount>().UpdateCountOnCanvas();

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
                        if(firstFrameForCount2== true)
                        {
                            InteractionsCount.startedInteractionsFromTesterH3++;
                            firstFrameForCount2 = false;
                        }                        
                        rightController.GetComponent<ActionBasedController>().enableInputTracking = false;
                        otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                        rightController.GetComponent<HandController>().isGrabbingH3 = true;                        
                        myNetRightController.GetComponent<NetworkHandController>().isGrabbingH3 = true;

                        firstFrame = true;
                        firstFrameForCount = true;
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
                        firstFrameForCount = true;
                        firstFrameForCount2 = true;
                        otherNetRightMesh = otherNetRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                        otherNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                        if (isColliding)
                        {
                            otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = true;
                            otherNetGrabConfirmCanvas.GetComponent<Canvas>().enabled = false;
                        }
                        else
                        {
                            otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                            otherNetGrabConfirmCanvas.GetComponent<Canvas>().enabled = false;
                        }
                    }
                    else
                    {
                        
                        if(firstFrameForCount == true)
                        {
                            otherGrabbedFirst = true;
                            initialTimeH3Player = System.DateTime.UtcNow;
                            InteractionsCount.startedInteractionsFromExperimenterH3++;
                            firstFrameForCount = false;
                        }                        
                        otherNetGrabConfirmCanvas.GetComponent<Canvas>().enabled = true;
                        otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                        otherNetRightMesh = otherNetRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                        otherNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
                        firstFrameForCount2 = true;
                    }
                }
                rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
                messageCanvas.GetComponent<Canvas>().enabled = false;
                this.GetComponent<CollidingH3>().isGrabbing = false;
                if (npcAnimationGoing == false)
                {
                    rightController.GetComponent<HandController>().isGrabbingH3 = false;
                }
                myNetRightController.GetComponent<NetworkHandController>().isGrabbingH3 = false;
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

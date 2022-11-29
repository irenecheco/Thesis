using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using NLog.Unity;

public class GrabbingH4 : MonoBehaviour
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
    private GameObject confirmCanvas;

    private string myId;
    private string otherPlayerId;

    private bool areShaking;

    public bool npcAnimationGoing;

    private int frameNumber;
    private bool firstFrame;
    public bool isColliding;
    private bool firstFrameForCount;
    private bool firstFrameForCount2;
    private bool otherGrabbedFirst;

    private string player1ID;
    private string player2ID;

    private System.DateTime initialTimeH4Player;
    private System.DateTime finalTimeH4Player;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color yellowColor = new Color(0.8679245f, 0.8271183f, 0.4208615f, 1.0f);
    private Color greenColor = new Color(0.4291207f, 0.7924528f, 0.6037189f, 1.0f);
    private Color waitingColor = new Color(0.4135279f, 0.7409829f, 0.9056604f, 1.0f);

    [SerializeField] private InputActionReference _releaseHandshake4;

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
        npcAnimationGoing = false;
        firstFrameForCount = true;
        firstFrameForCount2 = true;
        otherGrabbedFirst = false;

        _releaseHandshake4.action.performed += ctx =>
        { 
            if(myNetPlayer != null)
            {
                if(myNetPlayer.GetComponent<NetworkGrabMessageActivationH4>().animationGoing == true || npcAnimationGoing == true)
                {
                    
                    rightController.GetComponent<XRDirectInteractor>().allowSelect = true;
                    //rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
                    myNetPlayer.GetComponent<NetworkGrabMessageActivationH4>().animationGoing = false;
                    rightController.GetComponent<HandController>().isGrabbingH3 = false;
                    myNetRightController.GetComponent<NetworkHandController>().isGrabbingH3 = false;
                    myNetRightHand.GetComponent<MessageActivationH4>().isGrabbing = false;
                    npcAnimationGoing = false;

                    //Debug.Log("entra in release");
                }
            }
        };
    }

    //When I'm grabbing the other user, this function get called and it saves the other user game object
    public void SetGrabbing(GameObject otherNetPlRightHand, string otherPlId)
    {        
        myNetRightHand.GetComponent<MessageActivationH4>().isGrabbing = true;
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

        myNetHead.transform.FindChildRecursive("DeactivateCollider").gameObject.GetComponent<OnCollisionDeactivateCanvasH4>().otherRightHand = otherNetPlRightHand;
    }

    public void SetReleasing(GameObject otherNetPlRightHand, string otherPlId)
    {
        myNetRightHand.GetComponent<MessageActivationH4>().isGrabbing = false;
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
            if (myNetRightHand.GetComponent<MessageActivationH4>().isGrabbing == true)
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
                    if (otherNetRightHand.GetComponent<MessageActivationH4>().isGrabbing == true)
                    {
                        if (firstFrame == true)
                        {
                            //If both users are grabbing handshake animation starts, if one of them release or they get to far they
                            //cannot handshake anymore

                            if (otherGrabbedFirst)
                            {
                                InteractionsCount.finishedInteractionsH4++;
                                finalTimeH4Player = System.DateTime.UtcNow;
                                if (otherNetHead.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color == baseColor)
                                {
                                    NLogConfig.LogLine($"{"White_Version"};TimeFromCanvasAppearing:{(finalTimeH4Player - initialTimeH4Player).TotalMilliseconds.ToString("#.00")} ms");
                                }
                                else if (otherNetHead.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color == yellowColor)
                                {
                                    NLogConfig.LogLine($"{"Yellow_Version"};TimeFromCanvasAppearing:{(finalTimeH4Player - initialTimeH4Player).TotalMilliseconds.ToString("#.00")} ms");
                                }
                                else if (otherNetHead.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color == greenColor)
                                {
                                    NLogConfig.LogLine($"{"Green_Version"};TimeFromCanvasAppearing:{(finalTimeH4Player - initialTimeH4Player).TotalMilliseconds.ToString("#.00")} ms");
                                }
                                otherGrabbedFirst = false;
                            }                            

                            rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
                            rightController.GetComponent<HandController>().isGrabbingH3 = true;
                            myNetRightController.GetComponent<NetworkHandController>().isGrabbingH3 = true;
                            myNetRightMesh = myNetRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                            myNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                            otherNetRightHand.GetComponent<NetworkHand>().SetGrip(0);
                            otherNetRightMesh = otherNetRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                            otherNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                            messageCanvas.GetComponent<Canvas>().enabled = false;
                            this.GetComponent<CollidingH4>().otherNetRightHand = otherNetRightHand;
                            this.GetComponent<CollidingH4>().isGrabbing = true;
                            otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                            otherNetGrabConfirmCanvas.GetComponent<Canvas>().enabled = false;
                            areShaking = true;                            

                            //Haptic, visual and sound feedback are provided during the handshake
                            this.GetComponent<HapticController>().SendHaptics2H3();

                            this.GetComponent<Outline>().enabled = true;
                            otherNetRightHand.GetComponent<Outline>().enabled = true;
                            otherNetRightHand.GetComponent<AudioSource>().Play();
                            firstFrame = false;
                            firstFrameForCount = true;
                            firstFrameForCount2 = true;

                            SaveAndCallActivation(otherNetPlayer, otherNetRightHand);

                            //this.GetComponent<Animator>().Play("Handshake", -1, 0);

                            Invoke("disableOutline", 0.30f);
                        }

                    }
                    else
                    {
                        //If I'm grabbing, but the other user is not, I cannot move my right hand until I release the button or
                        //the other user grab my hand
                        this.GetComponent<CollidingH4>().isGrabbing = true;
                        messageCanvas.GetComponent<Canvas>().enabled = true;
                        if (firstFrameForCount2 == true)
                        {
                            InteractionsCount.startedInteractionsFromTesterH4++;
                            firstFrameForCount2 = false;
                        }
                        rightController.GetComponent<ActionBasedController>().enableInputTracking = false;
                        otherNetGrabMessageCanvas.GetComponent<Canvas>().enabled = false;
                        rightController.GetComponent<HandController>().isGrabbingH3 = true;
                        myNetRightController.GetComponent<NetworkHandController>().isGrabbingH3 = true;

                        areShaking = false;
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
                    if (otherNetRightHand.GetComponent<MessageActivationH4>().isGrabbing == false)
                    {
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
                        firstFrameForCount = true;
                        firstFrameForCount2 = true;
                    }
                    else
                    {
                        if(firstFrameForCount == true)
                        {
                            otherGrabbedFirst = true;
                            initialTimeH4Player = System.DateTime.UtcNow;
                            InteractionsCount.startedInteractionsFromExperimenterH4++;
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
                this.GetComponent<CollidingH4>().isGrabbing = false;
                if(npcAnimationGoing == false)
                {
                    rightController.GetComponent<HandController>().isGrabbingH3 = false;
                }                
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

    private void SaveAndCallActivation(GameObject otherPlayer, GameObject otherPlayerHand)
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if ((object)item.TagObject == otherPlayer)
            {
                //Debug.Log($"{item.UserId}");
                player2ID = item.UserId;
            }
        }
        player1ID = myId;
        int pl1HandId = myNetRightHand.GetComponent<PhotonView>().ViewID;
        int pl2HandId = otherPlayerHand.GetComponent<PhotonView>().ViewID;

        otherPlayer.GetComponent<NetworkGrabMessageActivationH4>().animationGoing = true;
        myNetPlayer.GetComponent<NetworkGrabMessageActivationH4>().animationGoing = true;

        myNetPlayer.GetComponent<NetworkHandshakeActivationH4>().CallActivationOverNetwork(player1ID, player2ID, pl1HandId, pl2HandId);
    }
}

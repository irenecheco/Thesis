using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.InputSystem;
using NLog.Unity;

public class OnButtonAPressed : MonoBehaviour, IPunObservable
{
    //Code responsible for keeping track of the A button pressure: it triggers the animation if both users press it

    public bool isColliding = false;

    public GameObject otherPlayerHead;
    private GameObject otherPlayer;
    private GameObject otherRightHand;
    private GameObject otherHandMesh;
    public GameObject myHead;
    public GameObject myPlayer;

    private GameObject rightHand;
    private GameObject handMesh;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color yellowColor = new Color(0.8679245f, 0.8271183f, 0.4208615f, 1.0f);
    private Color greenColor = new Color(0.4291207f, 0.7924528f, 0.6037189f, 1.0f);

    private string player1ID;
    private string player2ID;

    public bool isPressed;
    private bool firstCall;
    public bool animationGoing;

    PhotonView photonView;

    public System.DateTime initialTimeH2Player;
    private System.DateTime finalTimeH2Player;

    private string handshake2_messageCanva = "Handshake 2 message";

    [SerializeField] private InputActionReference _enableHandshake2;

    void Start()
    {
        myHead = this.gameObject;
        myPlayer = myHead.gameObject.transform.parent.gameObject;
        photonView = myHead.gameObject.transform.GetComponent<PhotonView>();
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        handMesh = rightHand.transform.FindChildRecursive("hands:Lhand").gameObject;

        isPressed = false;
        firstCall = true;
        animationGoing = false;

        //Saving the right controller characteristics to keep track of the A button

        _enableHandshake2.action.started += ctx =>
        {
            if (photonView.IsMine)
            {
                //Debug.Log("Entra, ma non collide");
                //isColling is true whenever my player's head collide with another player's head
                if (isColliding == true)
                {
                    isPressed = true;
                    //Debug.Log("Entra e collide");
                    if (otherPlayerHead != null)
                    {
                        if (otherPlayerHead.GetComponent<AngleOfViewControl>().isLooking)
                        {
                            SaveIds();
                            //Debug.Log($"{isPressed} mio e {otherPlayerHead.transform.GetComponent<OnButtonAPressed>().isPressed} altro");
                            if (otherPlayerHead.transform.GetComponent<OnButtonAPressed>().isPressed)
                            {
                                Debug.Log("Entra qui");
                                if (animationGoing == false)
                                {
                                    animationGoing = true;
                                    myPlayer.GetComponent<NetworkHandshakeActivationH2>().CallActivationOverNetwork(player1ID, player2ID);
                                    InteractionsCount.finishedInteractionsWithExperimenterH2++;
                                    finalTimeH2Player = System.DateTime.UtcNow;
                                    NLogConfig.LogLine($"{"Avatar"};TimeFromCanvasAppearing;{(finalTimeH2Player - initialTimeH2Player).TotalSeconds.ToString("#.000")};s");
                                    /*if (otherPlayerHead.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color == baseColor)
                                    {
                                        NLogConfig.LogLine($"{"White_Version"};TimeFromCanvasAppearing;{(finalTimeH2Player - initialTimeH2Player).TotalSeconds.ToString("#.000")};s");
                                    }
                                    else if (otherPlayerHead.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color == yellowColor)
                                    {
                                        NLogConfig.LogLine($"{"Yellow_Version"};TimeFromCanvasAppearing;{(finalTimeH2Player - initialTimeH2Player).TotalSeconds.ToString("#.000")};s");
                                    }
                                    else if (otherPlayerHead.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color == greenColor)
                                    {
                                        NLogConfig.LogLine($"{"Green_Version"};TimeFromCanvasAppearing;{(finalTimeH2Player - initialTimeH2Player).TotalSeconds.ToString("#.000")};s");
                                    }*/
                                    //Debug.Log("Entrambi gli isPressed sono true");                                    
                                }
                            }
                        }
                    }
                }
            }
        };

        _enableHandshake2.action.performed += ctx =>
        {
            if (photonView.IsMine)
            {
                //isColling is true whenever my player's head collide with another player's head
                if (isColliding == true)
                {
                    isPressed = false;
                    if (otherPlayerHead != null)
                    {                        
                        otherPlayer = otherPlayerHead.transform.parent.gameObject;
                        otherRightHand = otherPlayer.transform.FindChildRecursive("Right Hand").gameObject;
                        otherHandMesh = otherRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                        otherHandMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                        SaveIds();
                    }
                }
            }
        };
    }

    //Method of the Photon Pun library that lets you keep track of a variable through the network (class IPunObservable)
    //In this case it keeps track of two booleans: isPressed is true when A button is pressed, isColliding is true when players
    //are colliding (and they can handshake)
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isColliding);
            stream.SendNext(isPressed);
        }
        else
        {
            this.isColliding = (bool)stream.ReceiveNext();
            this.isPressed = (bool)stream.ReceiveNext();
        }
    }

    //Function to save the ids of the players involved in the handshake and keep track of the A buttons over the network
    public void SaveIds()
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if ((object)item.TagObject == otherPlayer)
            {
               player2ID = item.UserId;
            }
        }

        player1ID = PhotonNetwork.LocalPlayer.UserId;

        if(player1ID!= null && player2ID != null)
        {
            if (isPressed == true)
            {
                //firstCall is a boolean that is true if the previous frame the button was not pressed, so that the method over
                //the network gets called only on the pressure of the button and not every frame that the button is held
                if (firstCall == true)
                {
                    //Debug.Log("Entra in firstcall true");
                    firstCall = false;
                    myHead.transform.GetComponent<NetworkHandshakePressedA>().CallPressedAOverNetwork(player1ID, player2ID);
                }
            }
            else
            {
                firstCall = true;
                myHead.transform.GetComponent<NetworkHandshakePressedA>().CallReleasedAOverNetwork(player1ID, player2ID);
            }
        }
        //ids saved           
    }
}

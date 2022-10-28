using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class OnButtonAPressed : MonoBehaviour, IPunObservable
{
    //Code responsible for keeping track of the A button pressure: it triggers the animation if both users press it

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDeviceCharacteristics rControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
    private InputDevice targetDevice;

    public bool isColliding = false;

    public GameObject otherPlayerHead;
    public GameObject myHead;
    public GameObject myPlayer;

    private string player1ID;
    private string player2ID;

    public bool isPressed;
    private bool previousFramePressure;
    private bool firstCall;
    public bool animationGoing;

    PhotonView photonView;

    void Start()
    {
        myHead = this.gameObject;
        myPlayer = myHead.gameObject.transform.parent.gameObject;
        photonView = myHead.gameObject.transform.GetComponent<PhotonView>();

        isPressed = false;
        previousFramePressure = false;
        firstCall = true;
        animationGoing = false;

        //Saving the right controller characteristics to keep track of the A button
        InputDevices.GetDevicesWithCharacteristics(rControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            //isColling is true whenever my player's head collide with another player's head
            if (isColliding == true)
            {
                //Saving the A button value at every frame
                targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
                
                //If the A button is pressed it saves the information in a bool that it needs to check later and it calls
                //the function that could trigger the handshake
                if (primaryButtonValue)
                {
                    //Debug.Log("pressing button A");
                    if (otherPlayerHead != null)
                    {
                        isPressed = true;
                        if (previousFramePressure == false)
                        {
                            SaveIds();
                        }
                        previousFramePressure = true;
                    }
                }
                else
                {
                    //Debug.Log("releasing button A");
                    isPressed = false;
                    SaveIds();
                }

                //Checks if both users are pressing the A button: if true it calls the animation over the network
                if(otherPlayerHead != null)
                {
                    GameObject otherPlayer = otherPlayerHead.gameObject.transform.parent.gameObject;
                    if(isPressed && otherPlayerHead.transform.GetComponent<OnButtonAPressed>().isPressed)
                    {
                        if(animationGoing == false)
                        {
                            //Debug.Log("Entrambi gli isPressed sono true");
                            animationGoing = true;
                            myPlayer.GetComponent<NetworkHandshakeActivationH2>().CallActivationOverNetwork(player1ID, player2ID);
                        }                        
                    }
                }
            }
        }        
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
        if(otherPlayerHead != null)
        {
            GameObject otherPlayer = otherPlayerHead.transform.parent.gameObject;

            foreach (var item in PhotonNetwork.PlayerList)
            {
                if ((object)item.TagObject == otherPlayer)
                {
                    player2ID = item.UserId;
                }
            }

            player1ID = PhotonNetwork.LocalPlayer.UserId;

            //ids saved

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
                if (previousFramePressure == true)
                {
                    //Debug.Log("Releasing button A");
                    previousFramePressure = false;
                    firstCall = true;
                    myHead.transform.GetComponent<NetworkHandshakePressedA>().CallReleasedAOverNetwork(player1ID, player2ID);
                }
            }
        }        
    }
}

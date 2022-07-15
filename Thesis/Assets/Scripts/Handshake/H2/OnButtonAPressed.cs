using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class OnButtonAPressed : MonoBehaviour, IPunObservable
{
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDeviceCharacteristics rControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
    private InputDevice targetDevice;

    public bool isColliding = false;

    public GameObject otherPlayerHead;
    public GameObject myHead;
    public GameObject myPlayer;

    /*private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_waitingCanva = "Handshake 2 waiting";
    private string handshake2_confirmCanva = "Handshake 2 confirm";*/

    private string player1ID;
    private string player2ID;

    public bool isPressed;
    private bool previousFramePressure;
    private bool firstCall;
    public bool animationGoing;

    //PhotonView colliderPhotonView;
    PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        myHead = this.gameObject;
        myPlayer = myHead.gameObject.transform.parent.gameObject;
        photonView = myHead.gameObject.transform.GetComponent<PhotonView>();

        isPressed = false;
        previousFramePressure = false;
        firstCall = true;
        animationGoing = false;

        InputDevices.GetDevicesWithCharacteristics(rControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (isColliding == true)
            {
                targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
                
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
                    } else
                    {
                       //myPlayer.GetComponent<NetworkHandshakeActivationH2>().SetBackComponent();
                       //otherPlayer.GetComponent<NetworkHandshakeActivationH2>().SetBackComponent();
                    }
                }
            }
        }        
    }

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
            //Debug.Log($"{this.h2_messageActive}");
        }
    }

    public void SaveIds()
    {
        if(otherPlayerHead != null)
        {
            GameObject otherPlayer = otherPlayerHead.transform.parent.gameObject;

            foreach (var item in PhotonNetwork.PlayerList)
            {
                if ((object)item.TagObject == otherPlayer)
                {
                    //Debug.Log($"{item.UserId}");
                    player2ID = item.UserId;
                }
            }

            player1ID = PhotonNetwork.LocalPlayer.UserId;

            if (isPressed == true)
            {
                if (firstCall == true)
                {
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

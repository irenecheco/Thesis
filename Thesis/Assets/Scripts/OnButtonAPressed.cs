using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using Photon.Pun;

public class OnButtonAPressed : MonoBehaviour, IPunObservable
{
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDeviceCharacteristics rControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
    private InputDevice targetDevice;

    private int sceneIndex;

    public bool isColliding = false;

    public GameObject otherPlayerHead;
    public GameObject myHead;

    private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_waitingCanva = "Handshake 2 waiting";
    private string handshake2_confirmCanva = "Handshake 2 confirm";

    private string player1ID;
    private string player2ID;

    private bool isPressed;
    private bool previousFramePressure;

    //PhotonView colliderPhotonView;
    PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        myHead = this.gameObject.transform.parent.gameObject;
        photonView = myHead.gameObject.transform.GetComponent<PhotonView>();

        isPressed = false;
        previousFramePressure = false;

        if (sceneIndex == 2)
        {
            InputDevices.GetDevicesWithCharacteristics(rControllerCharacteristics, devices);

            if (devices.Count > 0)
            {
                targetDevice = devices[0];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneIndex == 2)
        {
            if (photonView.IsMine)
            {
                if (isColliding == true)
                {
                    //Debug.Log("isColliding è davvero true");
                    targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
                    //Debug.Log($"{primaryButtonValue}");
                    if (primaryButtonValue)
                    {
                        //Debug.Log("pressing button A");
                        if (otherPlayerHead != null)
                        {
                            isPressed = true;
                            previousFramePressure = true;
                            SaveIds();

                        }
                        else
                        {
                            //Debug.Log("otherPlayerHead is null");
                        }
                    }
                    else
                    {
                        //Debug.Log("releasing button A");
                        isPressed = false;
                        SaveIds();
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
            //stream.SendNext(isPressed);
        }
        else
        {
            this.isColliding = (bool)stream.ReceiveNext();
            //this.isPressed = (bool)stream.ReceiveNext();
            //Debug.Log($"{this.h2_messageActive}");
        }
    }

    public void SaveIds()
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

        if(isPressed == true)
        {
            myHead.transform.GetComponent<NetworkHandshakePressedA>().CallPressedAOverNetwork(player1ID, player2ID);
        } else
        {
            if(previousFramePressure == true)
            {
                previousFramePressure = false;
                myHead.transform.GetComponent<NetworkHandshakePressedA>().CallReleasedAOverNetwork(player1ID, player2ID);                
            }           
        }
    }
}

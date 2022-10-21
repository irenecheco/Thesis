using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeButton : MonoBehaviour
{
    //Code responsible to trigger the confirm canvas once a user press the handshake button (H1)

    private GameObject player;
    private GameObject myPlayer;
    private GameObject myPlayerHead;
    private GameObject myPlayerConfirm;
    private GameObject handshakeUI;
    private GameObject waitConfirmUI;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject rightController;

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDeviceCharacteristics lControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
    private InputDevice targetDevice;

    public RuntimeAnimatorController mayor_anim_controller;

    void Start()
    {
        if (this.gameObject.name != "NPC_RightHand")
        {
            rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
            rightController = GameObject.Find("Camera Offset/RightHand Controller");

            player = GameObject.Find("Player");
            player.transform.position = GameObject.Find("Camera Offset/RightHand Controller").transform.position;
            if (this.gameObject.name == "Handshake Button")
            {
                handshakeUI = this.gameObject.transform.parent.gameObject;
                leftHand = handshakeUI.transform.parent.gameObject;
                waitConfirmUI = leftHand.transform.GetChild(3).gameObject;
                waitConfirmUI.GetComponent<Canvas>().enabled = false;
            }

            InputDevices.GetDevicesWithCharacteristics(lControllerCharacteristics, devices);

            if (devices.Count > 0)
            {
                targetDevice = devices[0];
            }
        }
    }

    //Checks every frame is the left controller buttons are pressed, if X is pressed handshake button onClick is 
    //invoked
    public void Update()
    {
        if(this.gameObject.name != "NPC_RightHand")
        {
            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);

            if (this.gameObject.name == "Handshake Button")
            {
                if (this.GetComponent<Button>().interactable == true)
                {
                    if (primaryButtonValue)
                    {
                        this.GetComponent<Button>().onClick.Invoke();
                    }
                }
            }
        }
    }

    //Function called on the pressed handshake button: it changes the canvas that the player who pressed the button
    //sees and it activates on his head the confrim canvas
    public void OnHandshakePressed()
    {
        for(int i=0; i<PhotonNetwork.PlayerList.Length; i++)
        {
            foreach(var item in PhotonNetwork.PlayerList)
            {
                if (item.IsLocal)
                {
                    myPlayer = (GameObject)item.TagObject;
                }
            }

            if(myPlayer.GetComponent<PhotonView>().IsMine)
            {
                break;
            }
        }
        myPlayerHead = myPlayer.transform.GetChild(0).gameObject;
        myPlayerConfirm = myPlayerHead.transform.GetChild(0).gameObject;
        myPlayerConfirm.GetComponent<HandshakeConfirmCanvas>().ActivateHandshakeConfirmCanvas();
        if (this.gameObject.name == "Handshake Button")
        {
            handshakeUI.GetComponent<Canvas>().enabled = false;
            waitConfirmUI.GetComponent<Canvas>().enabled = true;
        }
    }

    //Called at the hand of the animation: it sets backe the parent and the components
    public void SetBackComponent()
    {
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<HandController>();
        rightController.GetComponent<HandController>().hand = rightHand.GetComponent<Hand>();
        rightHand.GetComponent<Hand>().flag = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HandshakeButton : MonoBehaviour
{
    //Code responsible to trigger the confirm canvas once a user press the handshake button (H1)

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject waitress;
    [SerializeField] private GameObject mayor_head;

    private GameObject myPlayer;
    private GameObject myPlayerHead;
    private GameObject myPlayerConfirm;
    private GameObject handshakeUI;
    private GameObject waitConfirmUI;
    private GameObject hand;

    /*private List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
    private InputDeviceCharacteristics lControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
    private UnityEngine.XR.InputDevice targetDevice;*/

    //public RuntimeAnimatorController mayor_anim_controller;

    public bool isCollidingWithWaitress;
    public bool firstHandshake;
    public GameObject collidingPlayerHead;

    [SerializeField] private InputActionReference _enableHandshake;
    private Button _button;
    private Canvas _canvas;

    void Start()
    {
        isCollidingWithWaitress = false;
        firstHandshake = true;

        if(player != null)
        {
            player.transform.position = rightController.transform.position;
        }        
        handshakeUI = this.gameObject.transform.parent.gameObject;
        hand = handshakeUI.transform.parent.gameObject;
        waitConfirmUI = hand.transform.GetChild(3).gameObject;
        waitConfirmUI.GetComponent<Canvas>().enabled = false;

        /*InputDevices.GetDevicesWithCharacteristics(lControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }*/

        _button = this.GetComponent<Button>();
        _canvas = handshakeUI.GetComponent<Canvas>();

        //When primary button in pressed, if user is colliding with waitress or player, it starts the handshake

        _enableHandshake.action.performed += ctx => {
            if(_canvas!= null)
            {
                if (_canvas.enabled == true)
                {
                    if (collidingPlayerHead != null)
                    {
                        GameObject confirmCanvas = collidingPlayerHead.transform.GetChild(0).gameObject;
                        //Debug.Log($"Entra qui e confirm canvas è {confirmCanvas.GetComponent<HandshakeConfirmCanvas>().confirmActive}");
                        if (confirmCanvas.GetComponent<HandshakeConfirmCanvas>().confirmActive == true)
                        {
                            confirmCanvas.transform.GetChild(2).gameObject.GetComponent<HandshakeActivation>().CallHeadMethod();
                        }
                        else
                        {
                            _button.onClick.Invoke();
                        }
                    }
                    else
                    {
                        _button.onClick.Invoke();
                    }
                }
                /*if (mayor_head.GetComponent<MayorConfirmCanvas>().activeCanvas == true)
                {
                    mayor_head.transform.FindChildRecursive("HandshakeConfirm Button").GetComponent<HandshakeActivationNPC>().StartHandshake();
                }*/
            }
        };
    }

    //Function called on the pressed handshake button: it changes the canvas that the player who pressed the button
    //sees and it activates on his head the confirm canvas
    public void OnHandshakePressed()
    {
        if(isCollidingWithWaitress == false)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.IsLocal)
                    {
                        myPlayer = (GameObject)item.TagObject;
                    }
                }

                if (myPlayer.GetComponent<PhotonView>().IsMine)
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
        else
        {
            if(firstHandshake == true)
            {
                waitress.GetComponent<HandshakeActivationNPC>().StartHandshake();
                firstHandshake = false;
                handshakeUI.GetComponent<Canvas>().enabled = false;
            }            
        }
    }
}

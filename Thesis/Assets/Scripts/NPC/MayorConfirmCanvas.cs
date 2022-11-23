using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class MayorConfirmCanvas : MonoBehaviour
{
    public AudioClip mayor_speech1;
    public AudioClip mayor_speech2;

    private int sceneIndex;

    private GameObject mayor_head;
    private GameObject mayor_head_canvas;
    private GameObject mayor_confirm_button;
    private GameObject mayor_left;
    private GameObject mayor_right;
    private GameObject mayor_rightMesh;
    private GameObject mayor;
    private GameObject mayor_hand_holder;

    private Animator animator_mayor_head;
    private Animator animator_mayor_left;
    private Animator animator_mayor_right;

    private Vector3 initial_hand_holder_position;
    private Quaternion initial_hand_holder_rotation;
    public bool activeCanvas;
    public bool isColliding;
    public bool firstHandshake;

    [SerializeField] private InputActionReference _enableHandshake2;

    private Color waitingColor = new Color(0.4135279f, 0.7409829f, 0.9056604f, 1.0f);

    /*private List<InputDevice> devices = new List<InputDevice>();
    private InputDeviceCharacteristics rControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
    private InputDevice targetDevice;*/

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        activeCanvas = false;
        isColliding = false;
        firstHandshake = true;

        if (this.gameObject.name != "NPC_RightHand")
        {
            mayor_head = this.gameObject;
            mayor_head_canvas = mayor_head.transform.GetChild(0).gameObject;
            if(sceneIndex == 1)
            {
                mayor_confirm_button = mayor_head_canvas.transform.GetChild(2).gameObject;
            }
            
            mayor = mayor_head.transform.parent.gameObject;
            mayor_left = mayor.transform.GetChild(1).gameObject;
            mayor_hand_holder = mayor.transform.GetChild(2).gameObject;
            mayor_right = mayor_hand_holder.transform.GetChild(0).gameObject;
            animator_mayor_head = mayor_head.GetComponent<Animator>();
            animator_mayor_left = mayor_left.GetComponent<Animator>();
            animator_mayor_right = mayor_right.GetComponent<Animator>();
        } else
        {
            mayor_right = this.gameObject;
            mayor_hand_holder = mayor_right.transform.parent.gameObject;
            mayor = mayor_hand_holder.transform.parent.gameObject;
            mayor_left = mayor.transform.GetChild(1).gameObject;
            mayor_head = mayor.transform.GetChild(0).gameObject;

            animator_mayor_head = mayor_head.GetComponent<Animator>();
            animator_mayor_left = mayor_left.GetComponent<Animator>();
            animator_mayor_right = mayor_right.GetComponent<Animator>();
        }
        initial_hand_holder_position = mayor_hand_holder.transform.position;
        initial_hand_holder_rotation = mayor_hand_holder.transform.rotation;
        mayor_rightMesh = mayor_right.transform.FindChildRecursive("hands:Lhand").gameObject;


        if (sceneIndex == 2)
        {
            /*InputDevices.GetDevicesWithCharacteristics(rControllerCharacteristics, devices);

            if (devices.Count > 0)
            {
                targetDevice = devices[0];
            }*/

            _enableHandshake2.action.started += ctx =>
            {
                if (isColliding == true)
                {
                    if (this.GetComponent<AngleOfViewControl>().isLooking)
                    {
                        if (activeCanvas == true)
                        {
                            if (firstHandshake == true)
                            {
                                mayor_head_canvas.GetComponent<HandshakeActivationNPC>().StartHandshake();
                                firstHandshake = false;
                            }
                        }
                    }                    
                }
            };
        }

        if(sceneIndex == 3 || sceneIndex == 4)
        {
            mayor_right.GetComponent<XRGrabInteractable>().enabled = false;
        }
    }

    private void Update()
    {
        if(isColliding == true)
        {
            /*if (sceneIndex == 2)
            {
                if (activeCanvas == true)
                {
                    if(firstHandshake == true)
                    {
                        //Check when user press A button to start handshake
                        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);

                        if (primaryButtonValue)
                        {
                            mayor_head_canvas.GetComponent<HandshakeActivationNPC>().StartHandshake();
                            firstHandshake = false;
                        }
                    }
                }
            } else*/ 
            if(sceneIndex == 3 || sceneIndex == 4)
            {
                if(firstHandshake == true)
                {
                    mayor_right.GetComponent<XRGrabInteractable>().enabled = true;
                    firstHandshake = false;
                }
            }
        }
    }

    public void activateMayorCanvas()
    {
        if (sceneIndex == 1)
        {
            mayor_confirm_button.GetComponent<Button>().interactable = true;
        }
        if(sceneIndex != 3 && sceneIndex != 4)
        {
            mayor_head_canvas.GetComponent<Canvas>().enabled = true;
            mayor_head_canvas.GetComponent<AudioSource>().Play();
            mayor_rightMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
        }
        else
        {
            if(sceneIndex == 3)
            {
                mayor_right.GetComponent<GrabbingNPC>().initialPosition = mayor_right.transform.localPosition;
                mayor_hand_holder.GetComponent<ActivateBlueHand>().isReady = true;
                if (mayor_hand_holder.GetComponent<ActivateBlueHand>().isCollidingWithMayor)
                {
                    mayor_rightMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
                    mayor_head_canvas.GetComponent<Canvas>().enabled = true;
                }
            } else if(sceneIndex == 4)
            {
                mayor_right.GetComponent<GrabbingNPC>().initialPosition = mayor_right.transform.localPosition;
                mayor_hand_holder.GetComponent<ActivateBlueHandH4>().isReady = true;
                if (mayor_hand_holder.GetComponent<ActivateBlueHandH4>().isCollidingWithMayor)
                {
                    mayor_rightMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
                    mayor_head_canvas.GetComponent<Canvas>().enabled = true;
                }
            }
            
        }
        animator_mayor_head.speed = 0;
        animator_mayor_left.speed = 0;
        animator_mayor_right.speed = 0;
        activeCanvas = true;
    }

    public void secondSpeech()
    {
        activeCanvas = false;
        mayor_hand_holder.transform.position = initial_hand_holder_position;
        mayor_hand_holder.transform.rotation = initial_hand_holder_rotation;
        animator_mayor_right.speed = 1;
        animator_mayor_left.speed = 1;
        animator_mayor_head.speed = 1;
        if(sceneIndex == 3)
        {
            animator_mayor_right.speed = 1;
            mayor_hand_holder.GetComponent<ActivateBlueHand>().isReady = false;
        } else if(sceneIndex == 4)
        {
            animator_mayor_right.speed = 1;
            mayor_hand_holder.GetComponent<ActivateBlueHandH4>().isReady = false;
        }
        mayor_head.GetComponent<AudioSource>().clip = mayor_speech2;
        mayor_head.GetComponent<AudioSource>().Play();
        animator_mayor_head.Play("MayorSpeech2_head");
        animator_mayor_right.Play("MayorSpeech2_right");
        animator_mayor_left.Play("MayorSpeech2_left");
    }

    /*public void backToIdle()
    {
        animator_mayor_left.SetBool("Default", true);
        animator_mayor_head.SetBool("Default", true);
        animator_mayor_right.SetBool("Default", true);
    }*/
}

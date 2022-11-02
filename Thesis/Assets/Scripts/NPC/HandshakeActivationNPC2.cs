using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandshakeActivationNPC2 : MonoBehaviour
{
    //Code responsible for NPC handshake activation in H1 and H2

    private int sceneIndex;

    private GameObject npcHead;
    private GameObject npc;
    private GameObject npcLeft;
    private GameObject npcRight;
    private GameObject npcHandHolder;
    private GameObject npcMessage;

    private GameObject rightHand;
    private GameObject leftHand;
    private GameObject rightController;
    private GameObject leftController;
    private GameObject player;
    private GameObject camera;

    private Animator rightHandAnimator;
    private Animator animator_NPC_head;
    private Animator animator_NPC_left;
    private Animator animator_NPC_right;

    private Vector3 direction;
    private Vector3 direction2;
    private float y_angle;
    private float y_angle2;

    private Vector3 initial_hand_holder_position;
    private Quaternion initial_hand_holder_rotation;

    public bool isCollidingWithWaitress;
    public bool firstHandshake;

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDeviceCharacteristics rControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
    private InputDevice targetDevice;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        isCollidingWithWaitress = false;
        firstHandshake = true;

        if (this.gameObject.name != "NPC_RightHand")
        {
            npc = this.gameObject;
            npcHead = npc.transform.GetChild(0).gameObject;
            npcLeft = npc.transform.GetChild(1).gameObject;
            npcHandHolder = npc.transform.GetChild(2).gameObject;
            npcRight = npcHandHolder.transform.GetChild(0).gameObject;
        }
        else
        {
            npcRight = this.gameObject;
            npcHandHolder = npcRight.transform.parent.gameObject;
            npc = npcHandHolder.transform.parent.gameObject;
            npcHead = npc.transform.GetChild(0).gameObject;
            npcLeft = npc.transform.GetChild(1).gameObject;
        }

        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        leftController = GameObject.Find("Camera Offset/LefttHand Controller");
        leftHand = GameObject.Find("Camera Offset/LeftHand Controller/LeftHand");
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        player = GameObject.Find("Player");
        camera = GameObject.Find("Camera Offset/Main Camera");
        rightHandAnimator = rightHand.GetComponent<Animator>();

        animator_NPC_head = npcHead.GetComponent<Animator>();
        animator_NPC_left = npcLeft.GetComponent<Animator>();
        animator_NPC_right = npcRight.GetComponent<Animator>();

        initial_hand_holder_position = npcHandHolder.transform.position;
        initial_hand_holder_rotation = npcHandHolder.transform.rotation;

        if (sceneIndex == 2)
        {
            npcMessage = npcHead.transform.GetChild(0).gameObject;

            InputDevices.GetDevicesWithCharacteristics(rControllerCharacteristics, devices);

            if (devices.Count > 0)
            {
                targetDevice = devices[0];
            }
        }
    }

    private void Update()
    {
        if(sceneIndex == 2)
        {
            if (isCollidingWithWaitress == true)
            {
                if (firstHandshake == true)
                {
                    //Check when user press A button to start handshake
                    targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);

                    if (primaryButtonValue)
                    {
                        StartHandshake();
                        firstHandshake = false;
                    }
                }
            }
        }        
    }

    //Function called when handshake button pressed: it calls the method to activate the animation
    public void StartHandshake()
    {
        StartCoroutine(Wait());
    }

    //Coroutine that trigger the animation on the player
    public IEnumerator Wait()
    {
        double time = 0.25;
        yield return new WaitForSeconds((float)time);
        float starting_y = 0;
        Vector3 midPosition;

        if (sceneIndex == 2)
        {
            npcMessage.GetComponent<Canvas>().enabled = false;
        }

        if (camera.transform.position.y <= npcHead.transform.position.y)
        {
            starting_y = camera.transform.position.y;
        }
        else
        {
            starting_y = npcHead.transform.position.y;
        }

        Destroy(rightController.GetComponent("HandController"));
        rightHand.GetComponent<Hand>().flag = true;
        rightHand.transform.parent = player.transform;
        midPosition = Vector3.Lerp(npcHead.transform.position, camera.transform.position, 0.5f);
        player.transform.position = new Vector3(midPosition.x, (float)(starting_y - 0.4), midPosition.z);
        npcHandHolder.transform.position = new Vector3(midPosition.x, (float)(starting_y - 0.4), midPosition.z);
        direction = (npcHead.transform.position - camera.transform.position).normalized;
        direction2 = (camera.transform.position - npcHead.transform.position).normalized;

        y_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        y_angle2 = Mathf.Atan2(direction2.x, direction2.z) * Mathf.Rad2Deg;

        float camera_y_angle = camera.transform.rotation.eulerAngles.y;
        float NPCHead_y_angle = npcHead.transform.rotation.eulerAngles.y;

        if (y_angle < 0)
        {
            float offset = -y_angle;
            y_angle = 360 - offset;
        }
        if (camera_y_angle < 0)
        {
            float offset = camera_y_angle;
            camera_y_angle = 360 - offset;
        }
        if ((y_angle - 90) < camera_y_angle && camera_y_angle < (y_angle + 90))
        {
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
            player.transform.rotation = Quaternion.Euler(0, y_angle, 0);
            player.transform.Translate(new Vector3((float)(-0.008), 0, (float)(-0.5)), Space.Self);
        }
        else
        {
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
            player.transform.rotation = Quaternion.Euler(0, (y_angle - 180), 0);
            player.transform.Translate(new Vector3((float)(+0.008), 0, (float)(+0.5)), Space.Self);
        }

        if (y_angle2 < 0)
        {
            float offset = -y_angle2;
            y_angle2 = 360 - offset;
        }
        if (NPCHead_y_angle < 0)
        {
            float offset = NPCHead_y_angle;
            NPCHead_y_angle = 360 - offset;
        }

        if ((y_angle2 - 90) < NPCHead_y_angle && NPCHead_y_angle < (y_angle2 + 90))
        {
            npcHandHolder.transform.rotation = new Quaternion(0, 0, 0, 0);
            npcHandHolder.transform.rotation = Quaternion.Euler(0, y_angle2, 0);
            npcHandHolder.transform.Translate(new Vector3((float)(-0.035), 0, (float)(-0.540)), Space.Self);
        }
        else
        {
            npcHandHolder.transform.rotation = new Quaternion(0, 0, 0, 0);
            npcHandHolder.transform.rotation = Quaternion.Euler(0, (y_angle2 - 180), 0);
            npcHandHolder.transform.Translate(new Vector3((float)(+0.035), 0, (float)(+0.540)), Space.Self);
        }

        rightHandAnimator.Play("Handshake", -1, 0);
        animator_NPC_right.Play("Waitress_handshake", 0, 0);
        animator_NPC_head.Play("Waitress_handshake_head", 0, 0);
        animator_NPC_left.Play("Waitress_handshake_left", 0, 0);
        
        if(sceneIndex == 1)
        {            
            GameObject canvas;
            canvas = leftHand.transform.GetChild(2).gameObject;
            canvas.GetComponent<Canvas>().enabled = false;
        }
    }

    public void secondSpeech()
    {
        if(sceneIndex != 3)
        {
            npcHandHolder.transform.position = initial_hand_holder_position;
            npcHandHolder.transform.rotation = initial_hand_holder_rotation;
        }        
        npcHead.GetComponent<AudioSource>().Play();
        if(npc.gameObject.name == "Waitress")
        {
            animator_NPC_head.Play("WaitressSpeech_head");
            animator_NPC_right.Play("WaitressSpeech_right");
            animator_NPC_left.Play("WaitressSpeech_left");
        }        
    }
}

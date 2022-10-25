using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class HandshakeActivationNPC : MonoBehaviour
{
    //Code responsible for NPC handshake activation in H1 and H2

    private int sceneIndex;

    private GameObject confirmCanvas;
    private GameObject confirmHead;
    private GameObject confirmNPC;
    private GameObject confirmLeft;
    private GameObject confirmRight;
    private GameObject confirmNPCHandHolder;

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

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex == 1)
        {
            confirmCanvas = this.gameObject.transform.parent.gameObject;
            confirmHead = confirmCanvas.transform.parent.gameObject;
            confirmNPC = confirmHead.transform.parent.gameObject;
            confirmLeft = confirmNPC.transform.GetChild(1).gameObject;
            confirmNPCHandHolder = confirmNPC.transform.GetChild(2).gameObject;
            confirmRight = confirmNPCHandHolder.transform.GetChild(0).gameObject;

        } else if(sceneIndex == 2)
        {
            confirmCanvas = this.gameObject;
            confirmHead = confirmCanvas.transform.parent.gameObject;
            confirmNPC = confirmHead.transform.parent.gameObject;
            confirmLeft = confirmNPC.transform.GetChild(1).gameObject;
            confirmNPCHandHolder = confirmNPC.transform.GetChild(2).gameObject;
            confirmRight = confirmNPCHandHolder.transform.GetChild(0).gameObject;
        }        

        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        leftController = GameObject.Find("Camera Offset/LefttHand Controller");
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        player = GameObject.Find("Player");
        camera = GameObject.Find("Camera Offset/Main Camera");
        rightHandAnimator = rightHand.GetComponent<Animator>();

        animator_NPC_head = confirmHead.GetComponent<Animator>();
        animator_NPC_left = confirmLeft.GetComponent<Animator>();
        animator_NPC_right = confirmRight.GetComponent<Animator>();
    }

    //Function called when confirm button pressed: it saves the ids and call the methed to activate the animation over the network
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

        if (camera.transform.position.y <= confirmHead.transform.position.y)
        {
            starting_y = camera.transform.position.y;
        }
        else
        {
            starting_y = confirmHead.transform.position.y;
        }

        Destroy(rightController.GetComponent("HandController"));
        rightHand.GetComponent<Hand>().flag = true;
        rightHand.transform.parent = player.transform;
        midPosition = Vector3.Lerp(confirmHead.transform.position, camera.transform.position, 0.5f);
        player.transform.position = new Vector3(midPosition.x, (float)(starting_y - 0.4), midPosition.z);
        confirmNPCHandHolder.transform.position = new Vector3(midPosition.x, (float)(starting_y - 0.4), midPosition.z);
        direction = (confirmHead.transform.position - camera.transform.position).normalized;
        direction2 = (camera.transform.position - confirmHead.transform.position).normalized;

        y_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        y_angle2 = Mathf.Atan2(direction2.x, direction2.z) * Mathf.Rad2Deg;

        float camera_y_angle = camera.transform.rotation.eulerAngles.y;
        float NPCHead_y_angle = confirmHead.transform.rotation.eulerAngles.y;

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
            player.transform.Translate(new Vector3((float)(-0.026), 0, (float)(-0.540)), Space.Self);
        }
        else
        {
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
            player.transform.rotation = Quaternion.Euler(0, (y_angle - 180), 0);
            player.transform.Translate(new Vector3((float)(+0.026), 0, (float)(+0.540)), Space.Self);
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
            confirmNPCHandHolder.transform.rotation = new Quaternion(0, 0, 0, 0);
            confirmNPCHandHolder.transform.rotation = Quaternion.Euler(0, y_angle2, 0);
            confirmNPCHandHolder.transform.Translate(new Vector3((float)(-0.026), 0, (float)(-0.540)), Space.Self);
        }
        else
        {
            confirmNPCHandHolder.transform.rotation = new Quaternion(0, 0, 0, 0);
            confirmNPCHandHolder.transform.rotation = Quaternion.Euler(0, (y_angle2 - 180), 0);
            confirmNPCHandHolder.transform.Translate(new Vector3((float)(+0.026), 0, (float)(+0.540)), Space.Self);
        }

        rightHandAnimator.Play("Handshake", -1, 0);
        animator_NPC_right.speed = 1;
        animator_NPC_right.Play("Mayor_handshake", 0, 0);

        confirmCanvas.GetComponent<Canvas>().enabled = false;
    }
}

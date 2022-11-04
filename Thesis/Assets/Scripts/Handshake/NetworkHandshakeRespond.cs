using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkHandshakeRespond : MonoBehaviour
{
    //Code responsible for the start of the handshake animation in H1 and H2

    private GameObject rightHand;
    private GameObject leftHand;
    private GameObject rightController;
    private GameObject leftController;
    private GameObject networkPlayer;
    private GameObject player;
    private GameObject head;
    private Vector3 cameraPosition;
    private Vector3 otherRightContr;
    float y_angle;
    private Vector3 direction;
    private Animator rightHandAnimator;

    private int sceneIndex;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        rightHand = this.gameObject;
        rightController = rightHand.transform.parent.gameObject;
        networkPlayer = rightController.transform.parent.gameObject;
        leftController = networkPlayer.transform.GetChild(1).gameObject;
        leftHand = leftController.transform.GetChild(0).gameObject;
        player = networkPlayer.transform.GetChild(3).gameObject;
        head = networkPlayer.transform.GetChild(0).gameObject;
        rightHandAnimator = rightHand.GetComponent<Animator>();
        player.transform.position = rightController.transform.position;
    }

    //Function that saves the camera and hand position (so that the animation starts in the right position) and start
    //the coroutine for the animation
    public void OnHandshakePressed(Vector3 camPosition, Vector3 otherRight)
    {
        cameraPosition = camPosition;
        otherRightContr = otherRight;
        StartCoroutine(Wait());
    }

    //Called at the end of the animation to set back the right object's parent and components
    public void SetBackComponent()
    {
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<NetworkHandController>();
        rightController.GetComponent<NetworkHandController>().rightHand_hand = rightHand.GetComponent<NetworkHand>();
        leftController.GetComponent<NetworkHandController>().leftHand_hand = leftHand.GetComponent<NetworkHand>();
        rightHand.GetComponent<NetworkHand>().flag = false;
    }

    //Coroutine used to start the animation
    public IEnumerator Wait()
    {
        double time = 0.25;
        yield return new WaitForSeconds((float)time);
        float starting_y = 0;
        Vector3 midPosition;

        //Check to understand the direction of the players
        if (cameraPosition.y <= head.transform.position.y)
        {
            starting_y = cameraPosition.y;
        }
        else
        {
            starting_y = head.transform.position.y;
        }

        //To set the right direction to the animation the right hand has to be detached from the controller and set to a
        //fixed position and rotation
        Destroy(rightController.GetComponent("NetworkHandController"));
        rightHand.GetComponent<NetworkHand>().flag = true;
        rightHand.transform.parent = player.transform;
        midPosition = Vector3.Lerp(head.transform.position, cameraPosition,  0.5f);
        player.transform.position = new Vector3(midPosition.x, (float)(starting_y - 0.4), midPosition.z);
        direction = (cameraPosition - head.transform.position).normalized;

        y_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float head_y_angle = head.transform.rotation.eulerAngles.y;
        if (y_angle < 0)
        {
            float offset = -y_angle;
            y_angle = 360 - offset;
        }
        if (head_y_angle < 0)
        {
            float offset = head_y_angle;
            head_y_angle = 360 - offset;
        }
        if ((y_angle - 90) < head_y_angle && head_y_angle < (y_angle + 90))
        {
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
            player.transform.rotation = Quaternion.Euler(0, y_angle, 0);
            player.transform.Translate(new Vector3((float)(-0.01), 0, (float)(-0.51)), Space.Self);
        }
        else
        {
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
            player.transform.rotation = Quaternion.Euler(0, (y_angle - 180), 0);
            player.transform.Translate(new Vector3((float)(+0.01), 0, (float)(+0.51)), Space.Self);
        }

        //Depending on the scene (H1 or H2) the animation trigger different things, hence two different animations
        if(sceneIndex == 1)
        {
            rightHandAnimator.Play("Handshake", -1, 0);
        } else if (sceneIndex == 2)
        {
            rightHandAnimator.Play("Handshake2", -1, 0);
        }
        
    }
}

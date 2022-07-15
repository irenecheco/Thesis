using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkHandshakeRespond : MonoBehaviour
{
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
    //float x_angle;
    //float z_angle;
    private Vector3 direction;
    //private GameObject otherPlayer;
    private Animator rightHandAnimator;
    //private Animation handshakeAnimation;
    //private AnimatorStateInfo animStateInfo;
    //private Vector3 rHandPosition;
    //rivate Vector3 lHandPosition;

    private int sceneIndex;

    // Start is called before the first frame update
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

    public void OnHandshakePressed(Vector3 camPosition, Vector3 otherRight)
    {
        cameraPosition = camPosition;
        otherRightContr = otherRight;
        StartCoroutine(Wait());
    }

    public void SetBackComponent()
    {
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<NetworkHandController>();
        rightController.GetComponent<NetworkHandController>().rightHand_hand = rightHand.GetComponent<NetworkHand>();
        leftController.GetComponent<NetworkHandController>().leftHand_hand = leftHand.GetComponent<NetworkHand>();
        rightHand.GetComponent<NetworkHand>().flag = false;
    }

    public IEnumerator Wait()
    {
        double time = 0.25;
        yield return new WaitForSeconds((float)time);
        float starting_y = 0;
        Vector3 midPosition;

        if (cameraPosition.y <= head.transform.position.y)
        {
            starting_y = cameraPosition.y;
        }
        else
        {
            starting_y = head.transform.position.y;
        }

        Destroy(rightController.GetComponent("NetworkHandController"));
        rightHand.GetComponent<NetworkHand>().flag = true;
        rightHand.transform.parent = player.transform;
        midPosition = Vector3.Lerp(head.transform.position, cameraPosition,  0.5f);
        player.transform.position = new Vector3(midPosition.x, (float)(starting_y - 0.4), midPosition.z);
        //player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, (float) (player.transform.position.z - 0.516));
        direction = (cameraPosition - head.transform.position).normalized;

        y_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //Debug.Log($"y direction {y_angle}");
        //Debug.Log($" camera {head.transform.rotation.eulerAngles.y}");
        //x_angle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
        //z_angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
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
            player.transform.Translate(new Vector3((float)(-0.026), 0, (float)(-0.540)), Space.Self);
        }
        else
        {
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
            player.transform.rotation = Quaternion.Euler(0, (y_angle - 180), 0);
            player.transform.Translate(new Vector3((float)(+0.026), 0, (float)(+0.540)), Space.Self);
        }
        if(sceneIndex == 1)
        {
            rightHandAnimator.Play("Handshake", -1, 0);
        } else if (sceneIndex == 2)
        {
            rightHandAnimator.Play("Handshake2", -1, 0);
        }
        
    }
}

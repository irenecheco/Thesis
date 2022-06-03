using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public IEnumerator Wait()
    {
        double time = 0.25;
        yield return new WaitForSeconds((float)time);
        //Vector3 rPos;

        Destroy(rightController.GetComponent("NetworkHandController"));
        rightHand.GetComponent<NetworkHand>().gripCurrent = 0;
        rightHand.GetComponent<NetworkHand>().triggerCurrent = 0;
        rightHand.transform.parent = player.transform;
        /*rPos = rightController.transform.position;
        player.transform.position = rPos;
        player.transform.position = Vector3.Lerp(otherRightContr, rPos,  0.5f);*/
        player.transform.position = new Vector3(head.transform.position.x, (float)(head.transform.position.y - 0.4), head.transform.position.z);
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
        }
        else
        {
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
            player.transform.rotation = Quaternion.Euler(0, (y_angle - 180), 0);
        }

        rightHandAnimator.Play("Handshake", -1, 0);
    }
}

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
        /*otherPlayer = camera.GetComponent<OnCollisionActivateButton>().otherPlayerHead;
        //Debug.Log($"{otherPlayer.name}");
        if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
        {
            GameObject netPlayer = otherPlayer.transform.parent.gameObject;
            GameObject rHandContainer = netPlayer.transform.GetChild(2).gameObject;
            GameObject rHand = rHandContainer.transform.GetChild(0).gameObject;
            rHand.GetComponent<HandshakeButton>().StartCoroutine(Wait());
            //Debug.Log($"{netPlayer} is the parent");
            //netPlayer.GetComponent<NetworkPlayer>().ActivateHandshakeConfirm();
        }*/
        cameraPosition = camPosition;
        otherRightContr = otherRight;
        StartCoroutine(Wait());
    }

    public void SetBackComponent()
    {
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<NetworkHandController>();
        rightController.GetComponent<NetworkHandController>().rightHand_hand = rightHand.GetComponent<NetworkHand>();
        rightController.GetComponent<NetworkHandController>().leftHand_hand = leftHand.GetComponent<NetworkHand>();
    }

    public IEnumerator Wait()
    {
        double time = 0.25;
        yield return new WaitForSeconds((float)time);
        Vector3 rPos;

        Destroy(rightController.GetComponent("NetworkHandController"));
        rightHand.transform.parent = player.transform;
        rPos = rightController.transform.position;
        player.transform.position = rPos;
        player.transform.position = Vector3.Lerp(rPos, otherRightContr, 0.5f);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, (float) (player.transform.position.z + 0.516));
        direction = head.transform.position - cameraPosition;
        y_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //x_angle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
        //z_angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        player.transform.rotation = Quaternion.Euler(0, (y_angle-180), 0);

        rightHandAnimator.Play("Handshake", -1, 0);
    }
}

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
    float angle;
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

    public void OnHandshakePressed(Vector3 position)
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
        cameraPosition = position;
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

        Destroy(rightController.GetComponent("NetworkHandController"));
        rightHand.transform.parent = player.transform;
        player.transform.position = rightController.transform.position;
        direction = head.transform.position - cameraPosition;
        angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        player.transform.rotation = Quaternion.Euler(0, (angle-180), 0);

        rightHandAnimator.Play("Handshake", -1, 0);
    }
}

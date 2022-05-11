using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandshakeButton : MonoBehaviour
{
    private GameObject rightHand;
    private GameObject rightController;
    private GameObject player;
    private GameObject camera;
    private GameObject otherPlayer;
    private Animator rightHandAnimator;
    private Animation handshakeAnimation;
    private AnimatorStateInfo animStateInfo;
    private Vector3 rHandPosition;
    private Vector3 lHandPosition;

    // Start is called before the first frame update
    void Start()
    {
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        player = GameObject.Find("Player");
        camera = GameObject.Find("Camera Offset/Main Camera");
        rightHandAnimator = rightHand.GetComponent<Animator>();
        handshakeAnimation = rightHand.GetComponent<Animation>();
        player.transform.position = GameObject.Find("Camera Offset/RightHand Controller").transform.position;
    }

    public void OnHandshakePressed()
    {

        otherPlayer = camera.GetComponent<OnCollisionActivateButton>().otherPlayerHead;
        //Debug.Log($"{otherPlayer.name}");
        if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
        {
            GameObject netPlayer;
            netPlayer = otherPlayer.transform.parent.gameObject;
            netPlayer.GetComponent<NetworkPlayer>().activateHandshakeConfirm();
        }
        StartCoroutine(Wait());
    }

    public void SetBackComponent()
    {
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<HandController>();
        rightController.GetComponent<HandController>().hand = rightHand.GetComponent<Hand>();
    }

    public IEnumerator Wait()
    {
        float angle;
        double time = 0.25;
        yield return new WaitForSeconds((float)time);
            
        Destroy(rightController.GetComponent("HandController"));
        rightHand.transform.parent = player.transform;
        player.transform.position = rightController.transform.position;
        angle = camera.transform.eulerAngles.y;
        player.transform.rotation = Quaternion.Euler(0, angle, 0);

        rightHandAnimator.Play("Handshake", -1, 0);
    }
}

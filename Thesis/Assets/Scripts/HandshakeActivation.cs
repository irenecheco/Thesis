using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandshakeActivation : MonoBehaviour
{
    private GameObject rightHand;
    private GameObject leftHand;
    private GameObject rightController;
    private GameObject player;
    private GameObject camera;
    private GameObject netPlayer;
    private GameObject rHandContainer;
    private GameObject rHand;
    private GameObject otherPlayer;
    private GameObject confirmCanvas;
    private GameObject waitConfirmUI;
    private GameObject handshakeUI;
    private GameObject myHead;

    private Animator rightHandAnimator;

    private Vector3 direction;

    private float y_angle;

    // Start is called before the first frame update
    void Start()
    {
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        leftHand = GameObject.Find("Camera Offset/LeftHand Controller/LeftHand");
        waitConfirmUI = leftHand.transform.GetChild(3).gameObject;
        handshakeUI = leftHand.transform.GetChild(2).gameObject;
        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        player = GameObject.Find("Player");
        camera = GameObject.Find("Camera Offset/Main Camera");
        rightHandAnimator = rightHand.GetComponent<Animator>();
        //handshakeAnimation = rightHand.GetComponent<Animation>();
        player.transform.position = GameObject.Find("Camera Offset/RightHand Controller").transform.position;
        confirmCanvas = this.gameObject.transform.parent.gameObject;
        myHead = confirmCanvas.transform.parent.gameObject;

    }

    public void ActivateHandshake()
    {
        if (!myHead.GetComponent<PhotonView>().Owner.IsLocal)
        {
            otherPlayer = camera.GetComponent<OnCollisionActivateButton>().otherPlayerHead;
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                netPlayer = otherPlayer.transform.parent.gameObject;
                rHandContainer = netPlayer.transform.GetChild(2).gameObject;
                rHand = rHandContainer.transform.GetChild(0).gameObject;
                if (rHand.name == "RightHand")
                {
                    rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, rightController.transform.position);
                }
                else
                {
                    rHand = rHandContainer.transform.GetChild(1).gameObject;
                    rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, rightController.transform.position);
                }
                //Debug.Log($"{rHand.name} è rHand");

            }
            StartCoroutine(Wait());
        }
    }

    public IEnumerator Wait()
    {
        double time = 0.25;
        GameObject head = netPlayer.transform.GetChild(0).gameObject;
        GameObject otherRightContr = netPlayer.transform.GetChild(2).gameObject;
        yield return new WaitForSeconds((float)time);
        Vector3 rPos;

        Destroy(rightController.GetComponent("HandController"));
        rightHand.transform.parent = player.transform;
        rPos = rightController.transform.position;
        player.transform.position = rPos;
        player.transform.position = Vector3.Lerp(rPos, otherRightContr.transform.position, 0.5f);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, (float)(player.transform.position.z + 0.516));
        direction = head.transform.position - camera.transform.position;
        y_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //x_angle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
        //z_angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        player.transform.rotation = Quaternion.Euler(0, y_angle, 0);

        rightHandAnimator.Play("Handshake", -1, 0);

        waitConfirmUI.GetComponent<Canvas>().enabled = false;
        handshakeUI.GetComponent<Canvas>().enabled = true;

        confirmCanvas.GetComponent<HandshakeConfirmCanvas>().DeactivateHandshakeConfirmCanvas();
    }
}

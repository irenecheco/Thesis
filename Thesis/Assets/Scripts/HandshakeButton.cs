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
    private GameObject netPlayer;
    private GameObject rHandContainer;
    private GameObject rHand;
    private GameObject netPlayer2;
    float angle;
    private Vector3 direction;
    //private Animation handshakeAnimation;
    //private AnimatorStateInfo animStateInfo;
    //private Vector3 rHandPosition;
    //rivate Vector3 lHandPosition;

    // Start is called before the first frame update
    void Start()
    {
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        player = GameObject.Find("Player");
        camera = GameObject.Find("Camera Offset/Main Camera");
        rightHandAnimator = rightHand.GetComponent<Animator>();
        //handshakeAnimation = rightHand.GetComponent<Animation>();
        player.transform.position = GameObject.Find("Camera Offset/RightHand Controller").transform.position;
    }

    public void OnHandshakePressed()
    {
        otherPlayer = camera.GetComponent<OnCollisionActivateButton>().otherPlayerHead;
        //Debug.Log($"{otherPlayer.name}");
        if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
        {
            netPlayer = otherPlayer.transform.parent.gameObject;
            rHandContainer = netPlayer.transform.GetChild(2).gameObject;
            rHand = rHandContainer.transform.GetChild(0).gameObject;
            if(rHand.name == "RightHand")
            {
                rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position);
            }
            else
            {
                rHand = rHandContainer.transform.GetChild(1).gameObject;
                rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position);
            }
            //Debug.Log($"{rHand.name} è rHand");
            
        }
        StartCoroutine(Wait());
    }

    public void SetBackComponent()
    {
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<HandController>();
        rightController.GetComponent<HandController>().hand = rightHand.GetComponent<Hand>();
        /*rHand = rHandContainer.transform.GetChild(0).gameObject;
        Debug.Log($"{rHand} è rHand");
        if (rHand.name == "RightHand")
        {
            rHand.GetComponent<NetworkHandshakeRespond>().SetBackComponent();
        }
        else
        {
            rHand = rHandContainer.transform.GetChild(1).gameObject;
            rHand.GetComponent<NetworkHandshakeRespond>().SetBackComponent();
        }*/        
    }

    public IEnumerator Wait()
    {
        double time = 0.25;
        GameObject head = netPlayer.transform.GetChild(0).gameObject;
        yield return new WaitForSeconds((float)time);
            
        Destroy(rightController.GetComponent("HandController"));
        rightHand.transform.parent = player.transform;
        player.transform.position = rightController.transform.position;
        direction = head.transform.position - camera.transform.position;
        angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        player.transform.rotation = Quaternion.Euler(0, angle, 0);

        rightHandAnimator.Play("Handshake", -1, 0);
    }
}

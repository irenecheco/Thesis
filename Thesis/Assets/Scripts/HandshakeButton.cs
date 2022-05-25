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
    private GameObject myPlayer;
    private GameObject myPlayerHead;
    private GameObject myPlayerConfirm;
    float y_angle;
    //float x_angle;
    //float z_angle;
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
        for(int i=0; i<PhotonNetwork.PlayerList.Length; i++)
        {
            myPlayer = GameObject.Find($"Network Player {+i}");
            if(myPlayer.GetComponent<PhotonView>().Owner.IsLocal)
            {
                break;
            }
        }
        myPlayerHead = myPlayer.transform.GetChild(0).gameObject;
        myPlayerConfirm = myPlayerHead.transform.GetChild(0).gameObject;
        myPlayerConfirm.GetComponent<HandshakeConfirm>().ActivateHandshakeConfirmCanvas();
       

        otherPlayer = camera.GetComponent<OnCollisionActivateButton>().otherPlayerHead;
        //Debug.Log($"{otherPlayer.name}");
        if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
        {
            netPlayer = otherPlayer.transform.parent.gameObject;
            rHandContainer = netPlayer.transform.GetChild(2).gameObject;
            rHand = rHandContainer.transform.GetChild(0).gameObject;
            if(rHand.name == "RightHand")
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

    public void SetBackComponent()
    {
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<HandController>();
        rightController.GetComponent<HandController>().hand = rightHand.GetComponent<Hand>();      
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
    }
}

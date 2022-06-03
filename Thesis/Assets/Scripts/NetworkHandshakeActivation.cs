using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class NetworkHandshakeActivation : MonoBehaviour
{
    private GameObject rightHand;
    private GameObject leftHand;
    private GameObject rightController;
    private GameObject leftController;
    private GameObject player;
    private GameObject camera;
    private GameObject rHandContainer;
    private GameObject rHand;
    private GameObject otherPlayer;
    private GameObject confirmCanvas;
    private GameObject waitConfirmUI;
    private GameObject handshakeUI;
    private GameObject myHead;
    private PhotonView photonView;

    private Animator rightHandAnimator;

    private Vector3 direction;

    private float y_angle;
    //private float x_angle;
    //private float z_angle;


    private string[] playersID = new string[2];
    //private string player2ID;

    // Start is called before the first frame update
    void Start()
    {
        photonView = this.GetComponent<PhotonView>();
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        leftHand = GameObject.Find("Camera Offset/LeftHand Controller/LeftHand");
        waitConfirmUI = leftHand.transform.GetChild(3).gameObject;
        handshakeUI = leftHand.transform.GetChild(2).gameObject;
        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        leftController = GameObject.Find("Camera Offset/LefttHand Controller");
        player = GameObject.Find("Player");
        camera = GameObject.Find("Camera Offset/Main Camera");
        rightHandAnimator = rightHand.GetComponent<Animator>();
        //handshakeAnimation = rightHand.GetComponent<Animation>();
        //player.transform.position = GameObject.Find("Camera Offset/RightHand Controller").transform.position;
        myHead = this.gameObject.transform.GetChild(0).gameObject;
        confirmCanvas = myHead.gameObject.transform.GetChild(0).gameObject;

    }

    public void CallActivationOverNetwork(string pl1ID, string pl2ID)
    {
        playersID[0] = pl1ID;
        playersID[1] = pl2ID;
        Debug.Log($"id 1 è {playersID[0]}, id 2 è {playersID[1]}");
        photonView.RPC("ActivateHandshakeOverNetwork", RpcTarget.All, playersID as object[]);
    }

    [PunRPC]
    public void ActivateHandshakeOverNetwork(object[] ids)
    {

        string[] playersIds = new string[2];
        playersIds[0] = (string)ids[0];
        playersIds[1] = (string)ids[1];
        if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
        {
            foreach( var item in PhotonNetwork.PlayerList)
            {
                if(item.UserId == playersIds[1])
                {
                    otherPlayer = (GameObject)item.TagObject;
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                rHandContainer = otherPlayer.transform.GetChild(2).gameObject;
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

        } else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
        {
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[0])
                {
                    otherPlayer = (GameObject)item.TagObject;
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                rHandContainer = otherPlayer.transform.GetChild(2).gameObject;
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
    /*public void SetBackComponent()
    {
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<HandController>();
        rightController.GetComponent<HandController>().hand = rightHand.GetComponent<Hand>();
    }*/

    public IEnumerator Wait()
    {
        double time = 0.25;
        GameObject head = otherPlayer.transform.GetChild(0).gameObject;
        yield return new WaitForSeconds((float)time);
        float starting_y = 0;
        Vector3 midPosition;

        if (camera.transform.position.y <= head.transform.position.y)
        {
            starting_y = camera.transform.position.y;
        } else {
            starting_y = head.transform.position.y;
        }

        Destroy(rightController.GetComponent("HandController"));
        rightHand.GetComponent<Hand>().flag = true;
        rightHand.transform.parent = player.transform;
        midPosition = Vector3.Lerp(head.transform.position, camera.transform.position, 0.5f);
        player.transform.position = new Vector3(midPosition.x, (float)(starting_y - 0.4), midPosition.z);
        //player.transform.position = new Vector3(camera.transform.position.x, (float)(mid_y - 0.4), camera.transform.position.z);
        //player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, (float)(player.transform.position.z + 0.516));
        direction = (head.transform.position - camera.transform.position).normalized;
        y_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //Debug.Log($"y direction {y_angle}");
        //Debug.Log($" camera {camera.transform.rotation.eulerAngles.y}");
        //x_angle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
        //z_angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        float camera_y_angle = camera.transform.rotation.eulerAngles.y;
        if (y_angle < 0)
        {
            float offset = -y_angle;
            y_angle = 360 - offset;
        }
        if(camera_y_angle < 0)
        {
            float offset = camera_y_angle;
            camera_y_angle = 360 - offset;
        }
        if ((y_angle - 90) < camera_y_angle && camera_y_angle < (y_angle + 90))
        {
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
            player.transform.rotation = Quaternion.Euler(0, y_angle, 0);
            player.transform.Translate(new Vector3((float)(-0.02), 0, (float)(-0.560)), Space.Self);
        } else
        {
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
            player.transform.rotation = Quaternion.Euler(0, (y_angle - 180), 0);
            player.transform.Translate(new Vector3((float)(+0.02), 0, (float)(+0.560)), Space.Self);
        }
        
        

        rightHandAnimator.Play("Handshake", -1, 0);

        //SetBackComponent();

        //Debug.Log($"{rightHand.transform.parent.gameObject.name} è il parent; {rightController.GetComponent<HandController>().hand} è la mano assegnata");

        waitConfirmUI.GetComponent<Canvas>().enabled = false;
        handshakeUI.GetComponent<Canvas>().enabled = true;

        confirmCanvas.GetComponent<HandshakeConfirmCanvas>().DeactivateHandshakeConfirmCanvas();
    }
}

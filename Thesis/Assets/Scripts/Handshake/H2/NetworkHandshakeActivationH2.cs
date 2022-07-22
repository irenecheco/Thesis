using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class NetworkHandshakeActivationH2 : MonoBehaviour
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

    private int sceneIndex;


    private string[] playersID = new string[2];

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        leftHand = GameObject.Find("Camera Offset/LeftHand Controller/LeftHand");

        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        leftController = GameObject.Find("Camera Offset/LefttHand Controller");
        player = GameObject.Find("Player");
        camera = GameObject.Find("Camera Offset/Main Camera");
        rightHandAnimator = rightHand.GetComponent<Animator>();

        if(this.name != "RightHand")
        {
            myHead = this.gameObject.transform.GetChild(0).gameObject;
            photonView = this.GetComponent<PhotonView>();
        }        
    }

    public void CallActivationOverNetwork(string pl1ID, string pl2ID)
    {
        playersID[0] = pl1ID;
        playersID[1] = pl2ID;
        //Debug.Log($"id 1 è {playersID[0]}, id 2 è {playersID[1]}");
        if (pl1ID != null && pl2ID != null)
        {
            photonView.RPC("ActivateHandshakeOverNetwork", RpcTarget.All, playersID as object[]);
        }
    }

    [PunRPC]
    public void ActivateHandshakeOverNetwork(object[] ids)
    {

        string[] playersIds = new string[2];
        playersIds[0] = (string)ids[0];
        playersIds[1] = (string)ids[1];
        if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
        {
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[1])
                {
                    otherPlayer = (GameObject)item.TagObject;
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                rHandContainer = otherPlayer.transform.GetChild(2).gameObject;
                if(rHandContainer.transform.childCount > 0)
                {
                    rHand = rHandContainer.transform.GetChild(0).gameObject;
                }
                if (rHand.name == "RightHand")
                {
                    rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, rightController.transform.position);
                }
                else
                {
                    rHand = rHandContainer.transform.GetChild(1).gameObject;
                    rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, rightController.transform.position);
                }
            }
            StartCoroutine(Wait());
        }
        else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
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
                if (rHandContainer.transform.childCount > 0)
                {
                    rHand = rHandContainer.transform.GetChild(0).gameObject;
                }
                if (rHand.name == "RightHand")
                {
                    rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, rightController.transform.position);
                }
                else
                {
                    rHand = rHandContainer.transform.GetChild(1).gameObject;
                    rHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position, rightController.transform.position);
                }
            }
            StartCoroutine(Wait());
        }
    }

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
        }
        else
        {
            starting_y = head.transform.position.y;
        }

        Destroy(rightController.GetComponent("HandController"));
        rightHand.GetComponent<Hand>().flag = true;
        rightHand.transform.parent = player.transform;

        midPosition = Vector3.Lerp(head.transform.position, camera.transform.position, 0.5f);
        player.transform.position = new Vector3(midPosition.x, (float)(starting_y - 0.4), midPosition.z);
        direction = (head.transform.position - camera.transform.position).normalized;
        y_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        
        float camera_y_angle = camera.transform.rotation.eulerAngles.y;
        if (y_angle < 0)
        {
            float offset = -y_angle;
            y_angle = 360 - offset;
        }
        if (camera_y_angle < 0)
        {
            float offset = camera_y_angle;
            camera_y_angle = 360 - offset;
        }
        if ((y_angle - 90) < camera_y_angle && camera_y_angle < (y_angle + 90))
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


        //Debug.Log("Attivo animazione");

        rightHandAnimator.Play("Handshake2", -1, 0);
    }

    public void SetBackComponent()
    {
        //Debug.Log("Entra nel setBackComponent");
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<HandController>();
        rightController.GetComponent<HandController>().hand = rightHand.GetComponent<Hand>();
        rightHand.GetComponent<Hand>().flag = false;
        //myHead.gameObject.transform.GetComponent<OnButtonAPressed>().animationGoing = false;
    }
}


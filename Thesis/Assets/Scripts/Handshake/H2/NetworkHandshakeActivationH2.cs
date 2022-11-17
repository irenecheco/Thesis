using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class NetworkHandshakeActivationH2 : MonoBehaviour
{
    //Code responsible for the activation of the handshake animation over the network: calls a method over the network so
    //that the involved users get notified and start the animation

    private GameObject rightHand;
    private GameObject rightController;
    private GameObject camera;
    private GameObject otherRHandContainer;
    private GameObject otherRHand;
    private GameObject otherPlayer;
    private GameObject confirmCanvas;
    private GameObject messageCanvas;
    private GameObject myHead;
    private PhotonView photonView;
    private GameObject handMesh;
    private GameObject otherHandMesh;

    private GameObject fakeHandHolder;
    private GameObject fakeHand;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private string[] playersID = new string[2];

    void Start()
    {
        rightHand = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        handMesh = rightHand.transform.FindChildRecursive("hands:Lhand").gameObject;

        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        fakeHandHolder = GameObject.Find("FakeHandHolder");
        fakeHand = fakeHandHolder.transform.GetChild(0).gameObject;
        camera = Camera.main.gameObject;

        myHead = this.gameObject.transform.GetChild(0).gameObject;
        photonView = this.GetComponent<PhotonView>();       
    }

    //Functions called when both users are pressing the A button
    public void CallActivationOverNetwork(string pl1ID, string pl2ID)
    {
        playersID[0] = pl1ID;
        playersID[1] = pl2ID;

        if (pl1ID != null && pl2ID != null)
        {
            //Photon Pun method that calls a function over the network
            photonView.RPC("ActivateHandshakeOverNetwork", RpcTarget.All, playersID as object[]);
        }
    }

    //Function called over the network (on every network player): it checks if the player is involved in the handshake and,
    //if it is, it triggers the animation
    [PunRPC]
    public void ActivateHandshakeOverNetwork(object[] ids)
    {
        //Debug.Log("Activate handshake over network");
        string[] playersIds = new string[2];
        playersIds[0] = (string)ids[0];
        playersIds[1] = (string)ids[1];
        if (playersIds[0] == PhotonNetwork.LocalPlayer.UserId)
        {
            //Debug.Log("Player locale è 0");
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[1])
                {
                    otherPlayer = (GameObject)item.TagObject;
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                //Debug.Log("altro player != null");
                otherRHandContainer = otherPlayer.transform.GetChild(2).gameObject;
                otherRHand = otherRHandContainer.transform.GetChild(0).gameObject;

                if (otherRHand.activeSelf)
                {
                    //Debug.Log("mano destra altro giocatore attiva");
                    otherRHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position);
                }
                otherHandMesh = otherRHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                GameObject netHead = otherPlayer.transform.GetChild(0).gameObject;
                netHead.GetComponent<OnButtonAPressed>().animationGoing = true;
                confirmCanvas = netHead.transform.GetChild(0).gameObject;
                messageCanvas = netHead.transform.GetChild(2).gameObject;        
            }
            StartCoroutine(Wait());
        }
        else if (playersIds[1] == PhotonNetwork.LocalPlayer.UserId)
        {
            Debug.Log("Player locale è 1");
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == playersIds[0])
                {
                    otherPlayer = (GameObject)item.TagObject;
                }
            }
            if (!otherPlayer.GetComponent<PhotonView>().IsMine && otherPlayer != null)
            {
                //Debug.Log("altro player != null");
                otherRHandContainer = otherPlayer.transform.GetChild(2).gameObject;
                otherRHand = otherRHandContainer.transform.GetChild(0).gameObject;

                if (rightHand.activeSelf)
                {
                    //Debug.Log("mano destra altro giocatore attiva");
                    otherRHand.GetComponent<NetworkHandshakeRespond>().OnHandshakePressed(camera.transform.position);
                }
                otherHandMesh = otherRHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                GameObject netHead = otherPlayer.transform.GetChild(0).gameObject;
                netHead.GetComponent<OnButtonAPressed>().animationGoing = true;
                confirmCanvas = netHead.transform.GetChild(0).gameObject;
                messageCanvas = netHead.transform.GetChild(2).gameObject;
            }
            StartCoroutine(Wait());
        }
    }

    //Coroutine that trigger the animation on the network player
    public IEnumerator Wait()
    {
        //Debug.Log("entra in coroutine");
        float time = (float)0.25;
        GameObject head = otherPlayer.transform.GetChild(0).gameObject;
        yield return new WaitForSeconds(time);

        rightHand.SetActive(false);
        fakeHand.SetActive(true);

        fakeHand.GetComponent<HandshakeFakeHand>().DoHandshake(camera.transform.position, head.transform.position);

        confirmCanvas.GetComponent<Canvas>().enabled = false;
        messageCanvas.GetComponent<Canvas>().enabled = true;
        myHead.gameObject.transform.GetComponent<OnButtonAPressed>().animationGoing = false;
        //handMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
        otherHandMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
    }
}


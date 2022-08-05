using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkGrabMessageActivationH3 : MonoBehaviour
{
    private string[] playersID = new string[2];

    private PhotonView photonView;

    private GameObject rightController;
    private GameObject rightHand;

    private GameObject GrabbedNetPlayer;
    private GameObject GrabbedNetRightController;
    private GameObject GrabbedNetRightHand;

    private GameObject GrabbingNetPlayer;
    private GameObject GrabbingNetHead;
    private GameObject GrabbingNetMessageCanvas;

    private GameObject ReleasedNetPlayer;
    private GameObject ReleasedNetRightController;
    private GameObject ReleasedNetRightHand;
    private GameObject ReleasedNetHead;
    private GameObject ReleasedNetMessageCanvas;

    void Start()
    {
        photonView = this.transform.GetParentComponent<PhotonView>();

        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        rightHand = rightController.transform.GetChild(0).gameObject;
    }

    public void CallActivateGrabMessage(string plGrabbing, string plGrabbed)
    {
        playersID[0] = plGrabbing;
        playersID[1] = plGrabbed;
        //Debug.Log($"id 1 è {playersID[0]}, id 2 è {playersID[1]}");
        if (plGrabbing != null && plGrabbed != null)
        {
            photonView.RPC("ActivateGrabMessageOverNetwork", RpcTarget.All, playersID as object[]);
        }
    }

    public void CallDeactivateGrabMessage(string plGrabbing, string plGrabbed)
    {
        playersID[0] = plGrabbing;
        playersID[1] = plGrabbed;
        //Debug.Log($"id 1 è {playersID[0]}, id 2 è {playersID[1]}");
        if (plGrabbing != null && plGrabbed != null)
        {
            photonView.RPC("DeactivateGrabMessageOverNetwork", RpcTarget.All, playersID as object[]);
        }
    }

    [PunRPC]
    public void ActivateGrabMessageOverNetwork(object[] ids)
    {
        string plGrabbing;
        string plGrabbed;
        plGrabbing = (string)ids[0];
        plGrabbed = (string)ids[1];
        if(plGrabbed == PhotonNetwork.LocalPlayer.UserId)
        {
            rightHand.GetComponent<HapticController>().SendHaptics1H3();
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == plGrabbed)
                {
                    GrabbedNetPlayer = (GameObject)item.TagObject;
                    GrabbedNetRightController = GrabbedNetPlayer.transform.GetChild(2).gameObject;
                    GrabbedNetRightHand = GrabbedNetRightController.transform.GetChild(0).gameObject;
                }
            }

            if(GrabbedNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == false)
            {
                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.UserId == plGrabbing)
                    {
                        GrabbingNetPlayer = (GameObject)item.TagObject;
                        GrabbingNetHead = GrabbingNetPlayer.transform.GetChild(0).gameObject;
                        GrabbingNetMessageCanvas = GrabbingNetHead.transform.GetChild(2).gameObject;
                    }
                }

                GrabbingNetMessageCanvas.GetComponent<Canvas>().enabled = true;
            }
        } else if(plGrabbing == PhotonNetwork.LocalPlayer.UserId)
        {
            rightHand.GetComponent<HapticController>().SendHaptics1H3();
        }
    }

    [PunRPC]
    public void DeactivateGrabMessageOverNetwork(object[] ids)
    {
        string plReleasing;
        string plReleased;
        plReleasing = (string)ids[0];
        plReleased = (string)ids[1];
        if (plReleasing == PhotonNetwork.LocalPlayer.UserId)
        {
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == plReleased)
                {
                    ReleasedNetPlayer = (GameObject)item.TagObject;
                    ReleasedNetRightController = ReleasedNetPlayer.transform.GetChild(2).gameObject;
                    ReleasedNetRightHand = ReleasedNetRightController.transform.GetChild(0).gameObject;
                    ReleasedNetHead = ReleasedNetPlayer.transform.GetChild(0).gameObject;
                    ReleasedNetMessageCanvas = ReleasedNetHead.transform.GetChild(2).gameObject;
                }
            }

            if (ReleasedNetRightHand.GetComponent<MessageActivationH3>().isGrabbing == true)
            {
                ReleasedNetMessageCanvas.GetComponent<Canvas>().enabled = true;
            } else
            {
                ReleasedNetMessageCanvas.GetComponent<Canvas>().enabled = false;
            }
        }
    }
}

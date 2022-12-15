using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkGrabMessageActivationH4 : MonoBehaviour
{
    //Code responsible fot the activation of the message canvas over the network when user grab the hand of the other user

    private string[] playersID = new string[2];

    private PhotonView photonView;

    private GameObject rightController;
    private GameObject rightHand;

    private GameObject GrabbedNetPlayer;
    private GameObject GrabbedNetRightController;
    private GameObject GrabbedNetRightHand;

    private GameObject GrabbingNetPlayer;
    private GameObject GrabbingNetHead;
    private GameObject GrabbingNetRightHand;
    private GameObject GrabbingNetRightMesh;
    private GameObject GrabbingNetConfirmCanvas;
    private GameObject GrabbingNetMessageCanvas;

    private GameObject ReleasedNetPlayer;
    private GameObject ReleasedNetRightController;
    private GameObject ReleasedNetRightHand;
    private GameObject ReleasedNetRightMesh;
    private GameObject ReleasedNetHead;
    private GameObject ReleasedNetMessageCanvas;
    private GameObject ReleasedNetConfirmCanvas;

    private GameObject ReleasingNetPlayer;
    private GameObject ReleasingNetRightController;
    private GameObject ReleasingNetRightHand;
    private GameObject ReleasingNetRightMesh;
    private GameObject ReleasingNetHead;
    private GameObject ReleasingNetConfirmCanvas;
    private GameObject ReleasingNetMessageCanvas;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color waitingColor = new Color(0.4135279f, 0.7409829f, 0.9056604f, 1.0f);

    public bool animationGoing;

    void Start()
    {
        photonView = this.transform.GetParentComponent<PhotonView>();

        animationGoing = false;

        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        rightHand = rightController.transform.GetChild(0).gameObject;
    }

    //Functions called when user grab the other user hand
    public void CallActivateGrabMessage(string plGrabbing, string plGrabbed)
    {
        playersID[0] = plGrabbing;
        playersID[1] = plGrabbed;
        if (plGrabbing != null && plGrabbed != null)
        {
            //Photon Pun method that calls a function over the network
            photonView.RPC("ActivateGrabMessageOverNetwork", RpcTarget.All, playersID as object[]);
        }
    }

    //Functions called when user release the other user hand
    public void CallDeactivateGrabMessage(string plGrabbing, string plGrabbed)
    {
        playersID[0] = plGrabbing;
        playersID[1] = plGrabbed;
        if (plGrabbing != null && plGrabbed != null)
        {
            //Photon Pun method that calls a function over the network
            photonView.RPC("DeactivateGrabMessageOverNetwork", RpcTarget.All, playersID as object[]);
        }
    }

    //Function called over the network (on every network player): it checks if the player is involved in the handshake and,
    //if it is, it triggers the message
    [PunRPC]
    public void ActivateGrabMessageOverNetwork(object[] ids)
    {
        string plGrabbing;
        string plGrabbed;
        plGrabbing = (string)ids[0];
        plGrabbed = (string)ids[1];
        if(plGrabbed == PhotonNetwork.LocalPlayer.UserId)
        {
            //My user id is in position 1: my hand is being grabbed
                        
            foreach (var item in PhotonNetwork.PlayerList)
            {
                if (item.UserId == plGrabbed)
                {
                    GrabbedNetPlayer = (GameObject)item.TagObject;
                    GrabbedNetRightController = GrabbedNetPlayer.transform.GetChild(2).gameObject;
                    GrabbedNetRightHand = GrabbedNetRightController.transform.GetChild(0).gameObject;
                }
            }

            if (GrabbedNetPlayer.GetComponent<PhotonView>().IsMine && GrabbedNetPlayer != null)
            {
                //this is the local net player
                if (GrabbedNetRightHand.GetComponent<MessageActivationH4>().isGrabbing == false)
                {
                    //If I'm not already grabbing the other users's hand, I receive an haptic feedback
                    rightHand.GetComponent<HapticController>().SendHaptics1H3();

                    foreach (var item in PhotonNetwork.PlayerList)
                    {
                        if (item.UserId == plGrabbing)
                        {
                            GrabbingNetPlayer = (GameObject)item.TagObject;
                            GrabbingNetHead = GrabbingNetPlayer.transform.GetChild(0).gameObject;
                            GrabbingNetRightHand = GrabbingNetPlayer.transform.FindChildRecursive("RightHand").gameObject;
                            GrabbingNetRightMesh = GrabbingNetRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                            GrabbingNetMessageCanvas = GrabbingNetHead.transform.GetChild(2).gameObject;
                            GrabbingNetConfirmCanvas = GrabbingNetHead.transform.GetChild(3).gameObject;
                        }
                    }
                    //I have saved the user who is grabbing me
                    //His hand turns blue and a message appears under his head
                    GrabbingNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
                    GrabbingNetMessageCanvas.GetComponent<Canvas>().enabled = false;
                    GrabbingNetConfirmCanvas.GetComponent<Canvas>().enabled = true;
                }
            } //else we are both grabbing -> we can handshake
        } else if(plGrabbing == PhotonNetwork.LocalPlayer.UserId)
        {
            if (this.transform.GetComponent<PhotonView>().IsMine)
            {
                //My user id is in position 0: I'm grabbing, I get an haptic feedback
                rightHand.GetComponent<HapticController>().SendHaptics1H3();
                //My movement should freeze and I should see a waiting message 
            }            
        }
    }

    //Function called over the network (on every network player): it checks if the player is involved in the handshake and,
    //if it is, it triggers the message
    [PunRPC]
    public void DeactivateGrabMessageOverNetwork(object[] ids)
    {
        if (animationGoing == false)
        {
            string plReleasing;
            string plReleased;
            plReleasing = (string)ids[0];
            plReleased = (string)ids[1];
            if (plReleasing == PhotonNetwork.LocalPlayer.UserId)
            {
                //My user id is in position 0: I'm releasing the other user's hand
                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.UserId == plReleased)
                    {
                        ReleasedNetPlayer = (GameObject)item.TagObject;
                        ReleasedNetRightController = ReleasedNetPlayer.transform.GetChild(2).gameObject;
                        ReleasedNetRightHand = ReleasedNetRightController.transform.GetChild(0).gameObject;
                        ReleasedNetRightMesh = ReleasedNetRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                        ReleasedNetHead = ReleasedNetPlayer.transform.GetChild(0).gameObject;
                        ReleasedNetMessageCanvas = ReleasedNetHead.transform.GetChild(2).gameObject;
                        ReleasedNetConfirmCanvas = ReleasedNetHead.transform.GetChild(3).gameObject;
                    }
                }
                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.UserId == plReleasing)
                    {
                        ReleasingNetPlayer = (GameObject)item.TagObject;
                        ReleasingNetHead = ReleasingNetPlayer.transform.GetChild(0).gameObject;
                        ReleasingNetRightController = ReleasingNetPlayer.transform.GetChild(2).gameObject;
                        ReleasingNetRightHand = ReleasingNetRightController.transform.GetChild(0).gameObject;
                        ReleasingNetRightMesh = ReleasedNetRightController.transform.FindChildRecursive("hands:Lhand").gameObject;
                        ReleasingNetMessageCanvas = ReleasingNetHead.transform.GetChild(2).gameObject;
                        ReleasingNetConfirmCanvas = ReleasingNetHead.transform.GetChild(3).gameObject;
                    }
                }

                if (ReleasingNetPlayer.GetComponent<PhotonView>().IsMine)
                {
                    //this is the local net player

                    if (ReleasedNetRightHand.GetComponent<MessageActivationH4>().isGrabbing == true)
                    {
                        //If the other user is still grabbing my hand, a message appears in front of me and his hand turns blue
                        ReleasedNetConfirmCanvas.GetComponent<Canvas>().enabled = true;
                        ReleasedNetMessageCanvas.GetComponent<Canvas>().enabled = false;
                        ReleasedNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
                    }
                    else
                    {
                        //The other user is not grabbing me as well, so everything gets back to normal
                        ReleasedNetConfirmCanvas.GetComponent<Canvas>().enabled = false;
                        ReleasedNetMessageCanvas.GetComponent<Canvas>().enabled = true;
                        ReleasedNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                        //ReleasedNetRightHand.GetComponent<MessageActivationH3>().DeactivateMessage();
                    }
                }
            }
            else if (plReleased == PhotonNetwork.LocalPlayer.UserId)
            {
                //My user id is in position 1: my hand is getting released
                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.UserId == plReleased)
                    {
                        ReleasedNetPlayer = (GameObject)item.TagObject;
                        ReleasedNetRightController = GrabbedNetPlayer.transform.GetChild(2).gameObject;
                        ReleasedNetRightHand = GrabbedNetRightController.transform.GetChild(0).gameObject;
                    }
                }

                if (ReleasedNetPlayer.GetComponent<PhotonView>().IsMine)
                {
                    //this is the local net player

                    if (ReleasedNetRightHand.GetComponent<MessageActivationH4>().isGrabbing == false)
                    {
                        //I am not grabbing the other user hand, so everything gets back to normal
                        foreach (var item in PhotonNetwork.PlayerList)
                        {
                            if (item.UserId == plReleasing)
                            {
                                ReleasingNetPlayer = (GameObject)item.TagObject;
                                ReleasingNetHead = ReleasingNetPlayer.transform.GetChild(0).gameObject;
                                ReleasingNetRightController = ReleasingNetPlayer.transform.GetChild(2).gameObject;
                                ReleasingNetRightHand = ReleasingNetRightController.transform.GetChild(0).gameObject;
                                ReleasingNetRightMesh = ReleasedNetRightController.transform.FindChildRecursive("hands:Lhand").gameObject;
                                ReleasingNetMessageCanvas = ReleasingNetHead.transform.GetChild(2).gameObject;
                                ReleasingNetConfirmCanvas = ReleasingNetHead.transform.GetChild(3).gameObject;
                            }
                        }
                        ReleasingNetRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                        ReleasingNetMessageCanvas.GetComponent<Canvas>().enabled = true;
                        ReleasingNetConfirmCanvas.GetComponent<Canvas>().enabled = false;
                    }
                    else
                    {
                        //I am still grabbing other user hand, I get an haptic feedback
                        rightHand.GetComponent<HapticController>().SendHaptics1H3();
                        //My movement should freeze and I should see a waiting message
                        //ReleasedNetRightHand.GetComponent<MessageActivationH3>().ActivateMessage();
                    }
                }
            }
        }
    }
}

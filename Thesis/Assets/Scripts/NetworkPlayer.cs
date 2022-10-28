using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkPlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    //Code responsible for the setup of some general information of the Network Player as mapping the movement of the head and 
    //controllers to the network head and controllers

    public Transform head;
    public Transform leftHand_controllers;
    public Transform rightHand_controllers;
    public Transform leftHand_hands;
    public Transform rightHand_hands;

    private PhotonView photonView;

    private XROrigin rig;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    private GameObject headObject;
    private GameObject handshakeConfirm;

    private int sceneIndex;

    private bool newUpdateHand = false;
    private bool newUpdateControllers = false;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        photonView = GetComponent<PhotonView>();
        headObject = this.gameObject.transform.GetChild(0).gameObject;

        //In H1 the confirm canvas is disabled at the beginning
        if(sceneIndex == 1)
        {
            handshakeConfirm = headObject.transform.GetChild(0).gameObject;
            handshakeConfirm.transform.GetComponent<Canvas>().enabled = false;
        }        

        //Find the local XR Origin
        rig = FindObjectOfType<XROrigin>();
        headRig = rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");

        //In H4 the XR Origin is slightly different because it also has the hand tracking
        if(sceneIndex == 4)
        {
            headRig = rig.transform.Find("Camera Offset/OculusInteractionSampleRig - no controllers/OVRCameraRig/TrackingSpace/CenterEyeAnchor");
            leftHandRig = rig.transform.Find("Camera Offset/OculusInteractionSampleRig - no controllers/InputOVR/Hands Controller/LeftHand/LeftHandVisual/OculusHand_L");
            rightHandRig = rig.transform.Find("Camera Offset/OculusInteractionSampleRig - no controllers/InputOVR/Hands Controller/RightHand/RightHandVisual/OculusHand_R");
        }

        //In the local scene the network player (corrisponding to the local player) has to be hidden to not have the same
        //player rendering two times on top of each other
        if (photonView.IsMine)
        {
            foreach(var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
    }

    void Update()
    {
        //The hands animation for the network player needs to be updated according to his local player's controllers, so it needs
        //to be updated only if the photon view component IsMine (is local)

        if (photonView.IsMine)
        {
            if(sceneIndex == 4)
            {
                //In H4 the movement mapping change if the player is using the controllers or the hands tracking, hence
                //the following if
                if(this.GetComponent<HandsOrControllersH4>().handsOn == true)
                {
                    if (newUpdateHand == false)
                    {
                        leftHandRig = rig.transform.Find("Camera Offset/OculusInteractionSampleRig - no controllers/InputOVR/Hands/LeftHand/LeftHandVisual/OculusHand_L").transform;
                        rightHandRig = rig.transform.Find("Camera Offset/OculusInteractionSampleRig - no controllers/InputOVR/Hands/RightHand/RightHandVisual/OculusHand_R").transform;
                        newUpdateHand = true;
                        newUpdateControllers = false;
                    }

                    if (leftHand_hands.parent.gameObject.activeSelf == true)
                    {
                        MapPosition(head, headRig);
                        MapPosition(leftHand_hands, leftHandRig);
                        MapPosition(rightHand_hands, rightHandRig);
                    }
                }
                else
                {
                    if (newUpdateControllers == false)
                    {
                        leftHandRig = rig.transform.Find("Camera Offset/OculusInteractionSampleRig - no controllers/InputOVR/Hands Controller/LeftHand/LeftHandVisual/OculusHand_L").transform;
                        rightHandRig = rig.transform.Find("Camera Offset/OculusInteractionSampleRig - no controllers/InputOVR/Hands Controller/RightHand/RightHandVisual/OculusHand_R").transform;
                        newUpdateControllers = true;
                        newUpdateHand = false;
                    }

                    if (leftHand_controllers.parent.gameObject.activeSelf == true)
                    {
                        MapPosition(head, headRig);
                        MapPosition(leftHand_controllers, leftHandRig);
                        MapPosition(rightHand_controllers, rightHandRig);
                    }
                }
            } else
            {
                //In the other scenes the mapping is done according to the controllers
                MapPosition(head, headRig);
                MapPosition(leftHand_controllers, leftHandRig);
                MapPosition(rightHand_controllers, rightHandRig);
            }            
        }

        //In H4 different hands need to be render according to the controllers used (controllers vs hand tracking)
        if (!photonView.IsMine)
        {
            if (sceneIndex == 4)
            {
                if (this.GetComponent<HandsOrControllersH4>().handsOn == true)
                {
                    leftHand_controllers.parent.gameObject.SetActive(false);
                    //rightHand_controllers.gameObject.SetActive(false);
                    leftHand_hands.parent.gameObject.SetActive(true);
                    //rightHand_hands.gameObject.SetActive(true);
                }
                else
                {
                    leftHand_hands.parent.gameObject.SetActive(false);
                    //rightHand_hands.gameObject.SetActive(false);
                    leftHand_controllers.parent.gameObject.SetActive(true);
                    //rightHand_controllers.gameObject.SetActive(true);
                }
            }
        }                
    }

    //Function used to map network head and hands to local heads and hands
    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }

    //Every time a player is instanciated its game object is saved as the tag object of that player in the network
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
        //Debug.Log($"{info.Sender.TagObject}");
    }
}

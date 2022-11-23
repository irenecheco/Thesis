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
            
                //In the other scenes the mapping is done according to the controllers
                MapPosition(head, headRig);
                MapPosition(leftHand_controllers, leftHandRig);
                MapPosition(rightHand_controllers, rightHandRig);
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

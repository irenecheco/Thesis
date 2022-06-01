using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.UI;

public class NetworkPlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    private PhotonView photonView;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    private GameObject headObject;
    private GameObject handshakeConfirm;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        headObject = this.gameObject.transform.GetChild(0).gameObject;
        handshakeConfirm = headObject.transform.GetChild(0).gameObject;
        handshakeConfirm.transform.GetComponent<Canvas>().enabled = false;

        XROrigin rig = FindObjectOfType<XROrigin>();
        headRig = rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");

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
        if (photonView.IsMine)
        {
            //Debug.Log($"Aggiorna e leftHandRig è {leftHandRig}");
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);
        }

    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
        //Debug.Log($"{info.Sender.TagObject}");
    }
}

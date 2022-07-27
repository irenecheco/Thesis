using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class MessageActivationH3 : MonoBehaviour
{
    private GameObject rightController;
    private GameObject rightHand;
    private GameObject messageCanvas;

    private PhotonView photonView;

    void Start()
    {
        photonView = this.transform.GetParentComponent<PhotonView>();
        rightController = GameObject.Find("Camera Offset/RightHand Controller");
        rightHand = rightController.transform.GetChild(0).gameObject;
        messageCanvas = rightHand.transform.GetChild(3).gameObject;
    }

    public void ActivateMessage()
    {
        if (!photonView.IsMine && photonView != null)
        {
            messageCanvas.GetComponent<Canvas>().enabled = true;
            rightController.GetComponent<ActionBasedController>().enableInputTracking = false;
        }
    }

    public void DeactivateMessage()
    {
        if (!photonView.IsMine && photonView != null)
        {
            messageCanvas.GetComponent<Canvas>().enabled = false;
            rightController.GetComponent<ActionBasedController>().enableInputTracking = true;
        }
    }
}

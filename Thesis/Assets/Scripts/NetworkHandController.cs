using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using Photon.Pun;

public class NetworkHandController : MonoBehaviour
{
    //Code responsible for the set up of grip and target value read from the controller: network correspondent of HandController.cs

    private PhotonView photonView;

    public NetworkHand leftHand_hand;
    public NetworkHand rightHand_hand;

    ActionBasedController controllerLeft;
    ActionBasedController controllerRight;

    public bool isGrabbingH3;

    void Start()
    {
        //Set up of the variables

        photonView = this.transform.parent.gameObject.GetComponent<PhotonView>();

        XROrigin rig = FindObjectOfType<XROrigin>();

        controllerLeft = rig.transform.Find("Camera Offset/LeftHand Controller").GetComponent<ActionBasedController>();
        controllerRight = rig.transform.Find("Camera Offset/RightHand Controller").GetComponent<ActionBasedController>();

        isGrabbingH3 = false;
    }

    void Update()
    {
        //The hands animation for the network player needs to be updated according to his local player's controllers, so it needs
        //to be updated only if the photon view component IsMine (is local)

        if (photonView.IsMine)
        {
            UpdateHandAnimationLeft(controllerLeft, leftHand_hand);
            UpdateHandAnimationRight(controllerRight, rightHand_hand);
        }
    }

    void UpdateHandAnimationLeft(ActionBasedController controller, NetworkHand network_hand)
    {
        network_hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        network_hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }

    void UpdateHandAnimationRight(ActionBasedController controller, NetworkHand network_hand)
    {
        //Check if users are doing the handshake in the H3: in that case the grip button is always pressed, so to keep the hand
        //in a handshake position, grip is set to 0

        if (isGrabbingH3 == false)
        {
            network_hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        }
        else
        {
            network_hand.SetGrip(0);
        }
        network_hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }
}

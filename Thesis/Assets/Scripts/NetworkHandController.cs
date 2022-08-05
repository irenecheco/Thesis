using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using Photon.Pun;

public class NetworkHandController : MonoBehaviour
{
    private PhotonView photonView;

    public NetworkHand leftHand_hand;
    public NetworkHand rightHand_hand;

    ActionBasedController controllerLeft;
    ActionBasedController controllerRight;

    public bool isGrabbingH3;

    // Start is called before the first frame update
    void Start()
    {
        photonView = this.transform.parent.gameObject.GetComponent<PhotonView>();

        XROrigin rig = FindObjectOfType<XROrigin>();

        controllerLeft = rig.transform.Find("Camera Offset/LeftHand Controller").GetComponent<ActionBasedController>();
        controllerRight = rig.transform.Find("Camera Offset/RightHand Controller").GetComponent<ActionBasedController>();

        isGrabbingH3 = false;
    }

    // Update is called once per frame
    void Update()
    {
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

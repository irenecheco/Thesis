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

    // Start is called before the first frame update
    void Start()
    {
        photonView = this.transform.parent.gameObject.GetComponent<PhotonView>();

        XROrigin rig = FindObjectOfType<XROrigin>();

        controllerLeft = rig.transform.Find("Camera Offset/LeftHand Controller").GetComponent<ActionBasedController>();
        controllerRight = rig.transform.Find("Camera Offset/RightHand Controller").GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            UpdateHandAnimation(controllerLeft, leftHand_hand);
            UpdateHandAnimation(controllerRight, rightHand_hand);
        }
    }

    void UpdateHandAnimation(ActionBasedController controller, NetworkHand network_hand)
    {
        network_hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        network_hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }
}

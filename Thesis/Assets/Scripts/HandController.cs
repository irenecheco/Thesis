using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    //Code responsible for the set up of grip and target value read from the controller

    ActionBasedController controller;
    public Hand hand;
    public bool isGrabbingH3;

    void Start()
    {
        //Set up of the variables
        controller = GetComponent<ActionBasedController>();
        isGrabbingH3 = false;
    }

    void Update()
    {
        //Check if users are doing the handshake in the H3: in that case the grip button is always pressed, so to keep the hand
        //in a handshake position, grip is set to 0

        if (isGrabbingH3 == false)
        {
            hand.SetGrip(controller.selectAction.action.ReadValue<float>());
            hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
        } else
        {
            hand.SetGrip(0);
            hand.SetTrigger(0);
        }        
    }
}

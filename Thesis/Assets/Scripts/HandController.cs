using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    ActionBasedController controller;
    public Hand hand;
    public bool isGrabbingH3;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
        isGrabbingH3 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrabbingH3 == false)
        {
            hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        } else
        {
            hand.SetGrip(0);
        }
        hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }
}

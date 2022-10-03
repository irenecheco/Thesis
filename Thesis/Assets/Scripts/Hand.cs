using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    //Code responsible for grip and trigger animation

    public float animationSpeed;
    Animator animator;
    private float gripTarget;
    private float triggerTarget;
    public bool flag = false; //flag to check if users are doing the handshake
    private float gripCurrent;
    private float triggerCurrent;
    private string animatorGripParam = "Grip";
    private string animatorTriggerParam = "Trigger";

    void Start()
    {
        //Setup of the animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(flag == false)
        {
            //if users are not doing the handshake the grip and trigger animations are started according to the buttons pressure
            
            AnimateHand();
        } else if (flag == true) 
        {
            //if users are doing the handshake in the H3 the grip button is always pressed, so to keep the hand in a handshake
            //position, grip and trigger are set to 0
            
            gripCurrent = 0;
            animator.SetFloat(animatorGripParam, gripCurrent);
            triggerCurrent = 0;
            animator.SetFloat(animatorTriggerParam, triggerCurrent);
        }
    }

    //Functions called from HandController.cs to set grip and target value read from the controller
    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    //Function to start the animations on grip and trigger pressure
    void AnimateHand()
    {
        if(gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorGripParam, gripCurrent);
        }
        if (triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorTriggerParam, triggerCurrent);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Animator))]
public class NetworkHand : MonoBehaviour
{
    //Code responsible for grip and trigger animation: network correspondent of Hand.cs

    private int sceneIndex;
    public float animationSpeed;
    Animator animator;
    private float gripTarget;
    private float triggerTarget;
    public bool flag = false; //flag to check if users are doing the handshake
    private float gripCurrent;
    private float triggerCurrent;
    private string animatorGripParam = "Grip";
    private string animatorTriggerParam = "Trigger";
    private PhotonView photonView;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        //If scene index is 3 (H3) the interaction Manager on the XR Grab Interactable component of the Network Right Hand
        //needs to be set up
        if(sceneIndex == 3)
        {
            if(this.name == "Right Hand")
            {
                var interactionManager = FindObjectOfType<XRInteractionManager>();
                this.gameObject.GetComponent<XRGrabInteractable>().interactionManager = interactionManager;
            }
        }

        //Setup of the animator
        animator = GetComponent<Animator>();

        //Setup of the photon view component: in H4 the player setup is difference, hence the distinction
        if(sceneIndex == 4)
        {
            GameObject rightHand = this.gameObject.transform.parent.gameObject;
            photonView = rightHand.gameObject.GetComponentInParent<PhotonView>();
        } else
        {
            photonView = this.gameObject.GetComponentInParent<PhotonView>();
        }
    }

    void Update()
    {
        //The hands animation for the network player needs to be updated according to his local player's controllers, so it needs
        //to be updated only if the photon view component IsMine (is local)

        if (photonView.IsMine)
        {
            //Flag check used as in Hand.cs (if users are not doing the handshake the grip and trigger animations are started
            //according to the buttons pressure)

            if (flag == false)
            {
                AnimateHand();
            }
            else if (flag == true)
            {
                gripCurrent = 0;
                animator.SetFloat(animatorGripParam, gripCurrent);
                triggerCurrent = 0;
                animator.SetFloat(animatorTriggerParam, triggerCurrent);

            }
        }        
    }

    //Functions called from NetworkHandController.cs to set grip and target value read from the controller
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
        if (gripCurrent != gripTarget)
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

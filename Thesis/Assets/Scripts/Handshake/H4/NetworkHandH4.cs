using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Animator))]
public class NetworkHandH4 : MonoBehaviour
{
    private int sceneIndex;

    //Animation
    public float animationSpeed;
    Animator animator;
    private float gripTarget;
    private float triggerTarget;
    public bool flag = false;
    private float gripCurrent;
    private float triggerCurrent;
    private string animatorGripParam = "Grip";
    private string animatorTriggerParam = "Trigger";
    private PhotonView photonView;

    //Physics Movement
    /*[SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    private GameObject followObject;
    private Transform _followTarget;
    private Rigidbody _body;*/

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex == 3)
        {
            if(this.name == "Right Hand")
            {
                //var interactionManager = FindObjectOfType<XRInteractionManager>();
                //this.gameObject.GetComponent<XRGrabInteractable>().interactionManager = interactionManager;
            }
        }

        //Animation
        animator = GetComponent<Animator>();
        photonView = this.gameObject.GetComponentInParent<PhotonView>();

        //Physics Movement
        /*
        if (!photonView.IsMine) {
            if (this.gameObject.name == "LeftHand")
            {
                followObject = GameObject.Find("Camera Offset/LeftHand Controller");
            }
            else if (this.gameObject.name == "RightHand")
            {
                followObject = GameObject.Find("Camera Offset/RightHand Controller");
            }
            _followTarget = followObject.transform;
            _body = GetComponent<Rigidbody>();
            _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _body.interpolation = RigidbodyInterpolation.Interpolate;
            _body.mass = 20f;

            //Teleport hands
            _body.position = _followTarget.position;
            _body.rotation = _followTarget.rotation;
        }*/

    }

    void Update()
    {
        if (photonView.IsMine && flag == false)
        {
            AnimateHand();
        } else if (flag == true)
        {
            gripCurrent = 0;
            animator.SetFloat(animatorGripParam, gripCurrent);
            triggerCurrent = 0;
            animator.SetFloat(animatorTriggerParam, triggerCurrent);

        }

        /*if (!photonView.IsMine)
        {
            PhysicsMove();
        }*/

        
    }

    /*private void PhysicsMove()
    {
        //Position
        var positionWithOffset = _followTarget.position + positionOffset;
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        _body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);

        //Rotation
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        _body.angularVelocity = angle * axis * Mathf.Deg2Rad * rotateSpeed;
    }*/

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

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

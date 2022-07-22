using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using Photon.Pun;

[RequireComponent(typeof(Animator))]
public class NetworkHandH3 : MonoBehaviour, IPunObservable
{
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
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    private GameObject followObject;
    private Transform _followTarget;
    private Rigidbody _body;
    private PhotonRigidbodyView photonBody;

    private Vector3 body_velocity;
    private Vector3 body_angularVelocity;

    void Start()
    {
        //Animation
        animator = GetComponent<Animator>();
        photonView = this.gameObject.GetComponentInParent<PhotonView>();

        //Physics Movement
        
        if (photonView.IsMine) {
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
            //photonBody = GetComponent<PhotonRigidbodyView>();

            //Teleport hands
            _body.position = _followTarget.position;
            _body.rotation = _followTarget.rotation;
            //photonBody.transform.position = _body.position;
            //photonBody.transform.rotation = _body.rotation;
        }

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

        if (photonView.IsMine)
        {
            PhysicsMove();
        }
    }

    private void PhysicsMove()
    {
        //Position
        var positionWithOffset = _followTarget.position + positionOffset;
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        _body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);
        body_velocity = _body.velocity;

        //Rotation
        var rotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        _body.angularVelocity = angle * axis * Mathf.Deg2Rad * rotateSpeed;
        body_angularVelocity = _body.angularVelocity;
    }

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(body_velocity);
            stream.SendNext(body_angularVelocity);
        }
        else
        {
            this.body_velocity = (Vector3)stream.ReceiveNext();
            this.body_angularVelocity = (Vector3)stream.ReceiveNext();
        }
    }
}

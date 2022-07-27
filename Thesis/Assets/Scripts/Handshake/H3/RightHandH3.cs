using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandH3 : MonoBehaviour
{
    [SerializeField] private ActionBasedController controller;
    [Space]
    [SerializeField] private Transform palm;
    [SerializeField] private float reachDistance = 0.1f, joinDistance = 0.05f;
    [SerializeField] private LayerMask grabbableLayer;

    private bool _isGrabbing;
    private GameObject _heldObject;
    private Transform _grabPoint;
    private FixedJoint _joint1, _joint2;

    void Start()
    {
        //Inputs Setup
        controller.selectAction.action.started += Grab;
        controller.selectAction.action.canceled += Release;
    }

    void Update()
    {
        
    }

    private void Grab(InputAction.CallbackContext context)
    {
        Debug.Log("inside grab function");

        if (_isGrabbing || _heldObject) return;

        Collider[] grabbableColliders = Physics.OverlapSphere(palm.position, reachDistance, grabbableLayer);
        if (grabbableColliders.Length < 1) return;

        var objectToGrab = grabbableColliders[0].transform.gameObject;
        var objectBody = objectToGrab.GetComponent<Rigidbody>();

        if(objectBody != null)
        {
            _heldObject = objectBody.gameObject;
        } else
        {
            objectBody = objectToGrab.GetComponentInChildren<Rigidbody>();
            if(objectBody != null)
            {
                _heldObject = objectBody.gameObject;
            } else
            {
                return;
            }
        }

        StartCoroutine(GrabObject(grabbableColliders[0], objectBody));
    }

    private IEnumerator GrabObject(Collider collider, Rigidbody targetBody)
    {
        _isGrabbing = true;

        //Create a Grab Point
        _grabPoint = new GameObject().transform;
        _grabPoint.position = collider.ClosestPoint(palm.position);
        _grabPoint.parent = _heldObject.transform;
        
        //Move hand to grab point
        

        //Wait for Hand to reach grab point
        while(_grabPoint != null && Vector3.Distance(_grabPoint.position, palm.position) > joinDistance && _isGrabbing)
        {
            yield return new WaitForEndOfFrame();
        }

        //Freeze hand and object motion
        targetBody.velocity = Vector3.zero;
        targetBody.angularVelocity = Vector3.zero;

        targetBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        targetBody.interpolation = RigidbodyInterpolation.Interpolate;

        //Attach joints
        _joint1 = gameObject.AddComponent<FixedJoint>();
        _joint1.connectedBody = targetBody;
        _joint1.breakForce = float.PositiveInfinity;
        _joint1.breakTorque = float.PositiveInfinity;

        _joint1.connectedMassScale = 1;
        _joint1.massScale = 1;
        _joint1.enableCollision = false;
        _joint1.enablePreprocessing = false;
        
        /*_joint2 = _heldObject.AddComponent<FixedJoint>();
        _joint2.connectedBody = targetBody;
        _joint2.breakForce = float.PositiveInfinity;
        _joint2.breakTorque = float.PositiveInfinity;
              
        _joint2.connectedMassScale = 1;
        _joint2.massScale = 1;
        _joint2.enableCollision = false;
        _joint2.enablePreprocessing = false;*/

        //Reset follow target
    }

    private void Release(InputAction.CallbackContext context)
    {
        Debug.Log("inside release function");

        if (_joint1 != null)
        {
            Destroy(_joint1);
        }
        if (_joint2 != null)
        {
            Destroy(_joint2);
        }
        if (_grabPoint != null)
        {
            Destroy(_grabPoint.gameObject);
        }
        if(_heldObject != null)
        {
            var targetObject = _heldObject.GetComponent<Rigidbody>();
            targetObject.collisionDetectionMode = CollisionDetectionMode.Discrete;
            targetObject.interpolation = RigidbodyInterpolation.None;
            _heldObject = null;
        }

        _isGrabbing = false;
        
    }
}

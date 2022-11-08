using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class OnCollisionMovePlayer : MonoBehaviour
{
    //private Vector3 _lookAt;
    private Vector3 direction;
    private float distance;

    private XROrigin origin;
    private GameObject originObject;
    //private GameObject head;

    private PhotonView colliderParentPhotonView;
    private GameObject colliderParent;

    void Start()
    {
        distance = 0.2f;

        origin = FindObjectOfType<XROrigin>();
        originObject = origin.gameObject;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Bubble")
        {
            colliderParent = collider.transform.parent.gameObject;
            colliderParentPhotonView = colliderParent.transform.GetComponent<PhotonView>();
            if (!colliderParentPhotonView.IsMine)
            {
                direction = (colliderParent.transform.position - Camera.main.transform.position).normalized;
                if (originObject != null)
                {
                    originObject.transform.position += new Vector3(0, 0, -direction.z * distance);
                }                
            }
        }        
    }
}

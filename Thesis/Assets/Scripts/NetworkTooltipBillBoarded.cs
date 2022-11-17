using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using Photon.Pun;

public class NetworkTooltipBillBoarded : MonoBehaviour
{
    private Vector3 _lookAt;
    [Range(10,100)]public float ElasticFactor = 0;

    private PhotonView photonView;

    private void Start()
    {
        photonView = this.transform.parent.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!photonView.IsMine)
        {
            _lookAt = Camera.main.transform.position;
            _lookAt.y = transform.position.y;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(transform.position - _lookAt, Vector3.up), Time.deltaTime * ElasticFactor);
            //Debug.Log($"local position {transform.localPosition}");
            //Debug.Log($"position {transform.position}");
            //Debug.Log($"local rotation {transform.localRotation}");
            //Debug.Log($"rotation {transform.rotation}");
            //Debug.Log($"final rotation {transform.position - _lookAt}");
        }        
    }
}

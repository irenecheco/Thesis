using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkTooltipBillBoarded : MonoBehaviour
{
    private Vector3 _lookAt;
    [Range(10,100)]public float ElasticFactor = 0;
    private int sceneIndex;

    private PhotonView photonView;

    private void Start()
    {
        photonView = this.transform.parent.gameObject.GetComponent<PhotonView>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            _lookAt = Camera.main.transform.position;
            _lookAt.y = transform.position.y;
            if (sceneIndex != 3)
            {
                this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(this.transform.position - _lookAt, Vector3.up), Time.deltaTime * ElasticFactor);
            } else
            {
                this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_lookAt - this.transform.position, Vector3.up), Time.deltaTime * ElasticFactor);
            }
        }        
    }
}

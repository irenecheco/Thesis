using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TooltipBillBoarded : MonoBehaviour
{
    private int sceneIndex;
    private Vector3 _lookAt;
    [Range(10,100)]public float ElasticFactor = 0;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _lookAt = Camera.main.transform.position;
        _lookAt.y = transform.position.y;
        if(sceneIndex != 3)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - _lookAt, Vector3.up), Time.deltaTime * ElasticFactor);
        } else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_lookAt - transform.position, Vector3.up), Time.deltaTime * ElasticFactor);
        }
    }
}

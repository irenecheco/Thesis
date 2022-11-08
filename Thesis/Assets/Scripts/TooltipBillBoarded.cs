using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class TooltipBillBoarded : MonoBehaviour
{
    private Vector3 _lookAt;
    [Range(10,100)]public float ElasticFactor = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _lookAt = Camera.main.transform.position;
        _lookAt.y = transform.position.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - _lookAt, Vector3.up), Time.deltaTime * ElasticFactor);
    }
}

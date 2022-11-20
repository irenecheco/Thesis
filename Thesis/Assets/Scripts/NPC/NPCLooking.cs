using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLooking : MonoBehaviour
{
    private Vector3 _lookAt;
    [Range(10, 100)] public float ElasticFactor = 0;

    // Update is called once per frame
    void Update()
    {
        _lookAt = Camera.main.transform.position;
        _lookAt.y = transform.position.y;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - _lookAt, Vector3.up), Time.deltaTime * ElasticFactor);
    }
}

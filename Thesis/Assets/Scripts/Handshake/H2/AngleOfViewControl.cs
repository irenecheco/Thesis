using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleOfViewControl : MonoBehaviour
{
    private Vector3 direction;
    private GameObject otherHead;
    private GameObject camera;

    private float angle;
    public bool isLooking;

    void Start()
    {
        camera = Camera.main.gameObject;
        otherHead = this.transform.GetChild(0).gameObject;
        isLooking = false;
    }

    void Update()
    {
        direction = (otherHead.transform.position - camera.transform.position);
        angle = Vector3.Angle(direction, camera.transform.forward);
        if (angle <= 45)
        {
            isLooking = true;
        } else
        {
            isLooking = false;
        }
    }
}

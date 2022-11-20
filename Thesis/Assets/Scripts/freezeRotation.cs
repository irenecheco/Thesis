using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezeRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, this.transform.eulerAngles.y, 0);
    }
}

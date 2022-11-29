using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ResetPositionIfCreator : MonoBehaviour
{
    private GameObject originObject;

    [SerializeField] private Transform newPosition;

    // Start is called before the first frame update
    void Start()
    {
        originObject = this.gameObject;

        if(MyUserControl.iAmThisUser == true)
        {
            originObject.transform.position = new Vector3(newPosition.position.x, originObject.transform.position.y, newPosition.position.z);

        }
    }
}

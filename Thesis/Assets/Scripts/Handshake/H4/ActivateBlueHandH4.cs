using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBlueHandH4 : MonoBehaviour
{
    public bool isReady;
    public bool isCollidingWithMayor;

    [SerializeField] private GameObject local_player_head;
    [SerializeField] private GameObject mayor_head_canvas;
    [SerializeField] private GameObject mayor_rightMesh;

    private System.DateTime initialTimeH4Mayor;

    private Color waitingColor = new Color(0.4135279f, 0.7409829f, 0.9056604f, 1.0f);
    private Color baseColor = new Color(0.8000001f, 0.4848836f, 0.3660862f, 1.0f);

    void Start()
    {
        isReady = false;
        isCollidingWithMayor = false;
        local_player_head = Camera.main.gameObject;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == local_player_head)
        {
            isCollidingWithMayor = true;
            if (isReady)
            {
                InteractionsCount.startedInteractionsFromMayorH4++;
                initialTimeH4Mayor = System.DateTime.UtcNow;
                mayor_rightMesh.transform.parent.transform.parent.GetComponent<GrabbingNPC>().initialTimeH4Mayor = initialTimeH4Mayor;
                mayor_rightMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
                mayor_head_canvas.GetComponent<Canvas>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == local_player_head)
        {
            isCollidingWithMayor = false;
            if (isReady)
            {
                mayor_rightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                mayor_head_canvas.GetComponent<Canvas>().enabled = false;
            }
        }
    }
}

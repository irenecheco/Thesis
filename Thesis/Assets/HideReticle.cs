using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HideReticle : MonoBehaviour
{
    private GameObject leftController;
    private GameObject reticle;

    void Start()
    {
        leftController = this.gameObject;
        reticle = GameObject.Find("Teleport Reticle");
        Debug.Log($"{reticle.gameObject.name}");
    }

    void Update()
    {
        leftController.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out RaycastHit hit);
        if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            leftController.GetComponent<XRInteractorLineVisual>().RemoveCustomReticle();
        }
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            leftController.GetComponent<XRInteractorLineVisual>().AttachCustomReticle(reticle);
        }
    }
}

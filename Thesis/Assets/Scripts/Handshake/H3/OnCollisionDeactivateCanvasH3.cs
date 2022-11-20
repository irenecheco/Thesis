using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class OnCollisionDeactivateCanvasH3 : MonoBehaviourPunCallbacks
{
    //Code responsible for activating the handshake button when two players collide

    private GameObject messageCanvas;
    private GameObject rightHandController;
    private GameObject rightHand;

    public GameObject otherPlayerHead;
    public GameObject otherRightHand;
    private GameObject otherRightMesh;
    [SerializeField] private GameObject localNetRightHand;

    public bool firstExited;
    private bool wasGrabbed;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    public void Start()
    {
        rightHandController = GameObject.Find("Camera Offset/RightHand Controller");
        rightHand = rightHandController.transform.FindChildRecursive("RightHand").gameObject;
        firstExited = true;
        wasGrabbed = false;
    }

    //Function called on trigger entered: it activates the handshake button only if the two heads collide
    private void OnTriggerExit(Collider collider)
    {
        if (this.transform.parent.GetComponent<PhotonView>().IsMine)
        {
            if (collider.gameObject.name == "Head")
            {                   
                otherPlayerHead = collider.gameObject;
                PhotonView colliderPhotonView;
                colliderPhotonView = otherPlayerHead.transform.GetComponent<PhotonView>();
                if (!colliderPhotonView.IsMine)
                {
                    if (firstExited)
                    {
                        //Debug.Log("Exit collision");
                        messageCanvas = otherPlayerHead.transform.GetChild(2).gameObject;
                        messageCanvas.GetComponent<Canvas>().enabled = false;
                        messageCanvas.GetComponent<AudioSource>().enabled = false;
                        rightHand.GetComponent<GrabbingH3>().isColliding = false;
                        
                        if (otherRightHand != null)
                        {
                            otherRightMesh = otherRightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
                            /*otherRightHand.transform.localPosition = new Vector3(0, 0, 0);
                            otherRightHand.transform.localRotation = new Quaternion(0, 0, 0, 0);
                            otherRightHand.transform.localRotation = Quaternion.Euler(0, 0, -90);*/
                            /*if (otherRightHand.GetComponent<MessageActivationH3>().isGrabbed)
                            {
                                wasGrabbed = true;*/
                                //otherRightHand.GetComponent<MessageActivationH3>().DeactivateMessage();
                                //rightHandController.GetComponent<XRDirectInteractor>().allowSelect = false;
                                //rightHandController.GetComponent<ActionBasedController>().enableInputTracking = true;
                                //otherRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                                /*localNetRightHand.GetComponent<MessageActivationH3>().isGrabbing = false;
                                 * //otherRightHand.GetComponent<MessageActivationH3>().isGrabbed = false;
                                */
                                //Invoke("SetPosition()", 0.25f);
                            //}
                            //otherRightHand.transform.localPosition = new Vector3(0, 0, 0);
                            //otherRightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                            //Debug.Log($"{otherRightHand.GetComponent<MessageActivationH3>().isGrabbing} and other hand is {otherRightHand.gameObject.name}");
                        }

                        rightHandController.GetComponent<XRDirectInteractor>().allowSelect = false;
                        rightHandController.GetComponent<ActionBasedController>().enableInputTracking = true;
                        localNetRightHand.GetComponent<MessageActivationH3>().isGrabbing = false;

                        Invoke("SetPosition", 0.5f);
                        /*if (wasGrabbed == true)
                        {
                            Debug.Log("entra qui");*/
                            /*otherRightHand.transform.localPosition = new Vector3(0, 0, 0);
                            otherRightHand.transform.localRotation = new Quaternion(0, 0, 0, 0);
                            otherRightHand.transform.localRotation = Quaternion.Euler(0, 0, -90);*/
                            //wasGrabbed = false;
                        //}

                        firstExited = false;
                        this.transform.parent.transform.FindChildRecursive("ActivateCollider").gameObject.GetComponent<OnCollisionActivateCanvasH3>().firstEntered = true;
                    }                    
                }
            }
        }
    }

    private void SetPosition()
    {
        if (otherRightHand != null)
        {
            otherRightHand.transform.localPosition = new Vector3(0, 0, 0);
            otherRightHand.transform.localRotation = new Quaternion(0, 0, 0, 0);
            otherRightHand.transform.localRotation = Quaternion.Euler(0, 0, -90);
        }        
    }
}

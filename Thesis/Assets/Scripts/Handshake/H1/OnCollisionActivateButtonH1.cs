using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class OnCollisionActivateButtonH1 : MonoBehaviourPunCallbacks
{
    //Code responsible for activating the handshake button when two players collide

    public GameObject otherPlayerHead;
    private GameObject handshakeUI_l;
    private GameObject handshakeUI_r;
    private GameObject waitingUI_l;
    private GameObject waitingUI_r;
    private GameObject leftHand;
    private GameObject rightHand;

    private XROrigin origin;

    private int sceneIndex;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex == 1)
        {
            handshakeUI_l = GameObject.Find("Camera Offset/LeftHand Controller/LeftHand/Handshake UI");
            handshakeUI_r = GameObject.Find("Camera Offset/RightHand Controller/RightHand/Handshake UI");
            origin = FindObjectOfType<XROrigin>();
            handshakeUI_l.GetComponent<Canvas>().enabled = false;
            handshakeUI_r.GetComponent<Canvas>().enabled = false;
            leftHand = handshakeUI_l.transform.parent.gameObject;
            rightHand = handshakeUI_r.transform.parent.gameObject;
            waitingUI_l = leftHand.transform.GetChild(3).gameObject;
            waitingUI_r = rightHand.transform.GetChild(3).gameObject;
        }
    }

    //Function called on trigger entered: it activates the handshake button only if the two heads collide
    private void OnTriggerEnter(Collider collider)
    {
        if(sceneIndex == 1)
        {
            if (collider.gameObject.name == "Head")
            {
                PhotonView colliderPhotonView;
                colliderPhotonView = collider.transform.GetComponent<PhotonView>();
                if (!colliderPhotonView.IsMine)
                {
                    otherPlayerHead = collider.gameObject;
                    origin.GetComponent<ActiveHandController>().isColliding = true;

                    if (leftHand.transform.parent.gameObject.GetComponent<XRRayInteractor>().enabled == true)
                    {
                        if (waitingUI_l.GetComponent<Canvas>().enabled == false)
                        {
                            handshakeUI_l.GetComponent<Canvas>().enabled = true;
                            handshakeUI_l.GetComponent<AudioSource>().enabled = true;

                            leftHand.GetComponent<HapticController>().amplitude = 0.2f;
                            leftHand.GetComponent<HapticController>().duration = 0.2f;
                            leftHand.GetComponent<HapticController>().SendHaptics();
                            handshakeUI_l.GetComponent<AudioSource>().Play();
                            GameObject buttonL = handshakeUI_l.transform.GetChild(1).gameObject;
                            buttonL.GetComponent<HandshakeButton>().collidingPlayerHead = otherPlayerHead;
                            handshakeUI_r.GetComponent<AudioSource>().Play();
                            GameObject buttonR = handshakeUI_r.transform.GetChild(1).gameObject;
                            buttonR.GetComponent<HandshakeButton>().collidingPlayerHead = otherPlayerHead;
                        }
                    } else
                    {
                        if (waitingUI_r.GetComponent<Canvas>().enabled == false)
                        {
                            handshakeUI_r.GetComponent<Canvas>().enabled = true;
                            handshakeUI_r.GetComponent<AudioSource>().enabled = true;

                            rightHand.GetComponent<HapticController>().amplitude = 0.2f;
                            rightHand.GetComponent<HapticController>().duration = 0.2f;
                            rightHand.GetComponent<HapticController>().SendHaptics();
                            handshakeUI_r.GetComponent<AudioSource>().Play();
                            GameObject buttonR = handshakeUI_r.transform.GetChild(1).gameObject;
                            buttonR.GetComponent<HandshakeButton>().collidingPlayerHead = otherPlayerHead;
                            handshakeUI_l.GetComponent<AudioSource>().Play();
                            GameObject buttonL = handshakeUI_l.transform.GetChild(1).gameObject;
                            buttonL.GetComponent<HandshakeButton>().collidingPlayerHead = otherPlayerHead;
                        }
                    }                                        
                }
            }
        }               
    }
}

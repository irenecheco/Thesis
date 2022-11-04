using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class OnCollisionActivateButtonH1 : MonoBehaviourPunCallbacks
{
    //Code responsible for activating the handshake button when two players collide

    public GameObject otherPlayerHead;
    private string handshake_button = "Handshake Button";
    private GameObject button;
    private GameObject handshakeUI;
    private GameObject waitingUI;
    private GameObject leftHand;

    private int sceneIndex;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex == 1)
        {
            button = GameObject.Find(handshake_button).gameObject;
            button.GetComponent<Button>().interactable = false;
            handshakeUI = button.transform.parent.gameObject;
            handshakeUI.GetComponent<Canvas>().enabled = false;
            leftHand = handshakeUI.transform.parent.gameObject;
            waitingUI = leftHand.transform.GetChild(3).gameObject;          
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

                    if(waitingUI.GetComponent<Canvas>().enabled == false)
                    {
                        handshakeUI.GetComponent<Canvas>().enabled = true;
                        button.transform.GetComponent<Button>().interactable = true;
                        handshakeUI.GetComponent<AudioSource>().enabled = true;

                        leftHand.GetComponent<HapticController>().amplitude = 0.2f;
                        leftHand.GetComponent<HapticController>().duration = 0.2f;
                        leftHand.GetComponent<HapticController>().SendHaptics();
                        handshakeUI.GetComponent<AudioSource>().Play();
                    }                    
                }
            }
        }               
    }

    //Function called on trigger exited: it disables the handshake button if the two heads does not collide anymore
    private void OnTriggerExit(Collider collider)
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

                    handshakeUI.GetComponent<AudioSource>().enabled = false;
                    button.transform.GetComponent<Button>().interactable = false;
                    handshakeUI.GetComponent<Canvas>().enabled = false;
                }
            }
        }        
    }
}

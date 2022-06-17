using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class OnCollisionActivateButtonH1 : MonoBehaviourPunCallbacks
{
    public GameObject otherPlayerHead;
    private string handshake_button = "Handshake Button";

    private int sceneIndex;
    //private PhotonView photonViewParent;

    // Start is called before the first frame update
    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex == 1)
        {
            GameObject.Find(handshake_button).GetComponent<Button>().interactable = false;
            //photonViewParent = GetComponent<PhotonView>();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(sceneIndex == 1)
        {
            if (collider.gameObject.name == "Head")
            {
                //Debug.Log($"{collider.transform.GetComponent<PhotonView>()}");
                PhotonView colliderPhotonView;
                colliderPhotonView = collider.transform.GetComponent<PhotonView>();
                if (!colliderPhotonView.IsMine)
                {
                    otherPlayerHead = collider.gameObject;

                    GameObject.Find(handshake_button).transform.GetComponent<Button>().interactable = true;
                }
            }
        }               
    }

    private void OnTriggerStay(Collider collider)
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

                    GameObject.Find(handshake_button).transform.GetComponent<Button>().interactable = true;
                }
            }
        }        
    }

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

                    GameObject.Find(handshake_button).transform.GetComponent<Button>().interactable = false;
                }
            }
        }        
    }
}

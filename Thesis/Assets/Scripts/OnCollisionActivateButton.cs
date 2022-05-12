using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class OnCollisionActivateButton : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject otherPlayerHead;
    private string handshake_button = "Handshake Button";
    //private PhotonView photonViewParent;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find(handshake_button).GetComponent<Button>().interactable = false;
        //photonViewParent = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        
        if (collider.gameObject.name == "Head")
        {
            //Debug.Log($"{collider.transform.GetComponent<PhotonView>()}");
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                GameObject.Find(handshake_button).GetComponent<Button>().interactable = true;
            }
        }
        
    }

    private void OnTriggerStay(Collider collider)
    {        
        if (collider.gameObject.name == "Head")
        {
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                GameObject.Find(handshake_button).GetComponent<Button>().interactable = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {

        if (collider.gameObject.name == "Head")
        {
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                GameObject.Find(handshake_button).GetComponent<Button>().interactable = false;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //sync interface
    }
}

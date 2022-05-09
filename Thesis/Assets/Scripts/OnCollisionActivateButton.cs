using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class OnCollisionActivateButton : MonoBehaviourPunCallbacks, IPunObservable
{
    private string handshake_button = "Handshake Button";
    private PhotonView photonViewParent;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find(handshake_button).GetComponent<Button>().interactable = false;
        //photonViewParent = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Collision triggered");
        /*if (!photonViewParent.IsMine)
        {
            return;
        }*/
        GameObject.Find(handshake_button).GetComponent<Button>().interactable = true;
       
    }

    private void OnTriggerExit(Collider other)
    {

        /*if (!photonViewParent.IsMine)
        {
            return;
        }*/
        GameObject.Find(handshake_button).GetComponent<Button>().interactable = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //sync interface
    }
}

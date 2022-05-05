using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class OnCollisionActivateButton : MonoBehaviourPunCallbacks, IPunObservable
{
    private string handshake_button = "Handshake Button";
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void onTriggerEnter(Collider other)
    {
        Debug.Log("Collision triggered");
        if (!photonView.IsMine)
        {
            return;
        }
        GameObject.Find(handshake_button).GetComponent<Button>().interactable = true;
       
    }

    private void onTriggerExit(Collider other)
    {

        if (!photonView.IsMine)
        {
            return;
        }
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

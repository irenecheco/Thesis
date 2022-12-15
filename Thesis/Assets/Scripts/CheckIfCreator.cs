using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfCreator : MonoBehaviour, IPunObservable
{
    private bool iAmCreator = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(iAmCreator);
        }
        else
        {
            this.iAmCreator = (bool)stream.ReceiveNext();
        }
    }

    void Start()
    {
        if (MyUserControl.iAmThisUser == true)
        {
            if (this.GetComponent<PhotonView>().IsMine)
            {
                iAmCreator = true;
            }            
        }
        if (iAmCreator)
        {
            this.GetComponent<ChangeColorIfCreator>().enabled = true;
        }        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandsOrControllersH4 : MonoBehaviour, IPunObservable
{
    public bool handsOn;
    private PhotonView photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(handsOn);
        }
        else
        {
            this.handsOn = (bool)stream.ReceiveNext();
        }
    }

    void Start()
    {
        photonView = this.GetComponent<PhotonView>();
        handsOn = false;
    }

    void Update()
    {
        if (photonView.IsMine)
        {//Debug.Log($"handsOn is {handsOn}");
            if (OVRPlugin.GetHandTrackingEnabled() == true)
            {
                handsOn = true;
            }
            else
            {
                handsOn = false;
            }
        }        
    }
}

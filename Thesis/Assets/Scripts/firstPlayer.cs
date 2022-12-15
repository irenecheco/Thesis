using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class firstPlayer : MonoBehaviour, IPunObservable
{
    public bool isFirstPlayer;
    //public int countPl;

    private void Start()
    {
        if (NetworkManager.isEven == false)
        {
            isFirstPlayer = true;
        }
        else
        {
            isFirstPlayer = false;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isFirstPlayer);
        }
        else
        {
            this.isFirstPlayer = (bool)stream.ReceiveNext();
        }
    }
}

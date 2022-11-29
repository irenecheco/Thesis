using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeColorIfCreator : MonoBehaviour, IPunObservable
{
    private GameObject netPlayer;
    private GameObject netHeadMesh;
    private GameObject netRightMesh;
    private GameObject netLeftMesh;
    private PhotonView photonView;
    private GameObject neededObject;

    private int flag = 0;
    private string localId;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color yellowColor = new Color(0.8679245f, 0.8271183f, 0.4208615f, 1.0f);
    private Color greenColor = new Color(0.4291207f, 0.7924528f, 0.6037189f, 1.0f);

    [SerializeField] private InputActionReference _changeColor;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(flag);
        }
        else
        {
            this.flag = (int)stream.ReceiveNext();
        }
    }

    void Start()
    {
        netPlayer = this.gameObject;
        netRightMesh = netPlayer.transform.FindChildRecursive("RightHand").transform.FindChildRecursive("hands:Lhand").gameObject;
        netLeftMesh = netPlayer.transform.FindChildRecursive("LeftHand").transform.FindChildRecursive("hands:Lhand").gameObject;
        photonView = this.GetComponent<PhotonView>();

        _changeColor.action.performed += ctx =>
        {
            //CheckFlag();
            if (photonView.IsMine)
            {
                localId = PhotonNetwork.LocalPlayer.UserId;
                photonView.RPC("CheckFlag", RpcTarget.All, localId as string);
            }
        };

    }

    /*public void CheckFlag()
    {
        if (flag == 0)
        {
            netHeadMesh.GetComponent<MeshRenderer>().material.color = yellowColor;
            flag = 1;
        }
        else if (flag == 1)
        {
            netHeadMesh.GetComponent<MeshRenderer>().material.color = greenColor;
            flag = 2;
        }
        else if (flag == 2)
        {
            netHeadMesh.GetComponent<MeshRenderer>().material.color = baseColor;
            flag = 0;
        }
    }*/

    [PunRPC]
    public void CheckFlag(string playerId)
    {
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.UserId == playerId)
            {
                neededObject = (GameObject)item.TagObject;
            }
        }
        if(neededObject != null)
        {
            if (neededObject == this.gameObject)
            {
                if (flag == 0)
                {
                    neededObject.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color = yellowColor;
                    flag = 1;
                }
                else if (flag == 1)
                {
                    neededObject.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color = greenColor;
                    flag = 2;
                }
                else if (flag == 2)
                {
                    neededObject.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color = baseColor;
                    flag = 0;
                }
            }
        }        
    }
}

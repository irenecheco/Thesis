using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NLog.Unity;

public class HandshakeActivation : MonoBehaviour
{
    //Code responsible for handshake activation in H1

    private GameObject confirmCanvas;
    private GameObject confirmHead;
    private GameObject confirmPlayer;
    private string player1ID;
    private string player2ID;

    public System.DateTime initialTimeH1Player;
    private System.DateTime finalTimeH1Player;

    private Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color yellowColor = new Color(0.8679245f, 0.8271183f, 0.4208615f, 1.0f);
    private Color greenColor = new Color(0.4291207f, 0.7924528f, 0.6037189f, 1.0f);

    void Start()
    {
        confirmCanvas = this.gameObject.transform.parent.gameObject;
        confirmHead = confirmCanvas.transform.parent.gameObject;
        confirmPlayer = confirmHead.transform.parent.gameObject;
    }

    //Function called when confirm button pressed: it saves the ids and call the methed to activate the animation over the network
    public void CallHeadMethod()
    {
        foreach(var item in PhotonNetwork.PlayerList)
        {
            if((object)item.TagObject == confirmPlayer)
            {
                //Debug.Log($"{item.UserId}");
                player1ID = item.UserId;
            }
        }

        player2ID = PhotonNetwork.LocalPlayer.UserId;

        confirmPlayer.GetComponent<NetworkHandshakeActivationH1>().CallActivationOverNetwork(player1ID, player2ID);

        if (!confirmHead.GetComponent<PhotonView>().IsMine)
        {
            InteractionsCount.finishedInteractionsWithExperimenterH1++;
            finalTimeH1Player = System.DateTime.UtcNow;
            if(confirmHead.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color == baseColor)
            {
                NLogConfig.LogLine($"{"White_Version"};TimeFromCanvasAppearing;{(finalTimeH1Player - initialTimeH1Player).TotalSeconds.ToString("#.000")};s");
            } else if(confirmHead.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color == yellowColor)
            {
                NLogConfig.LogLine($"{"Yellow_Version"};TimeFromCanvasAppearing;{(finalTimeH1Player - initialTimeH1Player).TotalSeconds.ToString("#.000")};s");
            } else if(confirmHead.transform.FindChildRecursive("Sphere").gameObject.GetComponent<MeshRenderer>().material.color == greenColor)
            {
                NLogConfig.LogLine($"{"Green_Version"};TimeFromCanvasAppearing;{(finalTimeH1Player - initialTimeH1Player).TotalSeconds.ToString("#.000")};s");
            }
            
        }
    }
}

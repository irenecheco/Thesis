using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class NetworkHandshakeFakeHand : MonoBehaviour
{
    //Code responsible for starting the animation between network fake hands

    private float ending_y;
    //private float y_angle;
    private float time = (float)0.75;
    private Vector3 midPosition;
    private Vector3 startingPosition;
    private Quaternion direction;
    private Quaternion startingRotation;

    [SerializeField] private GameObject rightController;
    private GameObject fakeHand_holder;
    private GameObject localPlayer;
    private GameObject firstPlayer;

    private Animator fakeHandAnimator;

    public double x;
    public double z;
    public double y;

    private void Awake()
    {
        fakeHand_holder = this.transform.parent.gameObject;
        fakeHandAnimator = this.GetComponent<Animator>();

        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                localPlayer = (GameObject)item.TagObject;
            }
        }
    }

    //Function invoked when handshake needs to start
    public void DoHandshake(Vector3 myPosition, Vector3 otherPosition, bool firstConfirming)
    {
        if (myPosition.y <= otherPosition.y)
        {
            ending_y = myPosition.y;
        }
        else
        {
            ending_y = otherPosition.y;
        }

        midPosition = Vector3.Lerp(otherPosition, myPosition, 0.5f);

        startingPosition = rightController.transform.position;
        startingRotation = rightController.transform.rotation;

        fakeHand_holder.transform.position = startingPosition;
        fakeHand_holder.transform.rotation = startingRotation;

        direction = Quaternion.LookRotation((otherPosition - myPosition), Vector3.up);
        /*direction.x = 0;
        direction.z = 0;*/

        fakeHand_holder.transform.DORotateQuaternion(direction, time);
        
        
        if (firstConfirming)
        {
            Debug.Log("first confirming true");
            if(localPlayer.GetComponent<firstPlayer>().isFirstPlayer)
            {
                Debug.Log("first confirming true e primo giocatore");
                fakeHand_holder.transform.DOMove(new Vector3((midPosition.x - (float)0.005), (float)(ending_y - 0.4), (midPosition.z - (float)0.045)), time);
                //fakeHand_holder.transform.DOMove(new Vector3((midPosition.x + (float)0.005), (float)(ending_y - 0.4), (midPosition.z + (float)0.045)), time);
            } else
            {
                Debug.Log("first confirming true e secondo giocatore");
                //fakeHand_holder.transform.DOMove(new Vector3((midPosition.x - (float)0.005), (float)(ending_y - 0.4), (midPosition.z - (float)0.045)), time);
                fakeHand_holder.transform.DOMove(new Vector3((midPosition.x + (float)0.005), (float)(ending_y - 0.4), (midPosition.z + (float)0.045)), time);
            }
            
        }
        else
        {
            Debug.Log("first confirming false");
            if (localPlayer.GetComponent<firstPlayer>().isFirstPlayer)
            {
                Debug.Log("first confirming false e primo giocatore");
                fakeHand_holder.transform.DOMove(new Vector3((midPosition.x - (float)0.005), (float)(ending_y - 0.4), (midPosition.z - (float)0.045)), time);
                //fakeHand_holder.transform.DOMove(new Vector3((midPosition.x + (float)0.005), (float)(ending_y - 0.4), (midPosition.z + (float)0.045)), time);
            }
            else
            {
                Debug.Log("first confirming false e secondo giocatore");
                fakeHand_holder.transform.DOMove(new Vector3((midPosition.x + (float)0.005), (float)(ending_y - 0.4), (midPosition.z + (float)0.045)), time);
                //fakeHand_holder.transform.DOMove(new Vector3((midPosition.x - (float)0.005), (float)(ending_y - 0.4), (midPosition.z - (float)0.045)), time);
            }
        }
        
        //fakeHand_holder.transform.DOMove(new Vector3((midPosition.x), (float)(ending_y - 0.39), (midPosition.z)), time);

        Invoke("SecondPartHandshake", time);
    }

    //Once the fake hands are in position, this function triggers the up-and-down animation
    public void SecondPartHandshake()
    {
        fakeHandAnimator.Play("Handshake", -1, 0);
        //fakeHandAnimator.speed = 0;
    }
}

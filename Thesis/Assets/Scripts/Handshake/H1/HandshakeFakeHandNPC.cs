using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandshakeFakeHandNPC : MonoBehaviour
{
    //Code responsible for starting the animation of NPC fake hand

    private float ending_y;
    private float y_angle;
    private float time = (float)0.75;
    private Vector3 midPosition;
    private Vector3 startingPosition;
    private Quaternion direction;
    private Quaternion startingRotation;

    private GameObject fakeHand_holder;

    private Animator fakeHandAnimator;

    private void Awake()
    {
        fakeHand_holder = this.transform.parent.gameObject;
        fakeHandAnimator = this.GetComponent<Animator>();
    }

    //Function invoked when handshake needs to start
    public void DoHandshake(Vector3 NPCPosition, Vector3 playerPosition, GameObject NPCHand_holder)
    {
        if (NPCPosition.y <= playerPosition.y)
        {
            ending_y = NPCPosition.y;
        }
        else
        {
            ending_y = playerPosition.y;
        }

        midPosition = Vector3.Lerp(playerPosition, NPCPosition, 0.5f);

        startingPosition = NPCHand_holder.transform.position;
        startingRotation = NPCHand_holder.transform.rotation;

        fakeHand_holder.transform.position = startingPosition;
        fakeHand_holder.transform.rotation = startingRotation * Quaternion.Euler(0, 0, 90);

        direction = Quaternion.LookRotation((playerPosition - NPCPosition), Vector3.up);

        fakeHand_holder.transform.DORotateQuaternion(direction, time);
        fakeHand_holder.transform.DOMove(new Vector3((midPosition.x - (float) 0.017), (float)(ending_y - 0.4), (midPosition.z + (float)0.015)), time);

        Invoke("SecondPartHandshake", time);
    }

    //Once the fake hands are in position, this function triggers the up-and-down animation
    public void SecondPartHandshake()
    {
        fakeHandAnimator.Play("Handshake", -1, 0);
    }
}

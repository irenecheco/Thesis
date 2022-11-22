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
    public double x;
    public double z;

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
        /*direction.x = 0;
        direction.z = 0;*/

        fakeHand_holder.transform.DORotateQuaternion(direction, time);
        fakeHand_holder.transform.DOMove(new Vector3((midPosition.x - (float)0.042), (float)(ending_y - 0.39), (midPosition.z + (float)0.01)), time);
        //fakeHand_holder.transform.DOMove(new Vector3((midPosition.x), (float)(ending_y - 0.4), (midPosition.z)), time);
        //fakeHand_holder.transform.DOMove(new Vector3((midPosition.x - (float)0.017), (float)(ending_y - 0.4), (midPosition.z - (float)0.015)), time);
        //fakeHand_holder.transform.DOMove(new Vector3((midPosition.x - (float)x), (float)(ending_y - 0.4), (midPosition.z - (float)z)), time);

        Invoke("SecondPartHandshake", time);
    }

    //Once the fake hands are in position, this function triggers the up-and-down animation
    public void SecondPartHandshake()
    {
        /*Sequence shakeSequence = DOTween.Sequence();
        shakeSequence
            .Append(fakeHand_holder.transform.DOMoveY(fakeHand_holder.transform.position.y - (float)0.100, (float)0.25))
            .Append(fakeHand_holder.transform.DOMoveY(fakeHand_holder.transform.position.y + (float)0.200, (float)0.25))
            .Append(fakeHand_holder.transform.DOMoveY(fakeHand_holder.transform.position.y - (float)0.100, (float)0.25))
            .Append(fakeHand_holder.transform.DOMoveY(fakeHand_holder.transform.position.y + (float)0.200, (float)0.25))
            .Append(fakeHand_holder.transform.DOMoveY((float)(ending_y - 0.4), (float)0.25))
            .Play();*/
        //fakeHand_holder.transform.rotation *= Quaternion.Euler(-8, 4, -24);
        fakeHandAnimator.Play("Handshake", -1, 0);
        //fakeHandAnimator.speed = 0;
    }
}

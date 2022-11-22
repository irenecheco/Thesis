using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandshakeFakeHand : MonoBehaviour
{
    //Code responsible for starting the animation between fake hands

    private float ending_y;
    private float time = (float)0.75;
    private Vector3 midPosition;
    private Vector3 startingPosition;
    private Quaternion direction;
    private Quaternion startingRotation;

    public double x;
    public double z;
    public double y;

    [SerializeField] private GameObject rightController;
    private GameObject fakeHand_holder;

    private Animator fakeHandAnimator;

    private void Awake()
    {
        fakeHand_holder = this.transform.parent.gameObject;
        fakeHandAnimator = this.GetComponent<Animator>();
    }

    //Function invoked when handshake needs to start
    public void DoHandshake(Vector3 myPosition, Vector3 otherPosition)
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

        fakeHand_holder.transform.DORotateQuaternion(direction * Quaternion.AngleAxis((float)10.0, transform.forward), time);
        //fakeHand_holder.transform.DOMove(new Vector3((midPosition.x + (float) 0.017), (float)(ending_y - 0.4), (midPosition.z - (float)0.015)), time);
        fakeHand_holder.transform.DOMove(new Vector3((midPosition.x), (float)(ending_y - 0.4), (midPosition.z)), time);

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
        fakeHandAnimator.Play("Handshake", -1, 0);
        //fakeHandAnimator.speed = 0;
    }
}

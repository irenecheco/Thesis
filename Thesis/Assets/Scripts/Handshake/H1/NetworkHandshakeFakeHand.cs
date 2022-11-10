using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NetworkHandshakeFakeHand : MonoBehaviour
{
    private float ending_y;
    //private float y_angle;
    private float time = (float)0.75;
    private Vector3 midPosition;
    private Vector3 startingPosition;
    private Quaternion direction;
    private Quaternion startingRotation;

    [SerializeField] private GameObject rightController;
    private GameObject fakeHand_holder;

    private Animator fakeHandAnimator;

    public float x = 0.05f;
    public float z = 0.05f;

    private void Awake()
    {
        fakeHand_holder = this.transform.parent.gameObject;
        fakeHandAnimator = this.GetComponent<Animator>();
    }

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

        fakeHand_holder.transform.DORotateQuaternion(direction, time);
        fakeHand_holder.transform.DOMove(new Vector3((midPosition.x + x), (float)(ending_y - 0.4), (midPosition.z - z)), time);

        Invoke("SecondPartHandshake", time);
    }

    public void SecondPartHandshake()
    {
        fakeHandAnimator.Play("Handshake", -1, 0);
    }
}

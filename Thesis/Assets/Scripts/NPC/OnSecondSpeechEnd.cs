using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OnSecondSpeechEnd : MonoBehaviour
{
    [SerializeField] private GameObject finalPosition;
    [SerializeField] private GameObject secondPosition;
    private Quaternion direction1;
    private Quaternion direction2;

    public void OnSpeechEnd()
    {
        direction1 = Quaternion.LookRotation((this.transform.position - secondPosition.transform.position), Vector3.up);
        direction1 = Quaternion.Euler(0, -90, 0) * direction1;
        direction1.x = 0;
        direction1.z = 0;

        direction2 = Quaternion.LookRotation((secondPosition.transform.position - finalPosition.transform.position), Vector3.up);
        direction2 = Quaternion.Euler(0, -90, 0) * direction2;
        direction2.x = 0;
        direction2.z = 0;
        Sequence shakeSequence = DOTween.Sequence();
        shakeSequence
            .Append(this.transform.parent.DORotateQuaternion(direction1, 0.75f))
            .Append(this.transform.parent.DOMove(secondPosition.transform.position, 4f))
            .Append(this.transform.parent.DORotateQuaternion(direction2, 0.75f))
            .Append(this.transform.parent.DOMove(finalPosition.transform.position, 100f))
            .Play();
        //this.transform.parent.DOMove(finalPosition.transform.position, 150f);
    }
}

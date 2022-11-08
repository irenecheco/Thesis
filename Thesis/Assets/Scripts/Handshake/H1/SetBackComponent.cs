using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackComponent : MonoBehaviour
{
    private GameObject rightHand;
    private GameObject rightController;

    // Start is called before the first frame update
    void Start()
    {
        rightHand = this.gameObject;
        rightController = rightHand.transform.parent.gameObject;
    }

    //Called at the hand of the animation: it sets back the parent and the components
    public void SetBackComp()
    {
        rightHand.transform.parent = rightController.transform;
        rightController.AddComponent<HandController>();
        rightController.GetComponent<HandController>().hand = rightHand.GetComponent<Hand>();
        rightHand.GetComponent<Hand>().flag = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Oculus.Interaction.Input;

public class ActivateAnimationOverNetworkH4 : MonoBehaviour
{
    private PhotonView photonView;

    private GameObject controllersContainer;
    private GameObject handContainer;
    private GameObject hand;
   
    void Start()
    {
        handContainer = this.transform.GetChild(0).gameObject;
        hand = handContainer.transform.GetChild(0).gameObject;
        controllersContainer = this.transform.parent.gameObject;

        photonView = controllersContainer.transform.parent.gameObject.GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            if(controllersContainer.name == "Controllers")
            {
                if (this.name == "Left Hand")
                {
                    hand.GetComponent<AnimatedHandOVR>().InjectController(OVRInput.Controller.LTouch);
                }
                else if (this.name == "Right Hand")
                {
                    hand.GetComponent<AnimatedHandOVR>().InjectController(OVRInput.Controller.RTouch);
                }
            } else if(controllersContainer.name == "Hands")
            {
                if (this.name == "Left Hand")
                {
                    hand.GetComponent<AnimatedHandOVR>().InjectController(OVRInput.Controller.LHand);
                }
                else if (this.name == "Right Hand")
                {
                    hand.GetComponent<AnimatedHandOVR>().InjectController(OVRInput.Controller.RHand);
                }
            }
            
        }
    }
}

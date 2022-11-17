using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class OnCollisionActivateCanvasH2 : MonoBehaviour
{
    //Code responsible for activating the handshake message when two players collide

    public GameObject otherPlayerHead;
    private GameObject rightHandLocal;
  
    private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_waitingCanva = "Handshake 2 waiting";
    private string handshake2_confirmCanva = "Handshake 2 confirm";


    //Method of the Photon Pun library that lets you keep track of a variable through the network (class IPunObservable)
    //In this case it keeps track of a bool that is true when the message canvas is active

    void Start()
    {
        rightHandLocal = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
    }

    //Function called on trigger entered: it activates the message canvas on the other player head only if the two heads collide
    //and button A is not pressed
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Head")
        {
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;

                //When two players' heads are colliding it saves the other player                    
                this.gameObject.transform.GetComponent<OnButtonAPressed>().otherPlayerHead = otherPlayerHead;
                this.gameObject.transform.GetComponent<OnButtonAPressed>().isColliding = true;
                GameObject messageCanva = otherPlayerHead.transform.Find(handshake2_messageCanva).gameObject;
                GameObject confirmCanva = otherPlayerHead.transform.Find(handshake2_confirmCanva).gameObject;
                if (rightHandLocal != null)
                {
                    GameObject waitingCanva = rightHandLocal.transform.FindChildRecursive(handshake2_waitingCanva).gameObject;
                    if (!waitingCanva.GetComponent<Canvas>().enabled && !confirmCanva.GetComponent<Canvas>().enabled)
                    {
                        messageCanva.GetComponent<Canvas>().enabled = true;
                        if (rightHandLocal != null)
                        {
                            rightHandLocal.GetComponent<HapticController>().amplitude = 0.2f;
                            rightHandLocal.GetComponent<HapticController>().duration = 0.2f;
                            rightHandLocal.GetComponent<HapticController>().SendHaptics();
                        }
                        messageCanva.GetComponent<AudioSource>().Play();
                    }
                }            
            }
        }
    }
}

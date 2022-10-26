using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitressActivateCanvas : MonoBehaviour
{
    //Code that activates waitress canvas on collision

    private GameObject leftHand;
    private GameObject handshake_canvas;
    private GameObject handshake_button;

    void Start()
    {
        leftHand = GameObject.Find("Camera Offset/LeftHand Controller/LeftHand");
        handshake_canvas = leftHand.transform.GetChild(2).gameObject;
        handshake_button = handshake_canvas.transform.GetChild(1).gameObject;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Main Camera")
        {
            handshake_button.GetComponent<Button>().interactable = true;
            handshake_button.GetComponent<HandshakeButton>().isCollidingWithWaitress = true;
            
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Main Camera")
        {
            handshake_button.GetComponent<Button>().interactable = false;
            handshake_button.GetComponent<HandshakeButton>().isCollidingWithWaitress = false;
        }
    }
}

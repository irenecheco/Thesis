using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbingNPC : MonoBehaviour
{
    //Code that detect if user grabs NPC hand

    private int frameNumber;
    private bool firstFrame;
    private bool isGrabbing;

    private GameObject local_player_right;

    void Start()
    {
        frameNumber = 0;
        firstFrame = true;
        isGrabbing = false;

        local_player_right = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
    }

    private void Update()
    {
        if(isGrabbing == true)
        {
            frameNumber++;
            if (frameNumber >= 30)
            {
                frameNumber = 0;
                this.GetComponent<Outline>().enabled = false;
                local_player_right.GetComponent<Outline>().enabled = false;
            }
        }
    }
    public void isGrabbed()
    {
        //Haptic, visual and sound feedback are provided during the handshake
        local_player_right.GetComponent<HapticController>().SendHaptics2H3();
        if (firstFrame == true)
        {
            this.GetComponent<Outline>().enabled = true;
            local_player_right.GetComponent<Outline>().enabled = true;
            local_player_right.GetComponent<AudioSource>().Play();
            firstFrame = false;
            isGrabbing = true;
        }
    }

    public void isReleased()
    {
        firstFrame = true;
        this.transform.GetComponent<XRGrabInteractable>().enabled = false;
        isGrabbing = false;
        this.GetComponent<Outline>().enabled = false;
        local_player_right.GetComponent<Outline>().enabled = false;
    }
}

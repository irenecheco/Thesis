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
    private GameObject npc_hand_holder;
    private GameObject npc;
    private GameObject npc_head;
    private GameObject npc_head_canvas;

    void Start()
    {
        frameNumber = 0;
        firstFrame = true;
        isGrabbing = false;

        local_player_right = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        npc_hand_holder = this.transform.parent.gameObject;
        npc = npc_hand_holder.transform.parent.gameObject;
        npc_head = npc.transform.GetChild(0).gameObject;
        if(npc.gameObject.name == "Mayor")
        {
            npc_head_canvas = npc_head.transform.GetChild(0).gameObject;
        }        
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
            if (npc.gameObject.name == "Waitress")
            {
                npc_head_canvas = npc_head.transform.GetChild(0).gameObject;
                npc_head_canvas.GetComponent<Canvas>().enabled = false;
            }
        }
    }

    public void isReleased()
    {
        firstFrame = true;
        this.transform.GetComponent<XRGrabInteractable>().enabled = false;
        isGrabbing = false;
        this.GetComponent<Outline>().enabled = false;
        local_player_right.GetComponent<Outline>().enabled = false;
        if(npc.gameObject.name == "Mayor")
        {
            this.GetComponent<MayorConfirmCanvas>().secondSpeech();
            npc_head_canvas.GetComponent<Canvas>().enabled = false;
        } else if (npc.gameObject.name == "Waitress")
        {
            npc.GetComponent<HandshakeActivationNPC2>().secondSpeech();
            this.GetComponent<XRGrabInteractable>().enabled = false;
        }
    }
}

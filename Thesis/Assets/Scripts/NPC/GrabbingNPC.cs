using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbingNPC : MonoBehaviour
{
    //Code that detect if user grabs NPC hand

    private int frameNumber;
    private bool firstFrame;
    public bool isGrabbing;
    public bool releasedForCollision;

    private GameObject local_player_right;
    private GameObject npc_hand_holder;
    private GameObject npc;
    private GameObject npc_head;
    private GameObject npc_head_canvas;
    private GameObject rightController;

    public Vector3 initialPosition;

    private Color baseColor = new Color(0.8000001f, 0.4848836f, 0.3660862f, 1.0f);

    void Start()
    {
        frameNumber = 0;
        firstFrame = true;
        isGrabbing = false;
        releasedForCollision = false;

        local_player_right = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        rightController = local_player_right.transform.parent.gameObject;
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
            rightController.GetComponent<HandController>().isGrabbingH3 = true;
            if (npc.gameObject.name == "Waitress")
            {
                npc_head_canvas = npc_head.transform.GetChild(0).gameObject;
                npc_head_canvas.GetComponent<Canvas>().enabled = false;
            } else if (npc.gameObject.name == "Mayor")
            {
                this.transform.FindChildRecursive("hands:Lhand").gameObject.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
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
        rightController.GetComponent<HandController>().isGrabbingH3 = false;
        if (npc.gameObject.name == "Mayor")
        {
            this.GetComponent<MayorConfirmCanvas>().secondSpeech();
            npc_head_canvas.GetComponent<Canvas>().enabled = false;
            this.transform.localPosition = initialPosition;
        } else if (npc.gameObject.name == "Waitress")
        {
            if(releasedForCollision == false)
            {
                npc.GetComponent<HandshakeActivationNPC2>().secondSpeech();
            }            
            this.GetComponent<XRGrabInteractable>().enabled = false;
        }
    }
}

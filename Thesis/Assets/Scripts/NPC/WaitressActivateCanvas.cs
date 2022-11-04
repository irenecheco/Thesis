using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class WaitressActivateCanvas : MonoBehaviour
{
    //Code that activates waitress canvas on collision

    private int sceneIndex;

    private GameObject leftHand;
    private GameObject handshake_canvas;
    private GameObject handshake_button;

    private GameObject npcHead;
    private GameObject npcMessage;
    private GameObject npc;
    private GameObject npcLeft;
    private GameObject npcRight;
    private GameObject npcHandHolder;
    private GameObject coffeeTray;

    private Animator animator_NPC_head;
    private Animator animator_NPC_left;
    private Animator animator_NPC_right;

    private GameObject environment;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        environment = GameObject.Find("Environment");

        if (sceneIndex == 1)
        {
            leftHand = GameObject.Find("Camera Offset/LeftHand Controller/LeftHand");
            handshake_canvas = leftHand.transform.GetChild(2).gameObject;
            handshake_button = handshake_canvas.transform.GetChild(1).gameObject;
            handshake_canvas.GetComponent<Canvas>().enabled = false;
        }        

        npc = this.gameObject;
        npcHead = npc.transform.GetChild(0).gameObject;
        npcLeft = npc.transform.GetChild(1).gameObject;
        npcHandHolder = npc.transform.GetChild(2).gameObject;
        npcRight = npcHandHolder.transform.GetChild(0).gameObject;
        if(this.gameObject.name == "Waitress")
        {
            coffeeTray = npcLeft.transform.GetChild(2).gameObject;
        }

        animator_NPC_head = npcHead.GetComponent<Animator>();
        animator_NPC_left = npcLeft.GetComponent<Animator>();
        animator_NPC_right = npcRight.GetComponent<Animator>();

        if(sceneIndex == 2)
        {
            npcMessage = npcHead.transform.GetChild(0).gameObject;
        }

        if (sceneIndex == 3)
        {
            npcRight.GetComponent<XRGrabInteractable>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Main Camera")
        {
            if(sceneIndex == 1)
            {
                handshake_canvas.GetComponent<Canvas>().enabled = true;
                handshake_button.GetComponent<Button>().interactable = true;
                handshake_button.GetComponent<HandshakeButton>().isCollidingWithWaitress = true;

                leftHand.GetComponent<HapticController>().amplitude = 0.2f;
                leftHand.GetComponent<HapticController>().duration = 0.2f;
                leftHand.GetComponent<HapticController>().SendHaptics();
                handshake_canvas.GetComponent<AudioSource>().Play();

            } else if(sceneIndex == 2)
            {
                npcMessage.GetComponent<Canvas>().enabled = true;
                npc.GetComponent<HandshakeActivationNPC2>().isCollidingWithWaitress = true;
            } else if (sceneIndex == 3)
            {
                npcRight.GetComponent<XRGrabInteractable>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Main Camera")
        {
            if(sceneIndex == 1)
            {
                handshake_button.GetComponent<Button>().interactable = false;
                handshake_button.GetComponent<HandshakeButton>().isCollidingWithWaitress = false;
                handshake_canvas.GetComponent<Canvas>().enabled = false;
                handshake_button.GetComponent<HandshakeButton>().firstHandshake = true;
            } else if(sceneIndex == 2)
            {
                npcMessage.GetComponent<Canvas>().enabled = false;
                npc.GetComponent<HandshakeActivationNPC2>().isCollidingWithWaitress = false;
                npc.GetComponent<HandshakeActivationNPC2>().firstHandshake = true;
            } else if (sceneIndex == 3)
            {
                npcRight.GetComponent<XRGrabInteractable>().enabled = false;
            }

            animator_NPC_head.Play("WaitressIdle_head");
            animator_NPC_right.Play("WaitressIdle_right");
            animator_NPC_left.Play("WaitressIdle_left");

            if (npcHead.GetComponent<AudioSource>().isPlaying)
            {
                npcHead.GetComponent<AudioSource>().Stop();
            }

            if(coffeeTray != null)
            {
                GameObject coffeCupTemp = GameObject.Find("Cafe Cup(Clone)");
                if(coffeCupTemp != null)
                {
                    coffeCupTemp.GetComponent<XRGrabInteractable>().enabled = false;
                    Destroy(coffeCupTemp);
                    environment.GetComponent<FruitSpawner>().SpawnCoffee();
                    coffeeTray.transform.GetChild(0).gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                    leftHand.GetComponent<SetCoffeeGrab>().childexists = false;
                }
            }            
        }
    }
}

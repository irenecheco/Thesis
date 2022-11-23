using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class WaitressActivateCanvas : MonoBehaviour
{
    //Code that activates waitress canvas on collision

    private int sceneIndex;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    private GameObject handshake_canvas_l;
    private GameObject handshake_canvas_r;
    private GameObject handshake_button_l;
    private GameObject handshake_button_r;

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

    [SerializeField] private GameObject environment;
    private GameObject rightHandController;

    private XROrigin origin;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 1)
        {
            handshake_canvas_l = leftHand.transform.GetChild(2).gameObject;
            handshake_canvas_r = rightHand.transform.GetChild(2).gameObject;
            handshake_button_l = handshake_canvas_l.transform.GetChild(1).gameObject;
            handshake_button_r = handshake_canvas_r.transform.GetChild(1).gameObject;
            handshake_canvas_l.GetComponent<Canvas>().enabled = false;
            handshake_canvas_r.GetComponent<Canvas>().enabled = false;
            origin = FindObjectOfType<XROrigin>();
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
            npcMessage = npcHead.transform.GetChild(0).gameObject;
            rightHandController = rightHand.transform.parent.gameObject;
        }

        if(sceneIndex == 4)
        {
            npcRight.GetComponent<XRGrabInteractable>().enabled = false;
            npcMessage = npcHead.transform.GetChild(0).gameObject;
            rightHandController = rightHand.transform.parent.gameObject;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Head Local")
        {
            if(sceneIndex == 1)
            {
                origin.GetComponent<ActiveHandController>().isCollidingWithWaitress = true;
                if (leftHand.transform.parent.gameObject.GetComponent<XRRayInteractor>().enabled == true)
                {
                    handshake_canvas_l.GetComponent<Canvas>().enabled = true;                    
                    leftHand.GetComponent<HapticController>().amplitude = 0.2f;
                    leftHand.GetComponent<HapticController>().duration = 0.2f;
                    leftHand.GetComponent<HapticController>().SendHaptics();
                    handshake_canvas_l.GetComponent<AudioSource>().Play();
                }
                else
                {
                    handshake_canvas_r.GetComponent<Canvas>().enabled = true;                    
                    rightHand.GetComponent<HapticController>().amplitude = 0.2f;
                    rightHand.GetComponent<HapticController>().duration = 0.2f;
                    rightHand.GetComponent<HapticController>().SendHaptics();
                    handshake_canvas_r.GetComponent<AudioSource>().Play();
                }
                handshake_button_l.GetComponent<HandshakeButton>().isCollidingWithWaitress = true;
                handshake_button_r.GetComponent<HandshakeButton>().isCollidingWithWaitress = true;
            } else if(sceneIndex == 2)
            {
                npcMessage.GetComponent<Canvas>().enabled = true;
                npcMessage.GetComponent<AudioSource>().enabled = true;
                npcMessage.GetComponent<AudioSource>().Play();
                npc.GetComponent<HandshakeActivationNPC2>().isCollidingWithWaitress = true;
                //Debug.Log($"{npc.GetComponent<HandshakeActivationNPC2>().isCollidingWithWaitress}");
            } else if (sceneIndex == 3)
            {
                npcRight.GetComponent<XRGrabInteractable>().enabled = true;
                npcMessage.GetComponent<Canvas>().enabled = true;
                npcMessage.GetComponent<AudioSource>().enabled = true;
                npcMessage.GetComponent<AudioSource>().Play();
                rightHandController.GetComponent<XRDirectInteractor>().allowSelect = true;
                npcRight.GetComponent<GrabbingNPC>().releasedForCollision = false;
            } else if(sceneIndex == 4)
            {
                npcRight.GetComponent<XRGrabInteractable>().enabled = true;
                npcMessage.GetComponent<Canvas>().enabled = true;
                npcMessage.GetComponent<AudioSource>().enabled = true;
                npcMessage.GetComponent<AudioSource>().Play();
                rightHandController.GetComponent<XRDirectInteractor>().allowSelect = true;
                npcRight.GetComponent<GrabbingNPC>().releasedForCollision = false;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Head Local")
        {
            if(sceneIndex == 1)
            {
                handshake_button_l.GetComponent<HandshakeButton>().isCollidingWithWaitress = false;
                handshake_canvas_l.GetComponent<Canvas>().enabled = false;
                handshake_button_l.GetComponent<HandshakeButton>().firstHandshake = true;
                handshake_button_r.GetComponent<HandshakeButton>().isCollidingWithWaitress = false;
                handshake_canvas_r.GetComponent<Canvas>().enabled = false;
                handshake_button_r.GetComponent<HandshakeButton>().firstHandshake = true;
            } else if(sceneIndex == 2)
            {
                npcMessage.GetComponent<Canvas>().enabled = false;
                npc.GetComponent<HandshakeActivationNPC2>().isCollidingWithWaitress = false;
                npc.GetComponent<HandshakeActivationNPC2>().firstHandshake = true;
            } else if (sceneIndex == 3)
            {
                if(npcRight.GetComponent<GrabbingNPC>().isGrabbing == true)
                {
                    rightHandController.GetComponent<XRDirectInteractor>().allowSelect = false;
                    npcRight.GetComponent<GrabbingNPC>().releasedForCollision = true;
                }                
                npcRight.GetComponent<XRGrabInteractable>().enabled = false;
                npcMessage.GetComponent<Canvas>().enabled = false;
                
            } else if(sceneIndex == 4)
            {
                if (npcRight.GetComponent<GrabbingNPC>().isGrabbing == true)
                {
                    rightHandController.GetComponent<XRDirectInteractor>().allowSelect = false;
                    npcRight.GetComponent<GrabbingNPC>().releasedForCollision = true;
                }
                npcRight.GetComponent<XRGrabInteractable>().enabled = false;
                npcMessage.GetComponent<Canvas>().enabled = false;
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

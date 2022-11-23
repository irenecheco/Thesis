using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class OnCollisionSetBool : MonoBehaviour
{
    //Code to detect collision and set bool in HandshakeActivationNPC accordingly
    private GameObject local_player_head;
    private GameObject npc;
    private GameObject npc_head;
    private GameObject npc_right;
    [SerializeField] private GameObject rightHandController;

    private int sceneIndex;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        local_player_head = Camera.main.gameObject;

        npc = this.gameObject;
        npc_head = npc.transform.GetChild(0).gameObject;
        npc_right = npc.transform.FindChildRecursive("NPC_RightHand").gameObject;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject == local_player_head)
        {
            npc_head.GetComponent<MayorConfirmCanvas>().isColliding = true;
            if(sceneIndex == 3)
            {
                rightHandController.GetComponent<XRDirectInteractor>().allowSelect = true;
            } else if(sceneIndex == 3)
            {
                rightHandController.GetComponent<XRDirectInteractor>().allowSelect = true;
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject == local_player_head)
        {
            if(npc_head.GetComponent<MayorConfirmCanvas>().isColliding == false)
            {
                npc_head.GetComponent<MayorConfirmCanvas>().isColliding = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject == local_player_head)
        {
            npc_head.GetComponent<MayorConfirmCanvas>().isColliding = false;
            if (sceneIndex == 3)
            {
                if (npc_right.GetComponent<GrabbingNPC>().isGrabbing == true) {
                    rightHandController.GetComponent<XRDirectInteractor>().allowSelect = false;
                }                
            } else if(sceneIndex == 4)
            {
                if (npc_right.GetComponent<GrabbingNPC>().isGrabbing == true)
                {
                    rightHandController.GetComponent<XRDirectInteractor>().allowSelect = false;
                }
            }
        }
    }
}

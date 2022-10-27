using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionSetBool : MonoBehaviour
{
    //Code to detect collision and set bool in HandshakeActivationNPC accordingly
    private GameObject local_player_head;
    private GameObject npc;
    private GameObject npc_head;

    void Start()
    {
        local_player_head = GameObject.Find("Camera Offset/Main Camera");

        npc = this.gameObject;
        npc_head = npc.transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject == local_player_head)
        {
            npc_head.GetComponent<MayorConfirmCanvas>().isColliding = true;
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
        }
    }
}

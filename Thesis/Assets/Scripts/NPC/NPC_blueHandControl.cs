using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC_blueHandControl : MonoBehaviour
{
    [SerializeField] private Animator npc_animator_head;
    [SerializeField] private GameObject npc_head;
    [SerializeField] private GameObject npc_rightMesh;

    private Color baseColor = new Color(0.8000001f, 0.4848836f, 0.3660862f, 1.0f);
    private Color waitingColor = new Color(0.4135279f, 0.7409829f, 0.9056604f, 1.0f);

    private bool firstFrame = false;

    public System.DateTime initialTimeWaitress;

    private int sceneIndex;

    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (npc_animator_head.GetCurrentAnimatorStateInfo(0).IsName("Waitress_waiting_head"))
        {
            if (!firstFrame)
            {
                npc_rightMesh.GetComponent<SkinnedMeshRenderer>().material.color = waitingColor;
                npc_head.transform.GetChild(0).transform.GetComponent<Canvas>().enabled = true;
                initialTimeWaitress = System.DateTime.UtcNow;
                if (sceneIndex == 1)
                {
                    InteractionsCount.startedInteractionsFromWaitressH1++;
                    npc_head.transform.FindChildRecursive("HandshakeConfirm Button").GetComponent<HandshakeActivationNPC>().initialTimeH1Waitress = initialTimeWaitress;
                } else if(sceneIndex == 2)
                {
                    if(npc_head.transform.parent.name == "Waitress")
                    {
                        InteractionsCount.startedInteractionsFromWaitressH2++;
                        npc_head.transform.parent.GetComponent<HandshakeActivationNPC2>().initialTimeH2Waitress = initialTimeWaitress;
                    }                    
                } else if(sceneIndex == 3)
                {
                    if (npc_head.transform.parent.name == "Waitress")
                    {
                        InteractionsCount.startedInteractionsFromWaitressH3++;
                        this.GetComponent<GrabbingNPC>().initialTimeH3Waitress = initialTimeWaitress;
                    }
                }
                else if (sceneIndex == 4)
                {
                    InteractionsCount.startedInteractionsFromWaitressH4++;
                    if (npc_head.transform.parent.name == "Waitress")
                    {
                        this.GetComponent<GrabbingNPC>().initialTimeH4Waitress = initialTimeWaitress;
                    }
                }

                firstFrame = true;
            }                         
        } else
        {
            if (firstFrame)
            {
                npc_rightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
                npc_head.transform.GetChild(0).transform.GetComponent<Canvas>().enabled = false;
                firstFrame = false;
            }            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;
using NLog.Unity;

public class HandshakeActivationNPC : MonoBehaviour
{
    //Code responsible for NPC handshake activation in H1 and H2

    private int sceneIndex;

    private GameObject confirmCanvas;
    private GameObject confirmHead;
    private GameObject confirmNPC;
    private GameObject NPC_rightHand;
    private GameObject NPC_rightMesh;
    private GameObject confirmNPCHandHolder;

    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject fakeHand_holder;
    [SerializeField] private GameObject fakeHand;
    [SerializeField] private GameObject fakeHandNPC_holder;
    [SerializeField] private GameObject fakeHandNPC;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject NPCHand_holder;

    private System.DateTime finalTimeH1Mayor;
    public System.DateTime initialTimeH1Mayor;
    private System.DateTime finalTimeH2Mayor;
    public System.DateTime initialTimeH2Mayor;

    private System.DateTime finalTimeH1Waitress;
    public System.DateTime initialTimeH1Waitress;

    private System.DateTime firstDate;

    private Color baseColor = new Color(0.8000001f, 0.4848836f, 0.3660862f, 1.0f);

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex == 1)
        {
            if(this.gameObject.name != "Waitress")
            {
                confirmCanvas = this.gameObject.transform.parent.gameObject;
                confirmHead = confirmCanvas.transform.parent.gameObject;
                confirmNPC = confirmHead.transform.parent.gameObject;
                
            } else
            {
                confirmNPC = this.gameObject;
                confirmHead = confirmNPC.transform.GetChild(0).gameObject;
            }
            confirmNPCHandHolder = confirmNPC.transform.GetChild(2).gameObject;
            NPC_rightHand = confirmNPCHandHolder.transform.GetChild(0).gameObject;
            NPC_rightMesh = NPC_rightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
            fakeHand = fakeHand_holder.transform.GetChild(0).gameObject;            

        } else if(sceneIndex == 2)
        {
            if (this.gameObject.name != "Waitress")
            {
                confirmCanvas = this.gameObject;
                confirmHead = confirmCanvas.transform.parent.gameObject;
                confirmNPC = confirmHead.transform.parent.gameObject;
            }
            else
            {
                confirmNPC = this.gameObject;
                confirmHead = confirmNPC.transform.GetChild(0).gameObject;
            }
            confirmNPCHandHolder = confirmNPC.transform.GetChild(2).gameObject;
            NPC_rightHand = confirmNPCHandHolder.transform.GetChild(0).gameObject;
            NPC_rightMesh = NPC_rightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
            fakeHand = fakeHand_holder.transform.GetChild(0).gameObject;
        }        
    }

    //Function called when confirm button pressed: it saves the ids and call the methed to activate the animation over the network
    public void StartHandshake()
    {
        if(sceneIndex == 1)
        {
            if (confirmNPC.gameObject.name == "Mayor")
            {
                InteractionsCount.finishedInteractionsWithMayorH1++;
                finalTimeH1Mayor = System.DateTime.UtcNow;
                NLogConfig.LogLine($"{"Mayor"};TimeFromCanvasAppearing;{(finalTimeH1Mayor - initialTimeH1Mayor).TotalSeconds.ToString("#.000")}; s");
            } else if (confirmNPC.gameObject.name == "Waitress")
            {
                InteractionsCount.finishedInteractionsWithWaitressH1++;
                finalTimeH1Waitress = System.DateTime.UtcNow;
                NLogConfig.LogLine($"{"Waitress"};TimeFromCanvasAppearing;{(finalTimeH1Waitress - initialTimeH1Waitress).TotalSeconds.ToString("#.000")}; s");
            }
        } else if(sceneIndex == 2)
        {
            if (confirmNPC.gameObject.name == "Mayor")
            {
                //devi ancora settare initial time
                //finalTimeH2Mayor = System.DateTime.Now;
                //NLogConfig.LogLine($"{"Mayor"};TimeFromCanvasAppearing:{(finalTimeH2Mayor - initialTimeH2Mayor).TotalMilliseconds.ToString("#.00")} ms");
            }
        }
        
        Invoke("SetParam", .25f);
    }

    //Coroutine that trigger the animation on the player
    public void SetParam()
    {
        rightHand.SetActive(false);
        fakeHand.SetActive(true);

        NPC_rightHand.SetActive(false);
        fakeHandNPC.SetActive(true);

        fakeHandNPC.GetComponent<SetBackComponent>().rightHand = NPC_rightHand;

        fakeHand.GetComponent<HandshakeFakeHand>().DoHandshake(camera.transform.position, confirmHead.transform.position);
        fakeHandNPC.GetComponent<HandshakeFakeHandNPC>().DoHandshake(confirmHead.transform.position, camera.transform.position, NPCHand_holder);

        if(confirmNPC.gameObject.name == "Mayor")
        {
            confirmCanvas.GetComponent<Canvas>().enabled = false;
            NPC_rightMesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
        }        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class GrabbingNPC : MonoBehaviour
{
    //Code that detect if user grabs NPC hand

    private int frameNumber;
    private bool firstFrame;
    public bool isGrabbing;
    public bool releasedForCollision;

    [SerializeField] private GameObject fakeHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject fakeHandNPC;
    [SerializeField] private GameObject NPC_rightHand;

    private GameObject local_player_right;
    private GameObject npc_hand_holder;
    private GameObject npc;
    private GameObject npc_head;
    private GameObject npc_head_canvas;
    private GameObject rightController;
    private GameObject npc_right_mesh;

    public Vector3 initialPosition;

    private int sceneIndex;

    private Color baseColor = new Color(0.8000001f, 0.4848836f, 0.3660862f, 1.0f);

    void Start()
    {
        frameNumber = 0;
        firstFrame = true;
        isGrabbing = false;
        releasedForCollision = false;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        local_player_right = GameObject.Find("Camera Offset/RightHand Controller/RightHand");
        rightController = local_player_right.transform.parent.gameObject;
    npc_hand_holder = this.transform.parent.gameObject;
        npc = npc_hand_holder.transform.parent.gameObject;
        npc_head = npc.transform.GetChild(0).gameObject;
        if(npc.gameObject.name == "Mayor")
        {
            npc_head_canvas = npc_head.transform.GetChild(0).gameObject;
            npc_right_mesh = NPC_rightHand.transform.FindChildRecursive("hands:Lhand").gameObject;
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
            if(sceneIndex == 4)
            {
                StartCoroutine(Wait());
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
            if (sceneIndex != 4)
            {
                this.GetComponent<MayorConfirmCanvas>().secondSpeech();
                npc_head_canvas.GetComponent<Canvas>().enabled = false;
                this.transform.localPosition = initialPosition;
            }
            else
            {
                npc_head_canvas.GetComponent<Canvas>().enabled = false;
                this.transform.localPosition = initialPosition;
            }                
        } else if (npc.gameObject.name == "Waitress")
        {
            if(releasedForCollision == false)
            {
                npc.GetComponent<HandshakeActivationNPC2>().secondSpeech();
            }            
            this.GetComponent<XRGrabInteractable>().enabled = false;
        }
    }

    public IEnumerator Wait()
    {
        float time = (float)0.25;
        //GameObject head = otherPlayer.transform.GetChild(0).gameObject;
        yield return new WaitForSeconds(time);
        //rightController.GetComponent<XRDirectInteractor>().allowSelect = false;
        //rightController.GetComponent<ActionBasedController>().enableInputTracking = true;

        rightHand.SetActive(false);
        fakeHand.SetActive(true);

        NPC_rightHand.SetActive(false);
        fakeHandNPC.SetActive(true);

        fakeHandNPC.GetComponent<SetBackComponent>().rightHand = NPC_rightHand;

        fakeHand.GetComponent<HandshakeFakeHand>().DoHandshakeH4(Camera.main.transform.position, npc_head.transform.position);
        fakeHandNPC.GetComponent<HandshakeFakeHandNPC>().DoHandshakeH4(npc_head.transform.position, Camera.main.transform.position, npc_hand_holder);

        if (npc.gameObject.name == "Mayor")
        {
            npc_head_canvas.GetComponent<Canvas>().enabled = false;
            npc_right_mesh.GetComponent<SkinnedMeshRenderer>().material.color = baseColor;
        }
    }
}

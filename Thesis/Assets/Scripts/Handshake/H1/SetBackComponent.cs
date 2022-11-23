using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SetBackComponent : MonoBehaviour
{
    private GameObject fakeHand;
    [SerializeField] private GameObject NPC_handHolder;

    [SerializeField] private GameObject fakeHand_holder;
    [SerializeField] private GameObject netRightController;
    public GameObject rightHand;
    private GameObject NPCHead;
    private GameObject NPCLeft;
    private GameObject netPlayer;
    private GameObject netHead;
    private GameObject localRightController;

    private Animator animator_NPC_head;
    private Animator animator_NPC_left;
    private Animator animator_NPC_right;

    private int sceneIndex;

    // Start is called before the first frame update
    void Awake()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        fakeHand = this.gameObject;
        if (sceneIndex == 2)
        {
            if (fakeHand_holder != null)
            {
                if (fakeHand_holder.transform.parent != null)
                {
                    netPlayer = fakeHand_holder.transform.parent.gameObject;
                    netHead = netPlayer.transform.GetChild(0).gameObject;
                }
            }
        }
        localRightController = GameObject.Find("Camera Offset/RightHand Controller");
    }

    //Called at the hand of the animation: it sets back the parent and the components
    public void SetBackComp()
    {
        if (sceneIndex != 4)
        {
            StartCoroutine(Wait());
        }
        else
        {
            StartCoroutine(Wait4());
        }
    }

    public IEnumerator Wait()
    {
        float time = (float)0.75;
        yield return new WaitForSeconds(time);

        fakeHand_holder.transform.DOMove(rightHand.transform.position, time);
        fakeHand_holder.transform.DORotateQuaternion(rightHand.transform.rotation * Quaternion.Euler(0, 0, 90), time);

        yield return new WaitForSeconds(time);

        rightHand.SetActive(true);
        fakeHand.SetActive(false);

        if (sceneIndex == 2)
        {
            if (netHead != null && netHead.gameObject.name == "Head")
            {
                netHead.gameObject.transform.GetComponent<OnButtonAPressed>().animationGoing = false;
                netHead.gameObject.transform.GetComponent<OnButtonAPressed>().isPressed = false;
            }
        }

        if(NPC_handHolder!= null)
        {
            NPC_handHolder = rightHand.transform.parent.gameObject;
            NPCHead = NPC_handHolder.transform.parent.gameObject.transform.GetChild(0).gameObject;
            NPCLeft = NPC_handHolder.transform.parent.gameObject.transform.GetChild(1).gameObject;

            if (NPC_handHolder.transform.parent.gameObject.name == "Mayor")
            {
                NPCHead.GetComponent<MayorConfirmCanvas>().secondSpeech();
            }
            else if (NPC_handHolder.transform.parent.gameObject.name == "Waitress")
            {
                secondSpeech();
            }
        }        
    }

    //Called at the hand of the set back component: it starts the speech
    public void secondSpeech()
    {
        NPCHead.GetComponent<AudioSource>().Play();

        animator_NPC_head = NPCHead.GetComponent<Animator>();
        animator_NPC_left = NPCLeft.GetComponent<Animator>();
        animator_NPC_right = rightHand.GetComponent<Animator>();

        animator_NPC_head.Play("WaitressSpeech_head");
        animator_NPC_right.Play("WaitressSpeech_right");
        animator_NPC_left.Play("WaitressSpeech_left");
    }

    public IEnumerator Wait4()
    {
        float time = (float)0.25;
        yield return new WaitForSeconds(time);

        fakeHand_holder.transform.DOMove(rightHand.transform.position, time);
        fakeHand_holder.transform.DORotateQuaternion(rightHand.transform.rotation * Quaternion.Euler(0, 0, 90), time);

        yield return new WaitForSeconds(time);

        rightHand.SetActive(true);
        fakeHand.SetActive(false);

        if(netRightController!= null)
        {
            rightHand.transform.SetParent(netRightController.transform);
            netRightController.GetComponent<NetworkHandController>().isGrabbingH3 = false;
            //rightHand.GetComponent<XRGrabInteractable>().enabled = true;
            //rightHand.GetComponent<XRGrabInteractable>().attachTransform = rightHand.transform.GetChild(2).transform;
        }

        localRightController.GetComponent<HandController>().isGrabbingH3 = false;

        if (NPC_handHolder != null)
        {
            if (NPC_handHolder.transform.parent.gameObject.name == "Mayor")
            {
                
                rightHand.transform.SetParent(NPC_handHolder.transform);
                rightHand.transform.localPosition = new Vector3(0, 0, 0);
                rightHand.transform.localRotation = new Quaternion(0, 0, 0, 0);
                rightHand.transform.localRotation = Quaternion.Euler(0, 0, -90);
                NPCHead = NPC_handHolder.transform.parent.gameObject.transform.GetChild(0).gameObject;
                NPCLeft = NPC_handHolder.transform.parent.gameObject.transform.GetChild(1).gameObject;
                NPCHead.GetComponent<MayorConfirmCanvas>().secondSpeech();
            }
            else
            {
                rightHand.transform.localPosition = new Vector3((float)-0.02, 0, (float)-0.09);
                rightHand.transform.localRotation = new Quaternion(0, 0, 0, 0);
                rightHand.transform.localRotation = Quaternion.Euler(0, 0, -90);
            }
        }
        else
        {
            rightHand.transform.localPosition = new Vector3((float)-0.02, 0, (float)-0.09);
            rightHand.transform.localRotation = new Quaternion(0, 0, 0, 0);
            rightHand.transform.localRotation = Quaternion.Euler(0, 0, -90);
        }        
    }
}

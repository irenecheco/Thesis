using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SetBackComponent : MonoBehaviour
{
    private GameObject fakeHand;
    private GameObject NPC_handHolder;

    [SerializeField] private GameObject fakeHand_holder;
    public GameObject rightHand;
    private GameObject NPCHead;
    private GameObject NPCLeft;

    private Animator animator_NPC_head;
    private Animator animator_NPC_left;
    private Animator animator_NPC_right;

    // Start is called before the first frame update
    void Start()
    {
        fakeHand = this.gameObject;
    }

    //Called at the hand of the animation: it sets back the parent and the components
    public void SetBackComp()
    {
        StartCoroutine(Wait());
    }

    public IEnumerator Wait()
    {
        float time = (float)0.75;
        yield return new WaitForSeconds(time);

        fakeHand_holder.transform.DOMove(rightHand.transform.position, time);
        fakeHand_holder.transform.DORotateQuaternion(rightHand.transform.rotation, time);

        yield return new WaitForSeconds(time);

        rightHand.SetActive(true);
        fakeHand.SetActive(false);

        NPC_handHolder = rightHand.transform.parent.gameObject;
        NPCHead = NPC_handHolder.transform.parent.gameObject.transform.GetChild(0).gameObject;
        NPCLeft = NPC_handHolder.transform.parent.gameObject.transform.GetChild(1).gameObject;

        if (NPC_handHolder.transform.parent.gameObject.name == "Mayor")
        {
            NPCHead.GetComponent<MayorConfirmCanvas>().secondSpeech();
        } 
        else if(NPC_handHolder.transform.parent.gameObject.name == "Waitress")
        {
            secondSpeech();
        }
    }

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
}

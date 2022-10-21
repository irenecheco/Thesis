using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MayorConfirmCanvas : MonoBehaviour
{
    public AudioClip mayor_speech1;
    public AudioClip mayor_speech2;

    private GameObject mayor_head;
    private GameObject mayor_head_canvas;
    private GameObject mayor_confirm_button;
    private GameObject mayor_left;
    private GameObject mayor_right;
    private GameObject mayor;
    private GameObject mayor_hand_holder;

    private Animator animator_mayor_head;
    private Animator animator_mayor_left;
    private Animator animator_mayor_right;

    private Vector3 initial_hand_holder_position;
    private Quaternion initial_hand_holder_rotation;

    void Start()
    {
        if(this.gameObject.name != "NPC_RightHand")
        {
            mayor_head = this.gameObject;
            mayor_head_canvas = mayor_head.transform.GetChild(0).gameObject;
            mayor_confirm_button = mayor_head_canvas.transform.GetChild(2).gameObject;

            mayor = mayor_head.transform.parent.gameObject;
            mayor_left = mayor.transform.GetChild(1).gameObject;
            mayor_hand_holder = mayor.transform.GetChild(2).gameObject;
            mayor_right = mayor_hand_holder.transform.GetChild(0).gameObject;
            animator_mayor_head = mayor_head.GetComponent<Animator>();
            animator_mayor_left = mayor_left.GetComponent<Animator>();
            animator_mayor_right = mayor_right.GetComponent<Animator>();
        } else
        {
            mayor_right = this.gameObject;
            mayor_hand_holder = mayor_right.transform.parent.gameObject;
            mayor = mayor_hand_holder.transform.parent.gameObject;
            mayor_left = mayor.transform.GetChild(1).gameObject;
            mayor_head = mayor.transform.GetChild(0).gameObject;

            animator_mayor_head = mayor_head.GetComponent<Animator>();
            animator_mayor_left = mayor_left.GetComponent<Animator>();
            animator_mayor_right = mayor_right.GetComponent<Animator>();
        }
        initial_hand_holder_position = mayor_hand_holder.transform.position;
        initial_hand_holder_rotation = mayor_hand_holder.transform.rotation;
    }

    public void activateMayorCanvas()
    {
        mayor_head_canvas.GetComponent<Canvas>().enabled = true;
        mayor_confirm_button.GetComponent<Button>().interactable = true;
        animator_mayor_head.speed = 0;
        animator_mayor_left.speed = 0;
        animator_mayor_right.speed = 0;
    }

    public void backToIdle()
    {
        mayor_hand_holder.transform.position = initial_hand_holder_position;
        mayor_hand_holder.transform.rotation = initial_hand_holder_rotation;
        animator_mayor_left.speed = 1;
        animator_mayor_head.speed = 1;
        mayor_head.GetComponent<AudioSource>().clip = mayor_speech2;
        mayor_head.GetComponent<AudioSource>().Play();
        animator_mayor_left.SetBool("Default", true);
        animator_mayor_head.SetBool("Default", true);
        animator_mayor_right.SetBool("Default", true);
    }
}

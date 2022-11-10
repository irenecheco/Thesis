using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticController : MonoBehaviour
{
    //Code responsible for haptics on the controller

    public XRBaseController controller;
    public float amplitude = 0.4f;
    public float duration = 0.5f;
    public float amplitude1H3 = 0.2f;
    public float duration1H3 = 0.3f;
    public float amplitude2H3 = 0.3f;
    public float duration2H3 = 1.5f;

    //Function called whenever an haptic is needed (handshake in H1 and H2)
    public void SendHaptics()
    {
        if(controller != null)
        {
            controller.SendHapticImpulse(amplitude, duration);                     
        }
    }

    public void SendHapticsinAnimation()
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(amplitude, duration);
            if (this.gameObject.name == "FakeRightHand")
            {
                if (this.gameObject.GetComponent<AudioSource>() != null)
                {
                    this.gameObject.GetComponent<AudioSource>().Play();
                }

                if (this.gameObject.GetComponent<Outline>() != null)
                {
                    this.gameObject.GetComponent<Outline>().enabled = true;
                    StartCoroutine(Wait());
                }
            }
        } else
        {
            if (this.gameObject.name == "FakeRightHand")
            {
                if (this.gameObject.GetComponent<Outline>() != null)
                {
                    this.gameObject.GetComponent<Outline>().enabled = true;
                    StartCoroutine(Wait());
                }
            }
        }
    }

    //In H3 there are two different haptics: first when grabbing other user's hand, second when the other user is grabbing back
    //and they can handshake
    public void SendHaptics1H3()
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(amplitude1H3, duration1H3);
        }
    }

    //Second one in stronger and longer
    public void SendHaptics2H3()
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(amplitude2H3, duration2H3);
        }
    }

    public IEnumerator Wait()
    {
        double time = 0.3;
        yield return new WaitForSeconds((float)time);
        this.gameObject.GetComponent<Outline>().enabled = false;
    }
}

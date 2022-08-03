using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticController : MonoBehaviour
{
    public XRBaseController rightController;
    public float amplitude = 0.4f;
    public float duration = 0.5f;
    public float amplitude1H3 = 0.2f;
    public float duration1H3 = 0.3f;
    public float amplitude2H3 = 0.3f;
    public float duration2H3 = 1.5f;

    public void SendHaptics()
    {
        if(rightController != null)
        {
            rightController.SendHapticImpulse(amplitude, duration);
        }
    }

    public void SendHaptics1H3()
    {
        if (rightController != null)
        {
            rightController.SendHapticImpulse(amplitude1H3, duration1H3);
        }
    }

    public void SendHaptics2H3()
    {
        if (rightController != null)
        {
            rightController.SendHapticImpulse(amplitude2H3, duration2H3);
        }
    }
}

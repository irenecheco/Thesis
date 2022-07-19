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

    public void SendHaptics()
    {
        if(rightController != null)
        {
            rightController.SendHapticImpulse(amplitude, duration);
        }
    }
}

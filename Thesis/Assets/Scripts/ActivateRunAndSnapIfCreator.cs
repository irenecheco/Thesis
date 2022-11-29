using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRatPolito.PrattiToolkit.VR;

public class ActivateRunAndSnapIfCreator : MonoBehaviour
{
    void Start()
    {
        if (MyUserControl.iAmThisUser == true)
        {
            this.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
        }
        else
        {
            this.GetComponent<ActionBasedSnapTurnProvider>().enabled = false;
        }
    }

}

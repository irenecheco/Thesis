using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BackToLobbyH2 : MonoBehaviour
{
    //Code responsible for handling the "Back to lobby" button (just for H3, without ray interactor)

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDeviceCharacteristics lControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
    private InputDevice targetDevice;

    private bool previouFramePressure;
    private bool confirmation;

    private GameObject headLocal;
    private GameObject backToLobbyConfirmCanva;

    void Start()
    {
        headLocal = this.gameObject.transform.GetChild(0).gameObject;
        backToLobbyConfirmCanva = headLocal.gameObject.transform.GetChild(0).gameObject;

        InputDevices.GetDevicesWithCharacteristics(lControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }

        previouFramePressure = false;
        confirmation = false;
    }

    //On X button pressed a confirm message appears: if you press again the X button you return to the lobby, if you press
    //Y you exit the confirm message and go back to the scene
    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);

        if (secondaryButtonValue)
        {
            if(previouFramePressure == false)
            {
                if (confirmation == false)
                {
                    backToLobbyConfirmCanva.gameObject.SetActive(true);
                    backToLobbyConfirmCanva.gameObject.transform.GetComponent<Canvas>().enabled = true;

                    confirmation = true;
                }
                else
                {
                    this.gameObject.transform.GetComponent<ReturnToLobby>().OnClick_ReturnToLobby();
                }
            }
            previouFramePressure = true;
        } else
        {
            previouFramePressure = false;
        }

        if (primaryButtonValue)
        {
            if(confirmation == true)
            {
                confirmation = false;

                backToLobbyConfirmCanva.gameObject.transform.GetComponent<Canvas>().enabled = false;
                backToLobbyConfirmCanva.gameObject.SetActive(false);
            }
        }
    }
}

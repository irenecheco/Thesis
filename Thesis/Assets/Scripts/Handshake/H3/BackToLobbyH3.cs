using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BackToLobbyH3 : MonoBehaviour
{
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDeviceCharacteristics lControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
    private InputDevice targetDevice;

    private bool previouFramePressure;
    private bool confirmation;

    private GameObject headLocal;
    private GameObject backToLobbyConfirmCanva;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);

        if (primaryButtonValue)
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

        if (secondaryButtonValue)
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

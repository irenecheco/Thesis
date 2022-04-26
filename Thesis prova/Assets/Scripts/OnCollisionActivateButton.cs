using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCollisionActivateButton : MonoBehaviour
{
    private string handshake_button = "Handshake Button";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected");
        GameObject.Find(handshake_button).GetComponent<Button>().interactable = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject.Find(handshake_button).GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
